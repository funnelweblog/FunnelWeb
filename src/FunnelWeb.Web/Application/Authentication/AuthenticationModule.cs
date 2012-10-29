using Autofac;
using FunnelWeb.Authentication;
using FunnelWeb.Authentication.Internal;

namespace FunnelWeb.Web.Application.Authentication
{
    public class AuthenticationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //Sql Authentication should be registered last as it will fall back to Forms if it is not enabled
            // or if there is a database issue

            //Authenticators
            builder
                .RegisterType<FormsAuthenticator>()
                .As<IAuthenticator>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<SqlAuthenticator>()
                .As<IAuthenticator>()
                .AsSelf()
                .InstancePerLifetimeScope();
            
            //Role Providers
            builder.RegisterType<FormsRoleProvider>()
                .As<IRoleProvider>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<SqlRoleProvider>()
                .As<IRoleProvider>()
                .AsSelf()
                .InstancePerLifetimeScope();

            //Membership
            builder
                .RegisterType<FormsFunnelWebMembership>()
                .As<IFunnelWebMembership>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<SqlFunnelWebMembership>()
                .As<IFunnelWebMembership>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
