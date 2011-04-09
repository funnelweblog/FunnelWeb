using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DbUp;
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

            string error;
            var model = new IndexModel();
            model.CanConnect = Database.TryConnect(connectionString, out error);
            model.ConnectionError = error;
            model.ConnectionString = connectionString;

            if (model.CanConnect)
            {
                var required = Database
                    .GetCoreRequiredScripts()
                    .Union(
                        Extensions.SelectMany(x => Database.GetExtensionRequiredScripts(x)))
                    .ToArray();

                var executedAlready = Database
                    .GetCoreExecutedScripts(connectionString)
                    .Union(
                        Extensions.SelectMany(x => Database.GetExtensionExecutedScripts(connectionString, x)))
                    .ToArray();

                model.ScriptsToRun = required.Except(executedAlready).ToArray();
                model.IsInstall = executedAlready.Length > 0;
            }

            return View("Index", model);
        }

        [HttpPost]
        [ActionName("test")]
        public virtual ActionResult Test(string connectionString)
        {
            ConnectionStringProvider.ConnectionString = connectionString;
            UpgradeDetector.Reset();
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public virtual ActionResult Upgrade()
        {
            var writer = new StringWriter();
            var log = new TextLog(writer);
            var result = Database.PerformUpgrade(ConnectionStringProvider.ConnectionString, Extensions, log);
            UpgradeDetector.Reset();
            
            return View("UpgradeReport", new UpgradeModel(result, writer.ToString()));
        }

        private class TextLog : ILog
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