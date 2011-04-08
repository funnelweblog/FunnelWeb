using System;
using System.Web;
using Autofac;

namespace FunnelWeb.Web.Application.Markup
{
    public class MarkupModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MarkdownProvider(
                c.Resolve<HttpContextBase>().Request.Url.GetLeftPart(UriPartial.Authority)))
                .As<IMarkdownProvider>()
                .InstancePerLifetimeScope();
        }
    }
}
