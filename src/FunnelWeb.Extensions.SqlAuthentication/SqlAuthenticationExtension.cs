using System.Web.Mvc;
using Autofac;
using FunnelWeb.Web.Application.Authentication;

namespace FunnelWeb.Extensions.SqlAuthentication
{
    [FunnelWebExtension(FullName = "Multi User SQL Authentication", Publisher = "FunnelWeb", SupportLink = "http://code.google.com/p/funnelweb")]
    public class SqlAuthenticationExtension : RoutableFunnelWebExtension, IRequireDatabaseScripts
    {
        public override void Initialize(ContainerBuilder builder)
        {
            builder
                .RegisterType<SqlAuthenticator>()
                .As<IAuthenticator>()
                .SingleInstance();

            Routes.MapRoute(null, "admin/sqlauthentication/{action}", new { controller = "SqlAuthentication", action = "Index" });
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
