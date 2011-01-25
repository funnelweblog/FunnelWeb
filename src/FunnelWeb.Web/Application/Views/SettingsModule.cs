using System;
using System.Web;
using System.Web.Mvc;
using Autofac;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Settings;

namespace FunnelWeb.Web.Application.Views
{
    public class SettingsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new SettingsProvider(c.Resolve<IAdminRepository>(), () => c.Resolve<HttpContextBase>().Server.MapPath("~/Themes")))
                .As<ISettingsProvider>()
                .InstancePerLifetimeScope();

            builder.Register(c => new MarkdownProvider(
                c.Resolve<HttpContextBase>().Request.Url.GetLeftPart(UriPartial.Authority)))
                .As<IMarkdownProvider>()
                .InstancePerLifetimeScope();
        }
    }
}
