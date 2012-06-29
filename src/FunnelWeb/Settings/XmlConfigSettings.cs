using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace FunnelWeb.Settings
{
    /// <summary>
    /// FunnelWeb initially used Web.config. But uploading new releases of FunnelWeb meant a complicated web.config merge for users. 
    /// Now we use a custom XML file (My.config) which contains user-specified information.
    /// </summary>
    public class XmlConfigSettings : IConfigSettings
    {
        private readonly XmlSerializer serializer = new XmlSerializer(typeof(FunnelWebConfiguration));
        private readonly string bootstrapSettingsFilePath;

        public XmlConfigSettings(string bootstrapSettingsFilePath)
        {
            this.bootstrapSettingsFilePath = bootstrapSettingsFilePath;
        }

        /// <summary>
        /// Gets a setting by the specified name. Returns null if the setting is not set.
        /// </summary>
        /// <param name="name">The setting name.</param>
        /// <returns></returns>
        public string Get(string name)
        {
            var config = OpenConfiguration();
            return config.Get(name);
        }

        /// <summary>
        /// Sets the specified setting.
        /// </summary>
        /// <param name="name">The setting name.</param>
        /// <param name="value">The value to set it to.</param>
        public void Set(string name, string value)
        {
            var config = OpenConfiguration();
            config.Set(name, value);
            if (config.HasChanged)
            {
                SaveConfiguration(config);
            }
        }

        public bool ConfigFileMissing()
        {
            return !File.Exists(bootstrapSettingsFilePath);
        }

        public string ReadOnlyReason { get { return null; } }

        private FunnelWebConfiguration OpenConfiguration()
        {
            if (ConfigFileMissing())
            {
                return new FunnelWebConfiguration();
            }
            using (var stream = new StreamReader(bootstrapSettingsFilePath))
            {
                return (FunnelWebConfiguration)serializer.Deserialize(stream);
            }
        }
        private void SaveConfiguration(FunnelWebConfiguration config)
        {
            using (var writer = new StreamWriter(bootstrapSettingsFilePath))
            {
                serializer.Serialize(writer, config);
                writer.Flush();
            }
        }

        [XmlRoot("funnelweb")]
        public class FunnelWebConfiguration
        {
            public FunnelWebConfiguration()
            {
                Settings = new List<Setting>();
                HasChanged = false;
            }

            [XmlIgnore]
            public bool HasChanged { get; set; }

            public void Set(string name, string value)
            {
                var existing = Settings.FirstOrDefault(x => x.Key == name);
                if (existing == null)
                {
                    existing = new Setting { Key = name };
                    Settings.Add(existing);
                }

                if (existing.Value == value)
                {
                    return;
                }

                existing.Value = value;
                HasChanged = true;
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