namespace FunnelWeb.Settings
{
    public interface ISettingsProvider
    {
        Settings GetSettings();
        void SaveSettings(Settings settings);
    }
}