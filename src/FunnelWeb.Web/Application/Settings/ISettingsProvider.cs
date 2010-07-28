namespace FunnelWeb.Web.Application.Settings
{
    public interface ISettingsProvider
    {
        string SiteTitle { get; }
        string Introduction { get; }
        string MainLinks { get; }
        string Footer { get; }
        string DefaultPage { get; }
        string Author { get; }
        string SearchDescription { get; }
        string SearchKeywords { get; }
        string SpamWords { get; }
    }
}