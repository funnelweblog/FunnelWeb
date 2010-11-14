using System;
using System.Web;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Settings;

namespace FunnelWeb.Web.Application
{
    public class RequireUpdatedDatabaseHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += ApplicationBeginRequest;
        }

        private static void ApplicationBeginRequest(object sender, EventArgs e)
        {
            var path = HttpContext.Current.Request.Path;
            path = path.ToLowerInvariant();
            if (path.Contains("/login") || path.Contains("/install") || path.Contains("/content"))
            {
                return;
            }

            var applicationDatabase = new ApplicationDatabase();
            var connectionStringProvider = new ConnectionStringProvider();

            var connectionString = connectionStringProvider.ConnectionString;

            string error;
            if (applicationDatabase.TryConnect(connectionString, out error))
            {
                var currentVersion = applicationDatabase.GetCurrentVersion(connectionString);
                var requiredVersion = applicationDatabase.GetApplicationVersion(connectionString);
                if (currentVersion == requiredVersion)
                {
                    return;
                }
            }

            HttpContext.Current.Response.Redirect("~/login?databaseIssue=true");
        }

        public void Dispose()
        {
            
        }
    }
}