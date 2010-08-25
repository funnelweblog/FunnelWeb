using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FunnelWeb.Web
{
    public class HyphenatedRouteHandler : MvcRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            requestContext.RouteData.Values["controller"] = requestContext.RouteData.Values["controller"].ToString().Replace("-", "");
            requestContext.RouteData.Values["action"] = requestContext.RouteData.Values["action"].ToString().Replace("-", "");
            return base.GetHttpHandler(requestContext);
        }
    }
}