using System;
using System.Globalization;
using System.Web;

namespace FunnelWeb.Web.Application
{
    public class ForceLowercaseUrlHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
        }

        private static void BeginRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;

            if (context.Request.Url.AbsolutePath.EndsWith(".axd") || context.Request.HttpMethod == "POST")
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

            if (context.Request.Url.ToString() != idealUrl)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", idealUrl);
                HttpContext.Current.Response.End();
            }
        }

        public void Dispose()
        {
        }
    }
}