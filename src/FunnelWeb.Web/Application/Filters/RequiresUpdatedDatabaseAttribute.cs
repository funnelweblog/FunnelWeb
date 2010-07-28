using System.Web;
using System.Web.Mvc;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Web.Application.Installation;

namespace FunnelWeb.Web.Application.Filters
{
    public class RequiresUpdatedDatabaseAttribute : ActionFilterAttribute
    {
        public IApplicationDatabase ApplicationDatabase { get; set; }
        public IConnectionStringProvider ConnectionStringProvider { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (ApplicationDatabase == null) ApplicationDatabase = new ApplicationDatabase();
            if (ConnectionStringProvider == null) ConnectionStringProvider = new ConnectionStringProvider();

            var connectionString = ConnectionStringProvider.ConnectionString;

            string error;
            if (ApplicationDatabase.TryConnect(connectionString, out error))
            {
                var currentVersion = ApplicationDatabase.GetCurrentVersion(connectionString);
                var requiredVersion = ApplicationDatabase.GetApplicationVersion(connectionString);
                if (currentVersion == requiredVersion)
                {
                    return;
                }
            }

            filterContext.Result = new RedirectResult("~/login?databaseIssue=true");
        }
    }
}