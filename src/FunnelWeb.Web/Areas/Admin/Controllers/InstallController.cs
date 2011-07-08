using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DbUp.Engine.Output;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Web.Areas.Admin.Views.Install;

namespace FunnelWeb.Web.Areas.Admin.Controllers
{
    [ValidateInput(false)]
    [Authorize(Roles = "Admin")]
    public class InstallController : Controller
    {
        public IApplicationDatabase Database { get; set; }
        public IConnectionStringProvider ConnectionStringProvider { get; set; }
        public IDatabaseUpgradeDetector UpgradeDetector { get; set; }
        public IEnumerable<ScriptedExtension> Extensions { get; set; }

        public virtual ActionResult Index()
        {
            var connectionString = ConnectionStringProvider.ConnectionString;
            var schema = ConnectionStringProvider.Schema;

            string error;
            var model = new IndexModel();
            model.CanConnect = Database.TryConnect(connectionString, out error);
            model.ConnectionError = error;
            model.ConnectionString = connectionString;
            model.Schema = schema;

            if (model.CanConnect)
            {
                var required = Database
                    .GetCoreRequiredScripts()
                    .Union(Extensions.SelectMany(x => Database.GetExtensionRequiredScripts(x)))
                    .ToArray();

                var executedAlready = Database
                    .GetCoreExecutedScripts(connectionString, schema)
                    .Union(Extensions.SelectMany(x => Database.GetExtensionExecutedScripts(connectionString, x, schema)))
                    .ToArray();

                model.ScriptsToRun = required.Except(executedAlready).ToArray();
                model.IsInstall = executedAlready.Length > 0;
            }

            return View("Index", model);
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
            var result = Database.PerformUpgrade(ConnectionStringProvider.ConnectionString, ConnectionStringProvider.Schema, Extensions, log);
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