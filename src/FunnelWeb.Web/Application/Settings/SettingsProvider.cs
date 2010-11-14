using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;
using FunnelWeb.Web.Model;
using FunnelWeb.Web.Model.Repositories;

namespace FunnelWeb.Web.Application.Settings
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly object @lock = new object();
        private readonly IAdminRepository repository;
        private readonly Func<string> themesDirectoryPath;
        private Settings settings;

        public SettingsProvider(IAdminRepository repository, Func<string> themesDirectoryPath)
        {
            this.repository = repository;
            this.themesDirectoryPath = themesDirectoryPath;
        }

        public Settings GetSettings()
        {
            if (settings == null)
            {
                lock (@lock)
                {
                    if (settings == null)
                    {
                        LoadSettings();
                    }
                }
            }

            return settings;
        }

        private void LoadSettings()
        {
            settings = new Settings();
            var settingMetadata = ReadSettingMetadata();
            var databaseSettings = repository.GetSettings().ToList();
            var webConfigSettings = WebConfigurationManager.AppSettings;

            foreach (var setting in settingMetadata)
            {
                // Initialize with default values
                setting.Write(settings, setting.DefaultValue);

                // Write over it using the stored value
                switch (setting.Storage.Location)
                {
                    case StorageLocation.Database:
                        var dbSetting = databaseSettings.FirstOrDefault(x => x.Name == setting.Storage.Key);
                        if (dbSetting != null)
                        {
                            setting.Write(settings, dbSetting.Value);
                        }
                        break;
                    case StorageLocation.Custom:
                        if (setting.Property.Name == "Themes")
                        {
                            var themeFolder = new DirectoryInfo(themesDirectoryPath());
                            var themes = themeFolder.GetDirectories().Select(x => x.Name).OrderBy(x => x).ToArray();
                            setting.Write(settings, themes);
                        }
                        else
                        {
                            throw new NotSupportedException(string.Format("Could not read the custom setting '{0}'", setting.Property.Name));
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void SaveSettings(Settings settingsToSave)
        {
            settings = settingsToSave;
            var settingsMetadata = ReadSettingMetadata();
            var databaseSettings = repository.GetSettings().ToList();

            foreach (var setting in settingsMetadata)
            {
                // Write over it using the stored value
                switch (setting.Storage.Location)
                {
                    case StorageLocation.Database:
                        var value = setting.Read(settings);
                        var dbSetting = databaseSettings.FirstOrDefault(x => x.Name == setting.Storage.Key);
                        if (dbSetting != null)
                        {
                            dbSetting.Value = value ?? setting.DefaultValue as string ?? string.Empty;
                        }
                        else
                        {
                            databaseSettings.Add(new Setting { Description = setting.Description, DisplayName = setting.DisplayName, Name = setting.Storage.Key, Value = value ?? setting.DefaultValue as string ?? string.Empty });
                        }
                        break;
                    case StorageLocation.Custom:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            repository.Save(databaseSettings);
        }

        private static SettingDescriptor[] ReadSettingMetadata()
        {
            return typeof (Settings).GetProperties()
                .OfType<PropertyInfo>()
                .Where(x => x.GetCustomAttributes(true).OfType<SettingStorageAttribute>().Any())
                .Select(x => new SettingDescriptor(x))
                .ToArray();
        }

        private class SettingDescriptor
        {
            private readonly PropertyInfo property;
            private object defaultValue;
            private string description;
            private string displayName;
            private SettingStorageAttribute storage;

            public SettingDescriptor(PropertyInfo property)
            {
                this.property = property;
                displayName = property.Name;

                ReadAttribute<DefaultValueAttribute>(d => defaultValue = d.Value);
                ReadAttribute<DescriptionAttribute>(d => description = d.Description);
                ReadAttribute<DisplayNameAttribute>(d => displayName = d.DisplayName);
                ReadAttribute<SettingStorageAttribute>(d => storage = d);
            }

            private void ReadAttribute<TAttribute>(Action<TAttribute> callback)
            {
                var instances = property.GetCustomAttributes(typeof (TAttribute), true).OfType<TAttribute>();
                foreach (var instance in instances)
                {
                    callback(instance);
                }
            }

            public PropertyInfo Property
            {
                get { return property; }
            }

            public object DefaultValue
            {
                get { return defaultValue; }
            }

            public string Description
            {
                get { return description; }
            }

            public string DisplayName
            {
                get { return displayName; }
            }

            public SettingStorageAttribute Storage
            {
                get { return storage; }
            }

            public void Write(Settings settings, object value)
            {
                if (value != null)
                {
                    var converted = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(settings, converted, null);
                }
            }

            public string Read(Settings settings)
            {
                return (property.GetValue(settings, null) ?? string.Empty).ToString();
            }
        }
    }
}