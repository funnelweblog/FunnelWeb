using Autofac;
using FunnelWeb.Extensions.WcfDemo.Services;
using Microsoft.ServiceModel.Http;

namespace FunnelWeb.Extensions.WcfDemo
{
    [FunnelWebExtension(FullName = "WCF Demo", Publisher = "FunnelWeb", SupportLink = "http://code.google.com/p/funnelweb")]
    public class WcfDemoBootstrapper : RoutableFunnelWebExtension
    {
        public override void Initialize(ContainerBuilder builder)
        {
            Routes.AddServiceRoute<DemoService>("services/demoservice", new HttpHostConfiguration());
        }
    }
}
