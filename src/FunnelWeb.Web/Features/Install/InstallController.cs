using System.Web.Mvc;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.DatabaseDeployer.Infrastructure;
using FunnelWeb.Web.Application.Installation;
using FunnelWeb.Web.Features.Install.Views;

namespace FunnelWeb.Web.Features.Install
{
    [ValidateInput(false)]
    public partial class InstallController : Controller
    {
        public IApplicationDatabase Database { get; set; }
        public IConnectionStringProvider ConnectionStringProvider { get; set; }

        [Authorize]
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
                model.CurrentVersion = Database.GetCurrentVersion(connectionString);
                model.NewVersion = Database.GetApplicationVersion(connectionString);
            }

            return View("Index", model);
        }

        [HttpPost]
        [Authorize]
        [ActionName("test")]
        public virtual ActionResult Test(string connectionString)
        {
            ConnectionStringProvider.ConnectionString = connectionString;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult Upgrade()
        {
            var log = new Log();
            var result = Database.PerformUpgrade(ConnectionStringProvider.ConnectionString, log);
            return View("UpgradeReport", new UpgradeModel(result, log));
        }
    }
}