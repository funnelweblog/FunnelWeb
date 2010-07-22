using FunnelWeb.Web.Application.Settings;

namespace FunnelWeb.Web.Application.Views
{
    public interface IApplicationView
    {
        ISettingsProvider Settings { get; }
    }
}