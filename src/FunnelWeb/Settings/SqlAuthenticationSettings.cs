using System;
using System.ComponentModel;

namespace FunnelWeb.Settings
{
    public class SqlAuthSettings : ISettings
    {
        [DisplayName("Sql Authentication Enabled")]
        [DefaultValue(false)]
        [Description("True if sql authentication is enabled")]
        [SettingStorage(StorageLocation.Database, "sql-authentication")]
        public bool SqlAuthenticationEnabled { get; set; }
    }
}
