using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Web.Mvc;
using FunnelWeb.Eventing;
using FunnelWeb.Model.Repositories;
using FunnelWeb.Tasks;
using FunnelWeb.Web.Application.Authentication;
using FunnelWeb.Web.Application.Mime;
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
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.Register<HttpServerUtilityBase>(x => new HttpServerUtilityWrapper(HttpContext.Current.Server));
            builder.RegisterModule(new ExtensionsModule(Server.MapPath("~/bin/Extensions"), RouteTable.Routes));
            builder.RegisterModule(new RoutesModule(RouteTable.Routes));
            builder.RegisterModule(new AuthenticationModule());
            builder.RegisterModule(new BindersModule(ModelBinders.Binders));
            builder.RegisterModule(new MimeSupportModule());
            builder.RegisterModule(new RepositoriesModule());
            builder.RegisterModule(new ViewsModule(ViewEngines.Engines));
            builder.RegisterModule(new SpamModule());
            builder.RegisterModule(new EventingModule());
            builder.RegisterModule(new TasksModule());
            
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}