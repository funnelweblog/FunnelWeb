using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Markup
{
    /// <summary>
    /// Renders content (trusted or untrusted) by resolving a formatter that matches the given content format (e.g., HTML vs. Markdown).
    /// </summary>
    public interface IContentRenderer
    {
        string RenderTrusted(string content, string format, HtmlHelper html);
        string RenderUntrusted(string content, string format, HtmlHelper html);
    }
}