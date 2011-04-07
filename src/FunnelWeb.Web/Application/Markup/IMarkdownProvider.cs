namespace FunnelWeb.Web.Application.Markup
{
    public interface IMarkdownProvider
    {
        string Render(string text, bool sanitize = false);
    }
}