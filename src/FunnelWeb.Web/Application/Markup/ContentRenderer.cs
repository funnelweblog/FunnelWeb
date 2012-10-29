using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Autofac.Features.Indexed;

namespace FunnelWeb.Web.Application.Markup
{
    /// <summary>
    /// Renders content by looking up an appropriate <see cref="IContentFormatter"/> that matches the given format name (e.g., HTML or Markdown).
    /// </summary>
    public class ContentRenderer : IContentRenderer
    {
        private readonly IIndex<string, IContentFormatter> formatters;
        private readonly IEnumerable<IContentEnricher> enrichers;
        
        public ContentRenderer(IIndex<string, IContentFormatter> formatters, IEnumerable<IContentEnricher> enrichers)
        {
            this.formatters = formatters;
            this.enrichers = enrichers;
        }

        public string RenderTrusted(string content, string format, HtmlHelper html)
        {
            return Render(content, format, true, html);
        }

        public string RenderUntrusted(string content, string format, HtmlHelper html)
        {
            return Render(content, format, false, html);
        }

        private string Render(string content, string format, bool trusted, HtmlHelper html)
        {
            IContentFormatter formatter;
            if (!formatters.TryGetValue(format, out formatter))
            {
                throw new Exception("Unable to render content of format '{0}'. No provider is registered.");
            }

            content = formatter.Format(content);

            foreach (var enricher in enrichers)
            {
                content = enricher.Enrich(content, trusted, html);
            }

            return content;
        }
    }
}