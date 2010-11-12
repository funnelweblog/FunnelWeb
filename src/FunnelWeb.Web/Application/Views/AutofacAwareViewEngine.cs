using System.Web;
using Autofac.Integration.Web;

namespace FunnelWeb.Web.Application.Views
{
    public class AutofacAwareViewEngine : WebFormViewEngine
    {
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new AutofacWebFormView(
                ((IContainerProviderAccessor)HttpContext.Current.ApplicationInstance).ContainerProvider.RequestLifetime, 
                viewPath, 
                masterPath);
        }
    }
}