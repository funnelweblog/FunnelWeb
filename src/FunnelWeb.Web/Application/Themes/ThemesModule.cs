using Autofac;

namespace FunnelWeb.Web.Application.Themes
{
    public class ThemesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ThemeProvider>().As<IThemeProvider>().InstancePerDependency();
        }
    }
}