using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Web.Mvc;
using DbUp.Engine.Output;
using FunnelWeb.Authentication.Internal;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Providers;
using FunnelWeb.Providers.Database;
using FunnelWeb.Settings;
using FunnelWeb.Web.Areas.Admin.Views.Install;

namespace FunnelWeb.Web.Areas.Admin.Controllers
{
	[ValidateInput(false)]
	[Authorize]
	// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
	public class InstallController : Controller
	{
		private readonly IDatabaseProvider databaseProvider;
		private readonly Func<IProviderInfo<IDatabaseProvider>> databaseProvidersInfo;
		private readonly ISettingsProvider settingsProvider;
		private readonly IFederatedAuthenticationService federatedAuthenticationService;

		// ReSharper disable UnusedAutoPropertyAccessor.Global
		public IApplicationDatabase Database { get; set; }
		public IConnectionStringSettings ConnectionStringSettings { get; set; }
		public IDatabaseUpgradeDetector UpgradeDetector { get; set; }
		public IEnumerable<ScriptedExtension> Extensions { get; set; }

		// ReSharper restore UnusedAutoPropertyAccessor.Global

		//public InstallController(Func<IProviderInfo<IDatabaseProvider>> databaseProvidersInfo)
		public InstallController(IDatabaseProvider databaseProvider, Func<IProviderInfo<IDatabaseProvider>> databaseProvidersInfo, ISettingsProvider settingsProvider, IFederatedAuthenticationService federatedAuthenticationService)
		{
			this.databaseProvider = databaseProvider;
			this.databaseProvidersInfo = databaseProvidersInfo;
			this.settingsProvider = settingsProvider;
			this.federatedAuthenticationService = federatedAuthenticationService;
		}

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.View, Resource = Authorization.Resources.Install.Index)]
		public virtual ActionResult Index()
		{
			var connectionString = ConnectionStringSettings.ConnectionString;
			var schema = ConnectionStringSettings.Schema;
			var databaseProviderName = ConnectionStringSettings.DatabaseProvider;

			string error;
			var model = new IndexModel
											{
												DatabaseProviders = new[] { "sql", "sqlce" },
												//DatabaseProviders = providerInfo.Keys,
												DatabaseProvider = databaseProviderName,
												CanConnect = databaseProvider.TryConnect(connectionString, out error),
												ConnectionError = error,
												ConnectionString = connectionString,
												Schema = databaseProvider.SupportSchema ? schema : null,
												DatabaseProviderSupportsSchema = databaseProvider.SupportSchema,
												IsSettingsReadOnly = ConnectionStringSettings.ReadOnlyReason != null,
												ReadOnlyReason = ConnectionStringSettings.ReadOnlyReason
											};

			if (model.CanConnect)
			{
				var connectionFactory = databaseProvider.GetConnectionFactory(connectionString);
				var required = Database
						.GetCoreRequiredScripts(connectionFactory)
						.Union(Extensions.SelectMany(x => Database.GetExtensionRequiredScripts(connectionFactory, x)))
						.ToArray();

				var executedAlready = Database
						.GetCoreExecutedScripts(connectionFactory)
						.Union(Extensions.SelectMany(x => Database.GetExtensionExecutedScripts(connectionFactory, x)))
						.ToArray();

				model.ScriptsToRun = required.Except(executedAlready).ToArray();
				model.IsInstall = executedAlready.Length > 0;
			}

			return View("Index", model);
		}

		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.Update, Resource = Authorization.Resources.Install.ChangeProvider)]
		public ActionResult ChangeProvider(string databaseProvider)
		{
			var provider = databaseProvidersInfo().GetProviderByName(databaseProvider);

			ConnectionStringSettings.ConnectionString = provider.DefaultConnectionString;
			ConnectionStringSettings.DatabaseProvider = databaseProvider;
			if (!provider.SupportSchema)
			{
				ConnectionStringSettings.Schema = null;				
			}

			UpgradeDetector.Reset();

			return RedirectToAction("Index");
		}

		[HttpPost]
		[ActionName("test")]
		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.View, Resource = Authorization.Resources.Install.Test)]
		public virtual ActionResult Test(string connectionString, string schema)
		{
			ConnectionStringSettings.ConnectionString = connectionString;
			ConnectionStringSettings.Schema = schema;
			UpgradeDetector.Reset();

			return RedirectToAction("Index");
		}

		[HttpPost]
		[ClaimsPrincipalPermission(SecurityAction.Demand, Operation = Authorization.Operations.Update, Resource = Authorization.Resources.Install.Upgrade)]
		public virtual ActionResult Upgrade()
		{
			var writer = new StringWriter();
			var log = new TextLog(writer);
			var result = Database.PerformUpgrade(Extensions, log);
			UpgradeDetector.Reset();

			var upgradeModel = new UpgradeModel(result, writer.ToString());

			if (settingsProvider.GetSettings<SqlAuthSettings>().SqlAuthenticationEnabled && upgradeModel.Results.All(x => x.Successful))
			{
				// We have upgraded and now we must sign back in as as sql user!
				federatedAuthenticationService.Logout();
			}

			return View("UpgradeReport", upgradeModel);
		}

		private class TextLog : IUpgradeLog
		{
			private readonly StringWriter writer;

			public TextLog(StringWriter writer)
			{
				this.writer = writer;
			}

			public void WriteInformation(string format, params object[] args)
			{
				writer.WriteLine("INFO:  " + string.Format(format, args));
			}

			public void WriteError(string format, params object[] args)
			{
				writer.WriteLine("ERROR: " + string.Format(format, args));
			}

			public void WriteWarning(string format, params object[] args)
			{
				writer.WriteLine("WARN:  " + string.Format(format, args));
			}
		}
	}
}