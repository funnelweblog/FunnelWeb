using System;

namespace FunnelWeb.Web.Application.Settings
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingStorageAttribute : Attribute
    {
        private readonly StorageLocation location;
        private readonly string key;

        public SettingStorageAttribute(StorageLocation location) : this(location, null)
        {
        }

        public SettingStorageAttribute(StorageLocation location, string key)
        {
            this.location = location;
            this.key = key;
        }

        public StorageLocation Location
        {
            get { return location; }
        }

        public string Key
        {
            get { return key; }
        }
    }
}