namespace FunnelWeb.Web.Application.Views
{
    public interface IMarkdownProvider
    {
        string Render(string text, bool sanitize = false);
    }
}