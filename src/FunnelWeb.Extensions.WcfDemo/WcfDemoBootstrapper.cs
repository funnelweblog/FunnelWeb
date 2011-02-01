using System.Collections.Generic;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Autofac;
using FunnelWeb.Extensions.WcfDemo.Services;
using Microsoft.ServiceModel.Description;
using Microsoft.ServiceModel.Dispatcher;
using Microsoft.ServiceModel.Http;

namespace FunnelWeb.Extensions.WcfDemo
{
    [FunnelWebExtension(FullName = "WCF Demo", Publisher = "FunnelWeb", SupportLink = "http://code.google.com/p/funnelweb")]
    public class WcfDemoBootstrapper : RoutableFunnelWebExtension
    {
        public override void Initialize(ContainerBuilder builder)
        {
            Routes.AddServiceRoute<DemoService>("services/demoservice", new HttpHostConfiguration());
            Routes.AddServiceRoute<JsonService>("services/jsonservice", new JsonValueSampleConfiguration());
        }
    }

     public class JsonValueSampleConfiguration : HttpHostConfiguration, IProcessorProvider
    {
        public void RegisterRequestProcessorsForOperation(HttpOperationDescription operation, IList<Processor> processors, MediaTypeProcessorMode mode)
        {
            processors.Add(new FormUrlEncodedProcessor(operation, mode));
            processors.Add(new JsonProcessor(operation, mode));

        }
        public void RegisterResponseProcessorsForOperation(HttpOperationDescription operation, IList<Processor> processors, MediaTypeProcessorMode mode)
        {
            processors.ClearMediaTypeProcessors();
            processors.Add(new JsonProcessor(operation, mode));
        }
    }


}
