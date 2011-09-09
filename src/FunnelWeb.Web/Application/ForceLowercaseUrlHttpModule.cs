using System;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FunnelWeb.Web.Application
{
    public class ForceLowercaseUrlHttpModule : IHttpModule
    {
        private static readonly string[] extensions = new[] { ".js", ".css", ".jpg", ".jpeg", ".gif", ".ico", ".png" };

        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
        }

        private static void BeginRequest(object sender, EventArgs e)
        {
            var context = new FunnelWeb.Routing.HttpContextDecorator(HttpContext.Current.Request.RequestContext.HttpContext);

            if (context.Request.Url.AbsolutePath.StartsWith("get", StringComparison.InvariantCultureIgnoreCase) || context.Request.Url.AbsolutePath.StartsWith("/get", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            if (context.Request.Url.AbsolutePath.EndsWith(".axd", StringComparison.InvariantCultureIgnoreCase) || context.Request.HttpMethod == "POST" || extensions.Any(x => context.Request.Url.AbsolutePath.EndsWith(x, StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }

            var idealUrl = context.Request.Url.GetLeftPart(UriPartial.Path).ToLower(CultureInfo.InvariantCulture);
            if (idealUrl.EndsWith("/") && context.Request.Url.AbsolutePath.LastIndexOf('/') > 0)
            {
                idealUrl = idealUrl.Substring(0, idealUrl.LastIndexOf('/'));
            }
            if (!string.IsNullOrEmpty(context.Request.Url.Query))
            {
                idealUrl += context.Request.Url.Query;
            }

            if (context.Request.Url.AbsoluteUri == idealUrl || context.Request.Url.AbsoluteUri == idealUrl + "/")
                return;

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Status = "301 Moved Permanently";
            HttpContext.Current.Response.AddHeader("Location", idealUrl);
            HttpContext.Current.Response.End();
        }

        public void Dispose()
        {
        }
    }
}
