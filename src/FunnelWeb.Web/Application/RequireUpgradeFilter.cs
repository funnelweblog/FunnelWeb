using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FunnelWeb.DatabaseDeployer;

namespace FunnelWeb.Web.Application
{
    public class RequireUpgradeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (!DependencyResolver.Current.GetService<IDatabaseUpgradeDetector>().UpdateNeeded()) return;

            var path = HttpContext.Current.Request.Path;
            path = path.ToLowerInvariant();
            if (path.Contains("/login") || path.Contains("/install") || path.Contains("/content"))
            {
                return;
            }

            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                                                                                          {
                                                                                              action = "Login",
                                                                                              controller = "Login",
                                                                                              area = "Admin"
                                                                                          }));
        }
    }
}