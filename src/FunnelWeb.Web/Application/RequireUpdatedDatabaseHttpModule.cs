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
            context.Error += ApplicationError;
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

        private static void ApplicationError(object sender, EventArgs e)
        {
            DependencyResolver.Current.GetService<IDatabaseUpgradeDetector>().Reset();
        }

        public void Dispose()
        {
        }
    }
}