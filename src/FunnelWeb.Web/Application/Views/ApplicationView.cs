using FunnelWeb.Web.Application.Settings;
using Autofac;

namespace FunnelWeb.Web.Application.Views
{
    public class ApplicationView<TModel> : ViewPage<TModel>, IInjectable, IApplicationView where TModel : class
    {
        private ISettingsProvider _settingsProvider;

        public ApplicationView()
        {
        }

        public ISettingsProvider Settings
        {
            get { return _settingsProvider; }
        }

        void IInjectable.Inject(ILifetimeScope container)
        {
            _settingsProvider = container.Resolve<ISettingsProvider>();
        }
    }
}
