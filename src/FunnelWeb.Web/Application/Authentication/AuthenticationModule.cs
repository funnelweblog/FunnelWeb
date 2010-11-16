using System.Configuration;
using Autofac;

namespace FunnelWeb.Web.Application.Authentication
{
    public class AuthenticationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var useSqlMembershipSetting = ConfigurationManager.AppSettings["funnelweb.configuration.useSqlMembership"];

            bool useSqlMembership;
            if (bool.TryParse(useSqlMembershipSetting, out useSqlMembership) && useSqlMembership)
                builder.RegisterType<SqlAuthenticator>().As<IAuthenticator>().SingleInstance();
            else
                builder.RegisterType<FormsAuthenticator>().As<IAuthenticator>().SingleInstance();
        }
    }
}
