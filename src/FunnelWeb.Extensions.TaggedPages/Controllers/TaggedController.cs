using System;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;

namespace FunnelWeb.Extensions.TaggedPages.Controllers
{
    [FunnelWebExtension(FullName = "Display posts by tags", Publisher = "FunnelWeb", SupportLink = "http://www.funnelweblog.com")]
    public class TaggedController : Controller, IRoutableFunnelWebExtension
    {
        void IFunnelWebExtension.Initialize(ContainerBuilder builder)
        {
            throw new NotImplementedException();
        }

        RouteCollection IRoutableFunnelWebExtension.Routes { get; set; }
    }
}
