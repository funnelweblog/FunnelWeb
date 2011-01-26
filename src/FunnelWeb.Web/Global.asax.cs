using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Autofac;
using Autofac.Integration.Mvc;
using FunnelWeb.Eventing;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Tasks;
using FunnelWeb.Web.Application.Authentication;
using FunnelWeb.Web.Application.Mime;
using FunnelWeb.Web.Application.Mvc.Binders;
using FunnelWeb.Web.Application.Spam;
using FunnelWeb.Web.Application.Views;
using FunnelWeb.Settings;
using FunnelWeb.Web.Application.Mvc;

namespace FunnelWeb.Web
{
    /// <summary>
    /// Entry point for the FunnelWeb application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        void Application_Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.Register<HttpContextBase>(x => new HttpContextWrapper(HttpContext.Current))
                .InstancePerLifetimeScope();
            builder.Register<HttpServerUtilityBase>(x => new HttpServerUtilityWrapper(HttpContext.Current.Server));
            builder.RegisterModule(new AuthenticationModule());
            builder.RegisterModule(new BindersModule(ModelBinders.Binders));
            builder.RegisterModule(new MimeSupportModule());
            builder.RegisterModule(new RepositoriesModule());
            builder.RegisterModule(new SettingsModule());
            builder.RegisterModule(new SpamModule());
            builder.RegisterModule(new EventingModule());
            builder.RegisterModule(new TasksModule());

            builder.RegisterModule(new ExtensionsModule(Server.MapPath("~/bin/Extensions"), RouteTable.Routes));
            builder.RegisterModule(new RoutesModule(RouteTable.Routes));

            var container = builder.Build();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new FunnelWebViewEngine(container.Resolve<ISettingsProvider>()));

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory(container));
        }

        public class ControllerFactory : DefaultControllerFactory
        {
            private readonly IContainer _container;

            public ControllerFactory(IContainer container)
            {
                _container = container;
            }
            protected override Type GetControllerType(RequestContext requestContext, string controllerName)
            {
                var controller = base.GetControllerType(requestContext, controllerName);
                if (controller == null)
                {
                    object x;
                    if (_container.TryResolveNamed(controllerName, typeof(IController), out x))
                        controller = x.GetType();
                }

                return controller;
            }
        }
    }
}