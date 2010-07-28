using System.Web.Configuration;
using System.Web.Mvc;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.DatabaseDeployer.Infrastructure;
using FunnelWeb.Web.Application.Filters;
using FunnelWeb.Web.Application.Installation;

namespace FunnelWeb.Web.Controllers
{
    [ValidateInput(false)]
    public partial class InstallController : Controller
    {
        private readonly IApplicationDatabase _database;
        private readonly IConnectionStringProvider _connectionStringProvider;

        public InstallController(IApplicationDatabase database, IConnectionStringProvider connectionStringProvider)
        {
            _database = database;
            _connectionStringProvider = connectionStringProvider;
        }

        [Authorize]
        public ActionResult Index()
        {
            var connectionString = _connectionStringProvider.ConnectionString;

            string error;
            var model = new IndexModel();
            model.CanConnect = _database.TryConnect(connectionString, out error);
            model.ConnectionError = error;
            model.ConnectionString = connectionString;
            if (model.CanConnect)
            {
                model.CurrentVersion = _database.GetCurrentVersion(connectionString);
                model.NewVersion = _database.GetApplicationVersion(connectionString);
            }

            return View("Index", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize]
        [ActionName("test")]
        public ActionResult Test(string connectionString)
        {
            _connectionStringProvider.ConnectionString = connectionString;
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize]
        public ActionResult Upgrade()
        {
            var log = new Log();
            var result = _database.PerformUpgrade(_connectionStringProvider.ConnectionString, log);
            return View("UpgradeReport", new UpgradeModel(result, log));
        }
    }
}