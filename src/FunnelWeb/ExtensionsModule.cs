using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Web.Routing;
using Autofac;
using DbUp.ScriptProviders;
using FunnelWeb.DatabaseDeployer;

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
                    var ex = new ScriptedExtension(
                        requiresScripts.SourceIdentifier,
                        extension.GetType().Assembly,
                        new EmbeddedScriptProvider(
                            extension.GetType().Assembly,
                            script => script.EndsWith(".sql")));

                    builder.RegisterInstance(ex);
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