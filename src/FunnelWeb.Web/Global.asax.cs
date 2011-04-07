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
            ExtensionModel.EnableAspNetIntegration(extensionsPath);
        }

        private void Application_Start()
        {
            var container = BuildContainer();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            AutofacHostFactory.Container = container;

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new FunnelWebViewEngine(() => container.Resolve<ISettingsProvider>(), container.Resolve<IDatabaseUpgradeDetector>()));

            ControllerBuilder.Current.SetControllerFactory(new FunnelWebControllerFactory(container));
        }

        private static IContainer BuildContainer()
        {
            IContainer container = null;

            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            // FunnelWeb Database
            builder.RegisterModule(new DatabaseModule(() => new ConnectionStringProvider().ConnectionString));

            // FunnelWeb Core
            builder.RegisterModule(new SettingsModule());
            builder.RegisterModule(new TasksModule(() => container));    // HACK: Need a better way to enable the TasksModule to create lifetime scopes from the root
            builder.RegisterModule(new RepositoriesModule());
            builder.RegisterModule(new EventingModule());
            builder.RegisterModule(new ExtensionsModule(extensionsPath, RouteTable.Routes));

            // FunnelWeb Web
            builder.RegisterModule(new WebAbstractionsModule());
            builder.RegisterModule(new FormsAuthenticationModule());
            builder.RegisterModule(new BindersModule(ModelBinders.Binders));
            builder.RegisterModule(new MimeSupportModule());
            builder.RegisterModule(new ThemesModule());
            builder.RegisterModule(new SpamModule());
            builder.RegisterModule(new RoutesModule(RouteTable.Routes));

            return container = builder.Build();
        }
    }
}