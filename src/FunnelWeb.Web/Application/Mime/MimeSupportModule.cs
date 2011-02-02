using Autofac;

namespace FunnelWeb.Web.Application.Mime
{
    public class MimeSupportModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegistryMimeTypeLookup>().As<IMimeTypeLookup>().SingleInstance();
        }
    }
}
