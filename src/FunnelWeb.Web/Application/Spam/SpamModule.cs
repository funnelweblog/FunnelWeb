using Autofac;

namespace FunnelWeb.Web.Application.Spam
{
    public class SpamModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AkismetSpamChecker>().As<ISpamChecker>()
                .InstancePerDependency();
        }
    }
}
