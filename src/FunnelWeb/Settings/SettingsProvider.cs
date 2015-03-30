using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using FunnelWeb.Model;
using FunnelWeb.Model.Repositories;
using NHibernate.Exceptions;

namespace FunnelWeb.Settings
{
	public class SettingsProvider : ISettingsProvider
	{
		private readonly object @lock = new object();
		private readonly Func<IAdminRepository> repository;
		private readonly Dictionary<Type, ISettings> settingsStore = new Dictionary<Type, ISettings>();

		public SettingsProvider(Func<IAdminRepository> repository)
		{
			this.repository = repository;
		}

		public bool TryGetSettings<T>(out T t) where T : ISettings
		{
			var settingsType = typeof(T);
			if (settingsStore.ContainsKey(settingsType))
			{
				t = (T)settingsStore[settingsType];
				return true;
			}

			lock (@lock)
			{
				if (!settingsStore.ContainsKey(settingsType))
				{
					try
					{
						LoadSettings<T>();
						t = (T) settingsStore[settingsType];
						return true;
					}
					catch (GenericADOException)
					{
						
					}
					catch (SqlException)
					{
						
					}
				}
			}

			t = default(T);
			return false;
		}

		public T GetSettings<T>() where T : ISettings
		{
			T t;

			if (!TryGetSettings(out t))
			{
				throw new ApplicationException(string.Format("Unable to load settings for '{0}'", typeof(T).Name));
			}

			return t;
		}

		public T GetDefaultSettings<T>() where T : ISettings
		{
			var settings = Activator.CreateInstance<T>();
			var settingMetadata = ReadSettingMetadata<T>();

			foreach (var setting in settingMetadata)
			{
				// Initialize with default values
				setting.Write(settings, setting.DefaultValue);
			}

			return settings;
		}

		private void LoadSettings<T>() where T : ISettings
		{
			var settings = Activator.CreateInstance<T>();
			settingsStore.Add(typeof(T), settings);
			var settingMetadata = ReadSettingMetadata<T>();
			var databaseSettings = repository().GetSettings().ToList();

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
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public void SaveSettings<T>(T settingsToSave) where T : ISettings
		{
			var settingsType = typeof(T);
			if (settingsStore.ContainsKey(settingsType))
				settingsStore[settingsType] = settingsToSave;
			else
				settingsStore.Add(settingsType, settingsToSave);

			var settingsMetadata = ReadSettingMetadata<T>();
			var databaseSettings = repository().GetSettings().ToList();

			foreach (var setting in settingsMetadata)
			{
				// Write over it using the stored value
				switch (setting.Storage.Location)
				{
					case StorageLocation.Database:
						var value = setting.Read(settingsToSave);
						var dbSetting = databaseSettings.FirstOrDefault(x => x.Name == setting.Storage.Key);
						if (dbSetting != null)
						{
							dbSetting.Value = value ?? setting.DefaultValue as string ?? string.Empty;
						}
						else
						{
							databaseSettings.Add(new Setting
							{
								Description = setting.Description,
								DisplayName = setting.DisplayName,
								Name = setting.Storage.Key,
								Value = value ?? setting.DefaultValue as string ?? string.Empty
							});
						}
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			repository().Save(databaseSettings);
		}

		private static IEnumerable<SettingDescriptor> ReadSettingMetadata<T>()
		{
			return typeof(T).GetProperties()
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
				var instances = property.GetCustomAttributes(typeof(TAttribute), true).OfType<TAttribute>();
				foreach (var instance in instances)
				{
					callback(instance);
				}
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

			public void Write(ISettings settings, object value)
			{
				if (value != null)
				{
					var converted = Convert.ChangeType(value, property.PropertyType);
					property.SetValue(settings, converted, null);
				}
			}

			public string Read(ISettings settings)
			{
				return (property.GetValue(settings, null) ?? string.Empty).ToString();
			}
		}
	}
}