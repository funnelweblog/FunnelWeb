namespace FunnelWeb.Web.Application.Settings
{
    public interface ISettingsProvider
    {
        Settings GetSettings();
        void SaveSettings(Settings settings);
    }
}