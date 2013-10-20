using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Eventing;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Providers;
using FunnelWeb.Settings;
using FunnelWeb.Tasks;
using FunnelWeb.Web.App_Start;
using FunnelWeb.Web.Application;
using FunnelWeb.Web.Application.Authentication;
using FunnelWeb.Web.Application.Markup;
using FunnelWeb.Web.Application.MetaWeblog;
using FunnelWeb.Web.Application.Mime;
using FunnelWeb.Web.Application.Mvc;
using FunnelWeb.Web.Application.Mvc.Binders;
using FunnelWeb.Web.Application.Spam;
using FunnelWeb.Web.Application.Themes;
using StackExchange.Profiling;

namespace FunnelWeb.Web
{
	public class MvcApplication : HttpApplication
	{
		private static string extensionsPath;

		public static void BeforeApplicationStart()
		{
			extensionsPath = HostingEnvironment.MapPath("~/bin/Extensions") ?? string.Empty;
			if (Directory.Exists(extensionsPath))
			{
				Extensibility.EnableAspNetIntegration(extensionsPath);
			}
		}

		internal static IContainer BuildContainer()
		{
			var builder = new ContainerBuilder();
			builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();

			// FunnelWeb Database
			builder.RegisterModule(new DatabaseModule());

			// FunnelWeb Core
			builder.RegisterModule(new SettingsModule(HostingEnvironment.MapPath("~/My.config")));
			builder.RegisterModule(new TasksModule());
			builder.RegisterModule(new InternalProviderRegistrationModule());
			builder.RegisterModule(new RepositoriesModule());
			builder.RegisterModule(new EventingModule());
			builder.RegisterModule(new ExtensionsModule(extensionsPath, RouteTable.Routes));
			builder.RegisterType<MetaWeblog>().As<IMetaWeblog>().InstancePerLifetimeScope();

			// FunnelWeb Web
			builder.RegisterModule(new WebAbstractionsModule());
			builder.RegisterModule(new MarkupModule());
			builder.RegisterModule(new AuthenticationModule());
			builder.RegisterModule(new BindersModule(ModelBinders.Binders));
			builder.RegisterModule(new MimeSupportModule());
			builder.RegisterModule(new ThemesModule());
			builder.RegisterModule(new SpamModule());

			return builder.Build();
		}

		protected void Application_BeginRequest()
		{
			MiniProfiler.Start();
		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				MiniProfiler.Stop(true);
			}
		}

		private void Application_Start()
		{
			MvcHandler.DisableMvcResponseHeader = true;

			AreaRegistration.RegisterAllAreas();

			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			var container = BuildContainer();

			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new FunnelWebViewEngine());

			ControllerBuilder.Current.SetControllerFactory(new FunnelWebControllerFactory(container));

			var federatedAuthenticationConfigurator = container.Resolve<IFederatedAuthenticationConfigurator>();
			federatedAuthenticationConfigurator.InitiateFederatedAuthentication();
		}

		protected void Application_EndRequest()
		{
			MiniProfiler.Stop();
		}
	}
}