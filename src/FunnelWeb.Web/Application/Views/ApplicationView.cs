using System.Web.Mvc;
using FunnelWeb.Web.Application.Settings;
using Autofac;

namespace FunnelWeb.Web.Application.Views
{
    public class ApplicationView<TModel> : ViewPage<TModel>, IInjectable, IApplicationView where TModel : class
    {
        private ISettingsProvider _settingsProvider;
        private IMarkdownProvider _markdownProvider;

        public ApplicationView()
        {
        }

        public ISettingsProvider Settings
        {
            get { return _settingsProvider; }
        }

        public IMarkdownProvider Markdown
        {
            get { return _markdownProvider; }
        }

        void IInjectable.Inject(ILifetimeScope container)
        {
            _settingsProvider = container.Resolve<ISettingsProvider>();
            _markdownProvider = container.Resolve<IMarkdownProvider>();
        }
    }
}
