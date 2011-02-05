using System;
using Autofac;
using FunnelWeb.Extensions.WcfDemo.Services;
using Microsoft.ServiceModel.Http;

namespace FunnelWeb.Extensions.WcfDemo
{
    [FunnelWebExtension(FullName = "WCF Demo", Publisher = "FunnelWeb", SupportLink = "http://code.google.com/p/funnelweb")]
    public class WcfDemoBootstrapper : RoutableFunnelWebExtension, IRequireDatabaseScripts
    {
        public override void Initialize(ContainerBuilder builder)
        {
            builder.RegisterType<DemoService>();
            Routes.AddServiceRoute<DemoService, AutofacConfigurableServiceHostFactory>("services/demoservice", new JsonSupportConfiguration());
        }

        public string SourceIdentifier
        {
            get { return "FunnelWeb.Extensions.WcfDemo"; }
        }

        public string ScriptNameFormat
        {
            get { return "FunnelWeb.Extensions.WcfDemo.Scripts.Script{0}.sql"; }
        }
    }
}
