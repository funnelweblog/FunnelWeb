using System.Web.Mvc;

namespace FunnelWeb.Web.Application.Markup
{
    public interface IContentEnricher
    {
        string Enrich(string content, bool isContentTrusted, HtmlHelper html);
    }
}