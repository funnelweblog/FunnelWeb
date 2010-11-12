using System;
using System.Web.Compilation;
using Autofac;
using Bindable.Core.Helpers;

namespace FunnelWeb.Web.Application.Views
{
    public class AutofacWebFormView : IView
    {
        private readonly ILifetimeScope _container;
        private readonly string _viewPath;
        private readonly string _masterPath;

        public AutofacWebFormView(ILifetimeScope container, string viewPath, string masterPath)
        {
            _container = container;
            _viewPath = viewPath;
            _masterPath = masterPath ?? string.Empty;
        }

        public void Render(ViewContext viewContext, System.IO.TextWriter writer)
        {
            Guard.NotNull(viewContext, "viewContext");
            
            var view = BuildManager.CreateInstanceFromVirtualPath(_viewPath, typeof(object));
            if (view == null)
            {
                throw new InvalidOperationException();
            }

            if (view is IInjectable)
            {
                ((IInjectable)view).Inject(_container);
            }
            
            var page = view as ViewPage;
            if (page != null)
            {
                RenderViewPage(viewContext, page);
            }
            else
            {
                var control = view as ViewUserControl;
                if (control == null)
                {
                    throw new InvalidOperationException();
                }
                RenderViewUserControl(viewContext, control);
            }
        }

        private void RenderViewPage(ViewContext context, ViewPage page)
        {
            if (!string.IsNullOrEmpty(_masterPath))
            {
                page.MasterLocation = _masterPath;
            }
            page.ViewData = context.ViewData;
            page.RenderView(context);
        }

        private void RenderViewUserControl(ViewContext context, ViewUserControl control)
        {
            if (!string.IsNullOrEmpty(_masterPath))
            {
                throw new InvalidOperationException();
            }
            control.ViewData = context.ViewData;
            control.RenderView(context);
        }
    }
}