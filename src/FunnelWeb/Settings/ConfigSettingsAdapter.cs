using Microsoft.WindowsAzure.ServiceRuntime;

namespace FunnelWeb.Settings
{
    public class ConfigSettingsAdapter : IConfigSettings
    {
        private readonly XmlConfigSettings myConfigSettings;

        public ConfigSettingsAdapter(XmlConfigSettings myConfigSettings)
        {
            this.myConfigSettings = myConfigSettings;
        }

        public string Get(string name)
        {
            return (RoleEnvironment.IsAvailable)
                        ? RoleEnvironment.GetConfigurationSettingValue(name)
                        : myConfigSettings.Get(name);
        }

        public void Set(string name, string value)
        {
            if (!RoleEnvironment.IsAvailable)
            {
                myConfigSettings.Set(name, value);
            }
        }

        public bool ConfigFileMissing()
        {
            return !RoleEnvironment.IsAvailable && myConfigSettings.ConfigFileMissing();
        }

        public string ReadOnlyReason
        {
            get
            {
                if (RoleEnvironment.IsAvailable)
                    return "You must update your FunnelWeb configuration from the Azure Configuration Portal";

                return myConfigSettings.ReadOnlyReason;
            }
        }
    }
}