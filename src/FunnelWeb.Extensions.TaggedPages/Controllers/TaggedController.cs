using System;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;

namespace FunnelWeb.Extensions.TaggedPages.Controllers
{
    [FunnelWebExtension(FullName = "Display posts by tags", Publisher = "FunnelWeb", SupportLink = "http://www.funnelweblog.com")]
    public class TaggedController : Controller, IRoutableFunnelWebExtension
    {
        public ActionResult Index(string tag)
        {
            throw new NotImplementedException();
            return View();
        }

        public void Initialize(ContainerBuilder builder)
        {
            Routes.MapRoute("tagged", "tagged/{*tag}", new { controller = "Tagged", action = "Index"});
        }

        public RouteCollection Routes { get; set; }
    }
}
