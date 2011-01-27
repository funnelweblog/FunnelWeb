using Autofac;

namespace FunnelWeb.Extensions.SqlAuthentication
{
    [FunnelWebExtension(FullName = "Multi User SQL Authentication", Publisher = "FunnelWeb", SupportLink = "http://code.google.com/p/funnelweb")]
    public class Extension : IFunnelWebExtension, IRequireDatabaseScripts
    {
        public void Initialize(ContainerBuilder builder)
        {
        }

        public string ScriptNameFormat
        {
            get { return "FunnelWeb.Extensions.SqlAuthentication.Scripts.Script{0}.sql"; }
        }
    }
}
