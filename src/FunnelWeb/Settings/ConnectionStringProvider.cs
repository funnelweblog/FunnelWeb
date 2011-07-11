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
                return settings.Get("funnelweb.configuration.database.connection");
            }
            set
            {
                settings.Set("funnelweb.configuration.database.connection", value);
            }
        }

        public string Schema
        {
            get
            {
                return settings.Get("funnelweb.configuration.database.schema") ?? "dbo";
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