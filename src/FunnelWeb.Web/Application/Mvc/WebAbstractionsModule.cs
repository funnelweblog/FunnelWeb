using System.Web;
using Autofac;

namespace FunnelWeb.Web.Application.Mvc
{
    public class WebAbstractionsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register<HttpContextBase>(x => new HttpContextWrapper(HttpContext.Current))
                .InstancePerLifetimeScope();

            builder.Register<HttpRequestBase>(x => new HttpContextWrapper(HttpContext.Current).Request)
                .InstancePerLifetimeScope();
        
            builder.Register<HttpServerUtilityBase>(x => new HttpServerUtilityWrapper(HttpContext.Current.Server))
                .InstancePerLifetimeScope();
        }
    }
}