using System;
using System.Web;

namespace FunnelWeb.Web.Application.Markup
{
    public class MarkdownFormatter : IContentFormatter
    {
        private readonly HttpRequestBase request;

        public MarkdownFormatter(HttpRequestBase request)
        {
            this.request = request;
        }

        public string Format(string content)
        {
            var renderer = new MarkdownRenderer(request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath);
            return renderer.Render(content);
        }
    }
}