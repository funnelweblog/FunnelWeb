using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DbUp.Engine.Output;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Providers;
using FunnelWeb.Providers.Database;
using FunnelWeb.Web.Areas.Admin.Views.Install;

namespace FunnelWeb.Web.Areas.Admin.Controllers
{
    [ValidateInput(false)]
    [Authorize(Roles = "Admin")]
    public class InstallController : Controller
    {
        private readonly Func<IProviderInfo<IDatabaseProvider>> databaseProvidersInfo;
        public IApplicationDatabase Database { get; set; }
        public IConnectionStringSettings ConnectionStringSettings { get; set; }
        public IDatabaseUpgradeDetector UpgradeDetector { get; set; }
        public IEnumerable<ScriptedExtension> Extensions { get; set; }

        public InstallController(Func<IProviderInfo<IDatabaseProvider>> databaseProvidersInfo)
        {
            this.databaseProvidersInfo = databaseProvidersInfo;
        }

        public virtual ActionResult Index()
        {
            var connectionString = ConnectionStringSettings.ConnectionString;
            var schema = ConnectionStringSettings.Schema;
            var databaseProviderName = ConnectionStringSettings.DatabaseProvider;
            var providerInfo = databaseProvidersInfo();
            var databaseProvider = providerInfo.GetProviderByName(databaseProviderName);

            string error;
            var model = new IndexModel
                            {
                                DatabaseProviders = providerInfo.Keys,
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
                var required = Database
                    .GetCoreRequiredScripts()
                    .Union(Extensions.SelectMany(x => Database.GetExtensionRequiredScripts(x)))
                    .ToArray();

                var connectionFactory = databaseProvider.GetConnectionFactory(connectionString);
                var executedAlready = Database
                    .GetCoreExecutedScripts(connectionFactory)
                    .Union(Extensions.SelectMany(x => Database.GetExtensionExecutedScripts(connectionFactory, x)))
                    .ToArray();

                model.ScriptsToRun = required.Except(executedAlready).ToArray();
                model.IsInstall = executedAlready.Length > 0;
            }

            return View("Index", model);
        }

        public ActionResult ChangeProvider(string databaseProvider)
        {
            var provider = databaseProvidersInfo().GetProviderByName(databaseProvider);

            ConnectionStringSettings.ConnectionString = provider.DefaultConnectionString;
            ConnectionStringSettings.DatabaseProvider = databaseProvider;
            if (!provider.SupportSchema)
                ConnectionStringSettings.Schema = null;
            UpgradeDetector.Reset();
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("test")]
        public virtual ActionResult Test(string connectionString, string schema)
        {
            ConnectionStringSettings.ConnectionString = connectionString;
            ConnectionStringSettings.Schema = schema;
            UpgradeDetector.Reset();
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public virtual ActionResult Upgrade()
        {
            var writer = new StringWriter();
            var log = new TextLog(writer);
            var result = Database.PerformUpgrade(Extensions, log);
            UpgradeDetector.Reset();
            
            return View("UpgradeReport", new UpgradeModel(result, writer.ToString()));
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