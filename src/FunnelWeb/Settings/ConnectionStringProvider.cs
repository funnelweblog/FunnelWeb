using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace FunnelWeb.Settings
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public string ConnectionString
        {
            get
            {

                return ConfigurationManager.AppSettings["funnelweb.configuration.database.connection"];
            }
            set
            {
                var config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                config.AppSettings.Settings["funnelweb.configuration.database.connection"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }
    }
}