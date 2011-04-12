using Autofac;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Application.Markup
{
    public class MarkupModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ContentRenderer>()
                .As<IContentRenderer>()
                .InstancePerDependency();

            // Formatters
            builder
                .RegisterType<MarkdownFormatter>()
                .Named<IContentFormatter>(Formats.Markdown)
                .InstancePerDependency();

            builder
                .RegisterType<HtmlFormatter>()
                .Named<IContentFormatter>(Formats.Html)
                .InstancePerDependency();

            // Enrichers
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
}
