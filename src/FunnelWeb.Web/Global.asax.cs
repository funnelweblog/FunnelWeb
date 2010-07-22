using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Web;
using Autofac.Integration.Web.Mvc;
using FunnelWeb.Web.Application.Authentication;
using FunnelWeb.Web.Application.Binders;
using FunnelWeb.Web.Application.Mime;
using FunnelWeb.Web.Application.Routes;
using FunnelWeb.Web.Application.Spam;
using FunnelWeb.Web.Application.Validation;
using FunnelWeb.Web.Application.Views;
using FunnelWeb.Web.Model.Repositories;

namespace FunnelWeb.Web
{
    public class MvcApplication : HttpApplication, IContainerProviderAccessor
    {
        private static IContainerProvider _containerProvider;

        IContainerProvider IContainerProviderAccessor.ContainerProvider
        {
            get { return _containerProvider; }
        }

        protected void Application_Start()
        {
            var connectionString = WebConfigurationManager.ConnectionStrings["funnelweb.configuration.database.connection"].ConnectionString;
            var automaticallyCreate = bool.Parse(WebConfigurationManager.AppSettings["funnelweb.configuration.database.autocreate"]);

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new RoutesModule(RouteTable.Routes));

            containerBuilder.RegisterControllers(Assembly.GetExecutingAssembly());

            containerBuilder.RegisterModule(new AuthenticationModule());
            containerBuilder.RegisterModule(new BindersModule(ModelBinders.Binders));
            containerBuilder.RegisterModule(new ValidationModule());
            containerBuilder.RegisterModule(new MimeSupportModule());
            containerBuilder.RegisterModule(new RepositoriesModule(automaticallyCreate, connectionString));
            containerBuilder.RegisterModule(new ViewsModule(ViewEngines.Engines));
            containerBuilder.RegisterModule(new SpamModule());

            _containerProvider = new ContainerProvider(containerBuilder.Build());

            ControllerBuilder.Current.SetControllerFactory(new AutofacControllerFactory(_containerProvider));
        }
    }
}