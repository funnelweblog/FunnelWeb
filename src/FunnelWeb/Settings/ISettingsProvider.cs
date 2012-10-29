namespace FunnelWeb.Settings
{
    public interface ISettingsProvider
    {
        T GetSettings<T>() where T : ISettings;
        T GetDefaultSettings<T>() where T : ISettings;
        void SaveSettings<T>(T settings) where T : ISettings;
    }
}