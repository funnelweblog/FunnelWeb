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
    /// <summary>
    /// An extensibility point for FunnelWeb
    /// </summary>
    public interface IFunnelWebExtension
    {
        /// <summary>
        /// Initializes the extension, the Autofac container is also provided so that you can include items for DI
        /// </summary>
        /// <param name="builder">The builder.</param>
        void Initialize(ContainerBuilder builder);
    }

    /// <summary>
    /// An extensibility point for FunnelWeb which allows modification of the Route data for FunnelWeb. Remember though with great power comes great responsibility!
    /// </summary>
    public abstract class RoutableFunnelWebExtension : IFunnelWebExtension
    {
        /// <summary>
        /// Gets or sets the routes.
        /// </summary>
        /// <value>The routes.</value>
        protected internal RouteCollection Routes { get; internal set; }
        /// <summary>
        /// Initializes the extension, the Autofac container is also provided so that you can include items for DI
        /// </summary>
        /// <param name="builder">The builder.</param>
        public abstract void Initialize(ContainerBuilder builder);
        /// <summary>
        /// Registers the controllers based on FunnelWeb standards
        /// </summary>
        /// <remarks>If you are overriding this but don't call the base implementation your controllers may not be registered property</remarks>
        /// <param name="builder">The builder.</param>
        protected internal virtual void RegisterControllers(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.GetType().Assembly)
                .Where(x => typeof (IController).IsAssignableFrom(x) && x.Name.EndsWith("Controller"))
                .Named<IController>(x => x.Name.Replace("Controller", string.Empty))
                ;
        }
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
