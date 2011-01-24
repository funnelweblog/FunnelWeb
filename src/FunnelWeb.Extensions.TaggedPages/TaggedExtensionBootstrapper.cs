using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;

namespace FunnelWeb.Extensions.TaggedPages
{
    [FunnelWebExtension(FullName = "Display posts by tags", Publisher = "FunnelWeb", SupportLink = "http://www.funnelweblog.com")]
    public class TaggedExtensionBootstrapper : IRoutableFunnelWebExtension
    {
        public void Initialize(ContainerBuilder builder)
        {
            Routes.MapRoute("tagged", "tagged/{*tag}", new { controller = "Tagged", action = "Index" });

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
        }

        public RouteCollection Routes { get; set; }
    }
}
