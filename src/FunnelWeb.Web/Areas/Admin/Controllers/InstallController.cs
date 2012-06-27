using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DbUp.Engine.Output;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.DatabaseDeployer.DbProviders;
using FunnelWeb.Web.Areas.Admin.Views.Install;

namespace FunnelWeb.Web.Areas.Admin.Controllers
{
    [ValidateInput(false)]
    [Authorize(Roles = "Admin")]
    public class InstallController : Controller
    {
        private readonly Func<IEnumerable<Lazy<IDatabaseProvider, IDatabaseProviderMetadata>>> databaseProviders;
        public IApplicationDatabase Database { get; set; }
        public IConnectionStringProvider ConnectionStringProvider { get; set; }
        public IDatabaseUpgradeDetector UpgradeDetector { get; set; }
        public IEnumerable<ScriptedExtension> Extensions { get; set; }

        public InstallController(Func<IEnumerable<Lazy<IDatabaseProvider, IDatabaseProviderMetadata>>> databaseProviders)
        {
            this.databaseProviders = databaseProviders;
        }

        public virtual ActionResult Index()
        {
            var connectionString = ConnectionStringProvider.ConnectionString;
            var schema = ConnectionStringProvider.Schema;
            var databaseProviderName = ConnectionStringProvider.DatabaseProvider;
            var databaseProviderList = databaseProviders().ToList();
            var databaseProvider = databaseProviderList.Single(p => p.Metadata.Name.Equals(databaseProviderName, StringComparison.InvariantCultureIgnoreCase));

            string error;
            var model = new IndexModel();
            model.DatabaseProviders = databaseProviderList.Select(p => p.Metadata.Name);
            model.DatabaseProvider = databaseProviderName;
            model.CanConnect = databaseProvider.Value.TryConnect(connectionString, out error);
            model.ConnectionError = error;
            model.ConnectionString = connectionString;
            model.Schema = databaseProvider.Value.SupportSchema ? schema : null;
            model.DatabaseProviderSupportsSchema = databaseProvider.Value.SupportSchema;
            model.IsSettingsReadOnly = ConnectionStringProvider.ReadOnlyReason != null;
            model.ReadOnlyReason = ConnectionStringProvider.ReadOnlyReason;

            if (model.CanConnect)
            {
                var required = Database
                    .GetCoreRequiredScripts()
                    .Union(Extensions.SelectMany(x => Database.GetExtensionRequiredScripts(x)))
                    .ToArray();

                var connectionFactory = databaseProvider.Value.GetConnectionFactory(connectionString);
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
            var provider = databaseProviders().Single(p => p.Metadata.Name.Equals(databaseProvider, StringComparison.InvariantCultureIgnoreCase));

            ConnectionStringProvider.ConnectionString = provider.Value.DefaultConnectionString;
            ConnectionStringProvider.DatabaseProvider = databaseProvider;
            if (!provider.Value.SupportSchema)
                ConnectionStringProvider.Schema = null;
            UpgradeDetector.Reset();
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("test")]
        public virtual ActionResult Test(string connectionString, string schema)
        {
            ConnectionStringProvider.ConnectionString = connectionString;
            ConnectionStringProvider.Schema = schema;
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