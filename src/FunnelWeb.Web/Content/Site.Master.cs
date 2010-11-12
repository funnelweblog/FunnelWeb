using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FunnelWeb.Web.Content
{
    public class SiteMaster : System.Web.Mvc.ViewMasterPage
    {
        protected Control _summary;
        protected ContentPlaceHolder SummaryContent;

        protected override void Render(HtmlTextWriter writer)
        {
            var sw = new StringWriter();
            SummaryContent.RenderControl(new HtmlTextWriter(sw));
            if (sw.ToString().Trim().Length == 0)
                _summary.Visible = false;
            
            base.Render(writer);
        }
    }
}
