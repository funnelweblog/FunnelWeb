using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;

namespace FunnelWeb
{
    public interface IFunnelWebExtension
    {
        void Initialize(ContainerBuilder builder);
    }

    public interface IRoutableFunnelWebExtension : IFunnelWebExtension, IController
    {
        RouteCollection Routes { get; set; } 
    }

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FunnelWebExtensionAttribute : ExportAttribute
    {
        public FunnelWebExtensionAttribute()
            : base(typeof(IFunnelWebExtension))
        {
            
        }

        public FunnelWebExtensionAttribute(IDictionary<string, object> something)
            : base(typeof(IFunnelWebExtension))
        {
            
        }

        public string FullName { get; set; }
        public string SupportLink { get; set; }
        public string Publisher { get; set; }
    }

    public class ExtensionsModule : Module
    {
        private readonly string extensionsPath;
        private readonly RouteCollection routes;

        public ExtensionsModule(string extensionsPath, RouteCollection routes)
        {
            this.extensionsPath = extensionsPath;
            this.routes = routes;
        }

        [ImportMany]
        public IEnumerable<Lazy<IFunnelWebExtension, FunnelWebExtensionAttribute>> Extensions { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            if (!Directory.Exists(extensionsPath))
                return;

            var container = new CompositionContainer(
                new DirectoryCatalog(extensionsPath)
                );

            container.SatisfyImportsOnce(this);
            foreach (var export in Extensions)
            {
                var extension = export.Value;
                var controller = extension as IRoutableFunnelWebExtension;
                
                if (controller != null)
                {
                    controller.Routes = routes;
                    builder.RegisterInstance(controller)
                        .AsImplementedInterfaces();
                }

                extension.Initialize(builder);
                builder.RegisterInstance(export.Metadata).As<FunnelWebExtensionAttribute>();
            }
        }
    }
}
