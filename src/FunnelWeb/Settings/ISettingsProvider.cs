namespace FunnelWeb.Settings
{
	public interface ISettingsProvider
	{
		bool TryGetSettings<T>(out T t) where T : ISettings;
		T GetSettings<T>() where T : ISettings;
		T GetDefaultSettings<T>() where T : ISettings;
		void SaveSettings<T>(T settings) where T : ISettings;
	}
}