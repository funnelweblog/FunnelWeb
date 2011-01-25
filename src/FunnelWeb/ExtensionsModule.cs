using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Web.Routing;
using Autofac;

namespace FunnelWeb
{
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
                var controller = extension as RoutableFunnelWebExtension;

                if (controller != null)
                {
                    controller.Routes = routes;
                    controller.Initialize(builder);
                    controller.RegisterControllers(builder);
                }
                else
                {
                    extension.Initialize(builder);
                }
                builder.RegisterInstance(export.Metadata).As<FunnelWebExtensionAttribute>();
            }
        }
    }
}