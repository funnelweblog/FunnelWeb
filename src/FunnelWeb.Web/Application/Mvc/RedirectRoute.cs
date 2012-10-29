using System.Web;
using System.Web.Routing;

namespace FunnelWeb.Web.Application.Mvc
{
    public class RedirectRoute : RegexRoute
    {
        private int _responseCode = 301;
        public int ResponseCode
        {
            get { return _responseCode; }
            set { _responseCode = value; }
        }

        public RedirectRoute(string urlPattern, IRouteHandler routeHandler)
            : base(urlPattern, routeHandler)
        { }

        public RedirectRoute(string urlPattern, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(urlPattern, defaults, routeHandler)
        {
           
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var route = base.GetRouteData(httpContext);

            if (route != null)
            {
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = _responseCode;

                var url = GetRequestUrl(httpContext);
                var replace = UrlRegex.Replace(url, ReplacePattern);
                httpContext.Response.AppendHeader("Location", replace);
                httpContext.Response.End();
            }

            return null;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            //this route doesnt make urls
            return null;
        }

        public string ReplacePattern { get; set; }
    }
}