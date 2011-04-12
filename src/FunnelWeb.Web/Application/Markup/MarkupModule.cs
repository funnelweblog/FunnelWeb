using System;
using System.Collections.Generic;
using System.Web;
using Autofac;
using Autofac.Features.Indexed;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Application.Markup
{
    public class MarkupModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<MarkdownFormatter>()
                .Named<IContentFormatter>(Formats.Markdown)
                .InstancePerDependency();

            builder
                .RegisterType<HtmlFormatter>()
                .Named<IContentFormatter>(Formats.Html)
                .InstancePerDependency();

            builder
                .RegisterType<ContentRenderer>()
                .As<IContentRenderer>()
                .InstancePerDependency();

            builder
                .RegisterType<MacroEvaluator>()
                .As<IContentEnricher>()
                .InstancePerDependency();

            builder
                .RegisterType<InputSanitizer>()
                .As<IContentEnricher>()
                .InstancePerDependency();
        }
    }

    /// <summary>
    /// Renders content (trusted or untrusted) by resolving a formatter that matches the given content format (e.g., HTML vs. Markdown).
    /// </summary>
    public interface IContentRenderer
    {
        string RenderTrusted(string content, string format);
        string RenderUntrusted(string content, string format);
    }

    public interface IContentFormatter
    {
        string Format(string content);
    }

    public class HtmlFormatter : IContentFormatter
    {
        public string Format(string content)
        {
            return content;
        }
    }

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

    public interface IContentEnricher
    {
        string Enrich(string content, bool isContentTrusted);
    }

    public class MacroEvaluator : IContentEnricher
    {
        public string Enrich(string content, bool isContentTrusted)
        {
            if (!isContentTrusted)
            {
                // Only evaluate macros in trusted content (e.g., posts by the site admin)
                return content;
            }

            return content.Replace("[Google]", "<iframe src='http://www.google.com'></iframe>");
        }
    }

    public class ContentRenderer : IContentRenderer
    {
        private readonly IIndex<string, IContentFormatter> formatters;
        private readonly IEnumerable<IContentEnricher> enrichers;
        
        public ContentRenderer(IIndex<string, IContentFormatter> formatters, IEnumerable<IContentEnricher> enrichers)
        {
            this.formatters = formatters;
            this.enrichers = enrichers;
        }

        public string RenderTrusted(string content, string format)
        {
            return Render(content, format, true);
        }

        public string RenderUntrusted(string content, string format)
        {
            return Render(content, format, false);
        }

        private string Render(string content, string format, bool trusted)
        {
            IContentFormatter formatter;
            if (!formatters.TryGetValue(format, out formatter))
            {
                throw new Exception("Unable to render content of format '{0}'. No provider is registered.");
            }

            content = formatter.Format(content);

            foreach (var enricher in enrichers)
            {
                content = enricher.Enrich(content, trusted);
            }

            return content;
        }
    }
}
