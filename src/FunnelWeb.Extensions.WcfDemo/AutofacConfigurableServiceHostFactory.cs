using System;
using System.ServiceModel;
using Autofac.Integration.Wcf;
using Microsoft.ServiceModel.Http;

namespace FunnelWeb.Extensions.WcfDemo
{
    public class AutofacConfigurableServiceHostFactory : AutofacServiceHostFactory, IConfigurableServiceHostFactory
    {
        // Methods
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new WebHttpServiceHost(serviceType, Configuration, baseAddresses);
        }

        public HttpHostConfiguration Configuration { get; set; }
    }
}