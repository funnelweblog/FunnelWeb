using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Xml.Linq;
using System.Xml.Serialization;
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
    }

    public interface IBootstrapSettings
    {
        string Get(string name);
        void Set(string name, string value);
    }

    /// <summary>
    /// FunnelWeb initially used Web.config.  
    /// </summary>
    public class XmlBootstrapSettings : IBootstrapSettings
    {
        private readonly XmlSerializer serializer = new XmlSerializer(typeof(FunnelWebConfiguration));

        private string ConfigurationFilePath
        {
            get { return Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "My.config"); }
        }

        private FunnelWebConfiguration OpenConfiguration()
        {
            if (!File.Exists(ConfigurationFilePath))
            {
                return new FunnelWebConfiguration();
            }
            using (var stream = new StreamReader(ConfigurationFilePath))
            {
                return (FunnelWebConfiguration)serializer.Deserialize(stream);
            }
        }

        private void SaveConfiguration(FunnelWebConfiguration config)
        {
            using (var writer = new StreamWriter(ConfigurationFilePath))
            {
                serializer.Serialize(writer, config);
            }
        }

        public string Get(string name)
        {
            var config = OpenConfiguration();
            return config.Get(name);
        }

        public void Set(string name, string value)
        {
            var config = OpenConfiguration();
            config.Set(name, value);
            SaveConfiguration(config);
        }

        [XmlRoot("funnelweb")]
        public class FunnelWebConfiguration
        {
            public FunnelWebConfiguration()
            {
                Settings = new List<Setting>();
            }

            public void Set(string name, string value)
            {
                var existing = Settings.FirstOrDefault(x => x.Key == name);
                if (existing == null)
                {
                    existing = new Setting();
                    Settings.Add(existing);
                }
                existing.Key = name;
                existing.Value = value;
            }

            public string Get(string name)
            {
                var existing = Settings.FirstOrDefault(x => x.Key == name);
                if (existing == null)
                {
                    return null;
                }
                return existing.Value;
            }

            [XmlElement("setting")]
            public List<Setting> Settings { get; set; }
        }

        public class Setting
        {
            [XmlAttribute("key")]
            public string Key { get; set; }

            [XmlAttribute("value")]
            public string Value { get; set; }
        }
    }
}