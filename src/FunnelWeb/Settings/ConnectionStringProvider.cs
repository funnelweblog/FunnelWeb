using System.Configuration;
using System.Web.Configuration;
using FunnelWeb.DatabaseDeployer;

namespace FunnelWeb.Settings
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IBootstrapSettings settings;

        public ConnectionStringProvider(IBootstrapSettings settings)
        {
            this.settings = settings;
        }

        public string ConnectionString
        {
            get
            {
                var apphbConnectionString = ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"];

                if (!string.IsNullOrEmpty(apphbConnectionString))
                    return apphbConnectionString;

                return settings.Get("funnelweb.configuration.database.connection");
            }
            set
            {
                var apphbConnectionString = ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"];

                if (!string.IsNullOrEmpty(apphbConnectionString))
                {
                    var config = WebConfigurationManager.OpenWebConfiguration("~");
                    config.AppSettings.Settings["SQLSERVER_CONNECTION_STRING"].Value = value;
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");

                    return;
                }

                settings.Set("funnelweb.configuration.database.connection", value);
            }
        }

        public string Schema
        {
            get
            {
                return settings.Get("funnelweb.configuration.database.schema");
            }
            set
            {
                settings.Set("funnelweb.configuration.database.schema", value);
            }
        }

        public string DatabaseProvider
        {
            get { return (settings.Get("funnelweb.configuration.database.provider") ?? "sql").ToLower(); }
            set { settings.Set("funnelweb.configuration.database.provider", value); }
        }
    }
}