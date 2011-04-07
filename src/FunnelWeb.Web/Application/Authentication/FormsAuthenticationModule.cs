using Autofac;
using FunnelWeb.Authentication;
using FunnelWeb.Authentication.Internal;

namespace FunnelWeb.Web.Application.Authentication
{
    public class FormsAuthenticationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<FormsAuthenticator>().As<IAuthenticator>().AsSelf().SingleInstance();
            builder.RegisterType<FormsRoleProvider>().As<IRoleProvider>().SingleInstance();
        }
    }
}
