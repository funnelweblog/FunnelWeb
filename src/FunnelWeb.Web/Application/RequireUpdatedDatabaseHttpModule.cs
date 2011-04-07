using System;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.DatabaseDeployer;

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
            if (!DependencyResolver.Current.GetService<IDatabaseUpgradeDetector>().UpdateNeeded()) return;

            var path = HttpContext.Current.Request.Path;
            path = path.ToLowerInvariant();
            if (path.Contains("/login") || path.Contains("/install") || path.Contains("/content"))
            {
                return;
            }

            HttpContext.Current.Response.Redirect("~/admin/login?databaseIssue=true");
        }

        public void Dispose()
        {
        }
    }
}