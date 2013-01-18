using Autofac;

namespace FunnelWeb.Eventing
{
    public class EventingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(x => new AutofacEventPublisher(x)).AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
