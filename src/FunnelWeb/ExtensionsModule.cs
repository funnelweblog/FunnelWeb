using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Web.Routing;
using Autofac;
using FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders;

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
        public IEnumerable<Lazy<IFunnelWebExtension, IFunnelWebExtensionMetaData>> Extensions { get; set; }

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
                var requiresScripts = extension as IRequireDatabaseScripts;

                if (requiresScripts != null)
                {
                    builder
                        .RegisterInstance(new EmbeddedSqlScriptProvider(
                                              export.Metadata.FullName,
                                              extension.GetType().Assembly,
                                              version =>
                                              string.Format(requiresScripts.ScriptNameFormat,
                                                            version.ToString().PadLeft(4, '0'))))
                        .As<IScriptProvider>();
                }

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
                builder.RegisterInstance(export.Metadata).As<IFunnelWebExtensionMetaData>();
            }
        }
    }
}