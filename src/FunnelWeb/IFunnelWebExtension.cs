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
}
