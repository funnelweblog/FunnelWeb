using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.Wcf;
using Bindable;
using FunnelWeb.DatabaseDeployer.Infrastructure.ScriptProviders;
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
using FunnelWeb.Web.Application.Views;

namespace FunnelWeb.Web
{
    /// <summary>
    /// Entry point for the FunnelWeb application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        private static string _extensionsPath;

        public static void Initialise()
        {
            // Add assemblies containing IModules to the BuildManager
            _extensionsPath = HostingEnvironment.MapPath("~/bin/Extensions") ?? string.Empty;

            if (!Directory.Exists(_extensionsPath))
            {
                try
                {
                    Directory.CreateDirectory(_extensionsPath);
                }
                catch (IOException ex)
                {
                    // oh, well nothing really we can do
                    Trace.WriteLine("Could not create extensions directory:" + _extensionsPath + "\r\n" + ex.Message);
                }
            }

            if (!Directory.Exists(_extensionsPath)) return;

            var catalog = new DirectoryCatalog(_extensionsPath);
            var compositionContainer = new CompositionContainer(catalog);
            IEnumerable<IFunnelWebExtension> modules;
            try
            {
                modules = compositionContainer.GetExportedValues<IFunnelWebExtension>();
            }
            catch (ReflectionTypeLoadException ex)
            {
                if (ex.LoaderExceptions.Length > 0)
                    throw new FunnelWebExtensionLoadException(string.Format("Failed to load {0}", ex.Types[0].Assembly.FullName), ex.LoaderExceptions[0]);
                throw;
            }
            var assemblies = new HashSet<Assembly>();
            modules.Each(m => assemblies.Add(m.GetType().Assembly));
            assemblies.Each(BuildManager.AddReferencedAssembly);
        }

        void Application_Start()
        {
            IContainer container = null;
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.Register<HttpContextBase>(x => new HttpContextWrapper(HttpContext.Current))
                .InstancePerLifetimeScope();
            builder.Register<HttpServerUtilityBase>(x => new HttpServerUtilityWrapper(HttpContext.Current.Server));
            builder
                .RegisterType<DatabaseUpgradeDetector>()
                .As<IDatabaseUpgradeDetector>()
                .SingleInstance();
            builder.RegisterModule(new AuthenticationModule());
            builder.RegisterModule(new BindersModule(ModelBinders.Binders));
            builder.RegisterModule(new MimeSupportModule());
            builder.RegisterModule(new SettingsModule());
            builder.RegisterModule(new SpamModule());
            builder.RegisterModule(new EventingModule());
            builder.RegisterModule(new TasksModule());

            builder.RegisterModule(new ExtensionsModule(_extensionsPath, RouteTable.Routes));
            // ReSharper disable AccessToModifiedClosure
            builder.RegisterModule(new RepositoriesModule(()=>container.Resolve<IEnumerable<IScriptProvider>>()));
            // ReSharper restore AccessToModifiedClosure

            builder.RegisterModule(new RoutesModule(RouteTable.Routes));

            container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            AutofacHostFactory.Container = container;

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new FunnelWebViewEngine(() => container.Resolve<ISettingsProvider>(), container.Resolve<IDatabaseUpgradeDetector>()));

            ControllerBuilder.Current.SetControllerFactory(new FunnelWebControllerFactory(container));
        }
    }
}