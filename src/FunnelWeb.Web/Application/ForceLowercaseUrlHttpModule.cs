using System;
using System.Globalization;
using System.Linq;
using System.Web;
using FunnelWeb.Utilities;

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
            var request = HttpContext.Current.Request;
            var uri = request.GetOriginalUrl();

            if (uri.AbsolutePath.StartsWith("get", StringComparison.InvariantCultureIgnoreCase) || uri.AbsolutePath.StartsWith("/get", StringComparison.InvariantCultureIgnoreCase) ||
                uri.AbsolutePath.StartsWith("upload", StringComparison.InvariantCultureIgnoreCase) || uri.AbsolutePath.StartsWith("/upload", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            if (uri.AbsolutePath.EndsWith(".axd", StringComparison.InvariantCultureIgnoreCase) || request.HttpMethod == "POST" || extensions.Any(x => uri.AbsolutePath.EndsWith(x, StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }

            var idealUrl = uri.GetLeftPart(UriPartial.Path).ToLower(CultureInfo.InvariantCulture);
            if (idealUrl.EndsWith("/") && uri.AbsolutePath.LastIndexOf('/') > 0)
            {
                idealUrl = idealUrl.Substring(0, idealUrl.LastIndexOf('/'));
            }
            if (!string.IsNullOrEmpty(uri.Query))
            {
                idealUrl += uri.Query;
            }

            if (uri.AbsoluteUri == idealUrl || uri.AbsoluteUri == idealUrl + "/")
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
