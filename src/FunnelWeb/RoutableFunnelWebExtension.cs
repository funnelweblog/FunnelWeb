using System.Web.Mvc;
using System.Web.Routing;
using Autofac;

namespace FunnelWeb
{
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
            builder.RegisterAssemblyTypes(GetType().Assembly)
                .Where(t => t.Name.EndsWith("Controller"))
                .Named<IController>(t => t.Name.Replace("Controller", string.Empty))
                .PropertiesAutowired()
                ;
        }
    }
}