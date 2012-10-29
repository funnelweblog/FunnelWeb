using System;
using System.Linq;
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

            var path = context.Request.FilePath;
            var extensions = new[] { ".js", ".css", ".jpg", ".jpeg", ".gif", ".ico", ".png" };
            if (extensions.Any(ext => path.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase)))
            {
                context.Response.Cache.SetExpires(DateTime.Now.AddDays(90));
            }
        }

        public void Dispose()
        {
        }
    }
}
