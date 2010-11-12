using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace FunnelWeb.Web.Application.Installation
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public string ConnectionString
        {
            get
            {
                var config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                return config.ConnectionStrings.ConnectionStrings["funnelweb.configuration.database.connection"].ConnectionString;
            }
            set
            {
                var config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                config.ConnectionStrings.ConnectionStrings["funnelweb.configuration.database.connection"].ConnectionString = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }
    }
}