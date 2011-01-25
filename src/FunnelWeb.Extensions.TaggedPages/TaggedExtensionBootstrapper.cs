using System.Web.Mvc;
using Autofac;

namespace FunnelWeb.Extensions.TaggedPages
{
    [FunnelWebExtension(FullName = "Display posts by tags", Publisher = "FunnelWeb", SupportLink = "http://www.funnelweblog.com")]
    public class TaggedExtensionBootstrapper : RoutableFunnelWebExtension
    {
        public override void Initialize(ContainerBuilder builder)
        {
            Routes.MapRoute(null, "tagged/{*tag}", new { controller = "Tagged", action = "Index" }, null, new[] { "FunnelWeb.Extensions.TaggedPages.Controllers" });
        }
    }
}
