using System.Collections.Generic;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.ServiceModel.Description;
using Microsoft.ServiceModel.Dispatcher;
using Microsoft.ServiceModel.Http;

namespace FunnelWeb.Extensions.WcfDemo
{
    public class JsonSupportConfiguration : HttpHostConfiguration, IProcessorProvider
    {
        public void RegisterRequestProcessorsForOperation(HttpOperationDescription operation, IList<Processor> processors, MediaTypeProcessorMode mode)
        {
            processors.Add(new FormUrlEncodedProcessor(operation, mode));
            processors.Add(new JsonProcessor(operation, mode));

        }
        public void RegisterResponseProcessorsForOperation(HttpOperationDescription operation, IList<Processor> processors, MediaTypeProcessorMode mode)
        {
            processors.Add(new JsonProcessor(operation, mode));
        }
    }
}