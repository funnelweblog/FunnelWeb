
using Autofac;
namespace FunnelWeb.Web.Application.Views
{
    public interface IInjectable
    {
        void Inject(ILifetimeScope container);
    }
}