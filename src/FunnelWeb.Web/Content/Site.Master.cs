using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using FunnelWeb.Web.Application.Settings;
using FunnelWeb.Web.Application.Views;
using Autofac;

namespace FunnelWeb.Web.Views.Shared
{
    public class SiteMaster : System.Web.Mvc.ViewMasterPage, IInjectable
    {
        protected Control _summary;
        protected ContentPlaceHolder SummaryContent;
        private ISettingsProvider _settingsProvider;

        protected override void Render(HtmlTextWriter writer)
        {
            var sw = new StringWriter();
            SummaryContent.RenderControl(new HtmlTextWriter(sw));
            if (sw.ToString().Trim().Length == 0)
                _summary.Visible = false;
            
            base.Render(writer);
        }

        public ISettingsProvider Settings
        {
            get
            {
                return ((IApplicationView) Page).Settings;
            }
        }

        void IInjectable.Inject(ILifetimeScope container)
        {
            _settingsProvider = container.Resolve<ISettingsProvider>();
        }
    }
}
