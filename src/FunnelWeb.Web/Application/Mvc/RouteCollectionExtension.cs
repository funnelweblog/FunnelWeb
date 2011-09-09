using System.Web.Mvc;
using System.Web.Routing;

namespace FunnelWeb.Web.Application.Mvc
{
    public static class RouteCollectionExtension
    {
        public static Route MapLowerCaseRoute(this RouteCollection routes, string url, object defaults)
        {
            return routes.MapLowerCaseRoute(url, defaults, null);
        }

        public static Route MapLowerCaseRoute(this RouteCollection routes, string url, object defaults, object constraints)
        {
            Route route = new LowercaseRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints)
            };

            routes.Add(null, route);

            return route;
        }

        public static Route MapHyphenatedRoute(this RouteCollection routes, string url, object defaults)
        {
            Route route = new Route(
                                url,
                                new RouteValueDictionary(defaults),
                                new HyphenatedRouteHandler()
                                );
            routes.Add(route);

            return route;
        }
    }
}