using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Web;
using Autofac.Integration.Web.Mvc;
using FunnelWeb.Web.Application.Authentication;
using FunnelWeb.Web.Application.Mime;
using FunnelWeb.Web.Application.Mvc.Binders;
using FunnelWeb.Web.Application.Spam;
using FunnelWeb.Web.Application.Views;
using FunnelWeb.Web.Model.Repositories;

namespace FunnelWeb.Web
{
    /// <summary>
    /// Entry point for the FunnelWeb application.
    /// </summary>
    public class MvcApplication : HttpApplication, IContainerProviderAccessor
    {
        private static IContainerProvider containerProvider;

        IContainerProvider IContainerProviderAccessor.ContainerProvider
        {
            get { return containerProvider; }
        }

        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            builder.Register<HttpServerUtilityBase>(x => new HttpServerUtilityWrapper(HttpContext.Current.Server));

            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterModule(new RoutesModule(RouteTable.Routes));
            builder.RegisterModule(new AuthenticationModule());
            builder.RegisterModule(new BindersModule(ModelBinders.Binders));
            builder.RegisterModule(new MimeSupportModule());
            builder.RegisterModule(new RepositoriesModule());
            builder.RegisterModule(new ViewsModule(ViewEngines.Engines));
            builder.RegisterModule(new SpamModule());

            containerProvider = new ContainerProvider(builder.Build());

            ControllerBuilder.Current.SetControllerFactory(new AutofacControllerFactory(containerProvider));
        }
    }
}