using System;
using System.Web;

namespace FunnelWeb.Web.Application
{
    public class CacheStaticFilesHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
        }

        private static void BeginRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;

            var path = context.Request.FilePath.ToLowerInvariant();
            if (path.EndsWith(".js")
                || path.ToLower().EndsWith(".css")
                || path.ToLower().EndsWith(".jpg")
                || path.ToLower().EndsWith(".jpeg")
                || path.ToLower().EndsWith(".gif")
                || path.ToLower().EndsWith(".ico")
                || path.ToLower().EndsWith(".png"))
            {
                context.Response.Cache.SetExpires(DateTime.Now.AddDays(90));
            }
        }

        public void Dispose()
        {
        }
    }
}
