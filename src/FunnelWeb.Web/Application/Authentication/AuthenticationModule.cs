using Autofac;

namespace FunnelWeb.Web.Application.Authentication
{
    public class AuthenticationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<FormsAuthenticator>().As<IAuthenticator>().SingleInstance();
        }
    }
}
