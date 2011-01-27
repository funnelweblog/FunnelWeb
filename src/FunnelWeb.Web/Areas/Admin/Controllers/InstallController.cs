using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.DatabaseDeployer.Infrastructure;
using FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders;
using FunnelWeb.Settings;
using FunnelWeb.Web.Areas.Admin.Views.Install;

namespace FunnelWeb.Web.Areas.Admin.Controllers
{
    [ValidateInput(false)]
    [Authorize]
    public class InstallController : Controller
    {
        public IApplicationDatabase Database { get; set; }
        public IConnectionStringProvider ConnectionStringProvider { get; set; }
        public IEnumerable<IScriptProvider> ExtensionScriptProviders { get; set; }

        public virtual ActionResult Index()
        {
            var connectionString = ConnectionStringProvider.ConnectionString;

            string error;
            var model = new IndexModel
                            {
                                CanConnect = Database.TryConnect(connectionString, out error),
                                ConnectionError = error,
                                ConnectionString = connectionString
                            };
            if (model.CanConnect)
            {
                model.CurrentVersion = Database.GetApplicationCurrentVersion(connectionString);
                model.NewVersion = Database.GetApplicationVersion();

                model.ExtensionVersions = ExtensionScriptProviders.Select(GetExtensionVersion);
            }

            return View("Index", model);
        }

        private ExtensionVersion GetExtensionVersion(IScriptProvider extensionScriptProvider)
        {
            return new ExtensionVersion
                       {
                           ExtensionName = extensionScriptProvider.SourceIdentifier,
                           CurrentVersion =
                               Database.GetExtensionCurrentVersion(ConnectionStringProvider.ConnectionString,
                                                                   extensionScriptProvider),
                           NewVersion = Database.GetExtensionVersion(extensionScriptProvider)
                       };
        }

        [HttpPost]
        [ActionName("test")]
        public virtual ActionResult Test(string connectionString)
        {
            ConnectionStringProvider.ConnectionString = connectionString;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public virtual ActionResult Upgrade()
        {
            var log = new Log();
            var result = Database.PerformUpgrade(ConnectionStringProvider.ConnectionString, ExtensionScriptProviders, log);
            return View("UpgradeReport", new UpgradeModel(result, log));
        }
    }
}