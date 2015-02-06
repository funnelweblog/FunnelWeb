using System;
using System.Web;
using System.Web.Mvc;
using FunnelWeb.DatabaseDeployer;

namespace FunnelWeb.Web.Application
{
	public class RequireUpdatedDatabaseHttpModule : IHttpModule
	{
		private HttpApplication httpApplication;

		public void Init(HttpApplication httpApplication)
		{
			this.httpApplication = httpApplication;
			httpApplication.BeginRequest += ApplicationBeginRequest;
			httpApplication.Error += ApplicationError;
		}

		private static void ApplicationBeginRequest(object sender, EventArgs e)
		{
			if (!DependencyResolver.Current.GetService<IDatabaseUpgradeDetector>().UpdateNeeded()) return;

			var path = HttpContext.Current.Request.Path;
			path = path.ToLowerInvariant();
			if (path.EndsWith(".js") ||
				path.EndsWith(".css") ||
				path.Contains("/login") ||
				path.Contains("/install") ||
				path.Contains("/content"))
			{
				return;
			}

			HttpContext.Current.Response.Redirect("~/admin/login?databaseIssue=true");
		}

		private static void ApplicationError(object sender, EventArgs e)
		{
			DependencyResolver.Current.GetService<IDatabaseUpgradeDetector>().Reset();
		}

		public void Dispose() { }
	}
}