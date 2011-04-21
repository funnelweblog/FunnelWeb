using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.Wcf;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Eventing;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Settings;
using FunnelWeb.Tasks;
using FunnelWeb.Web.Application;
using FunnelWeb.Web.Application.Authentication;
using FunnelWeb.Web.Application.Markup;
using FunnelWeb.Web.Application.Mime;
using FunnelWeb.Web.Application.Mvc;
using FunnelWeb.Web.Application.Mvc.Binders;
using FunnelWeb.Web.Application.Spam;
using FunnelWeb.Web.Application.Themes;

namespace FunnelWeb.Web
{
    /// <summary>
    /// Entry point for the FunnelWeb application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        private static string extensionsPath;

        public static void BeforeApplicationStart()
        {
            extensionsPath = HostingEnvironment.MapPath("~/bin/Extensions") ?? string.Empty;
            Extensibility.EnableAspNetIntegration(extensionsPath);
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            // FunnelWeb Database
            builder.RegisterModule(new DatabaseModule());

            // FunnelWeb Core
            builder.RegisterModule(new SettingsModule(HostingEnvironment.MapPath("~/My.config")));
            builder.RegisterModule(new TasksModule());
            builder.RegisterModule(new RepositoriesModule());
            builder.RegisterModule(new EventingModule());
            builder.RegisterModule(new ExtensionsModule(extensionsPath, RouteTable.Routes));

            // FunnelWeb Web
            builder.RegisterModule(new WebAbstractionsModule());
            builder.RegisterModule(new MarkupModule());
            builder.RegisterModule(new FormsAuthenticationModule());
            builder.RegisterModule(new BindersModule(ModelBinders.Binders));
            builder.RegisterModule(new MimeSupportModule());
            builder.RegisterModule(new ThemesModule());
            builder.RegisterModule(new SpamModule());
            builder.RegisterModule(new MarkupModule());
            builder.RegisterModule(new RoutesModule(RouteTable.Routes));

            return builder.Build();
        }

        private void Application_Start()
        {
            var container = BuildContainer();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            AutofacHostFactory.Container = container;

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new FunnelWebViewEngine());

            ControllerBuilder.Current.SetControllerFactory(new FunnelWebControllerFactory(container));
        }
    }
}