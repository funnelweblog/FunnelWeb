using Autofac;
using FunnelWeb.Web.Application.Authentication;

namespace FunnelWeb.Extensions.SqlAuthentication
{
    [FunnelWebExtension(FullName = "Multi User SQL Authentication", Publisher = "FunnelWeb", SupportLink = "http://code.google.com/p/funnelweb")]
    public class Extension : IFunnelWebExtension, IRequireDatabaseScripts
    {
        public void Initialize(ContainerBuilder builder)
        {
            builder
                .RegisterType<SqlAuthenticator>()
                .As<IAuthenticator>()
                .SingleInstance();
        }

        public string FullName { get; set; }
        public string SupportLink { get; set; }
        public string Publisher { get; set; }

        public string ScriptNameFormat
        {
            get { return "FunnelWeb.Extensions.SqlAuthentication.Scripts.Script{0}.sql"; }
        }
    }
}
