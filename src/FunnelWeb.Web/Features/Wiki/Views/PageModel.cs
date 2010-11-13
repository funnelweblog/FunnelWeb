using FunnelWeb.Web.Model;
using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Features.Wiki.Views
{
    public class PageModel
    {
        public PageModel(PageName page, Entry entry, bool isPriorVersion)
        {
            Page = page;
            Entry = entry;
            IsPriorVersion = isPriorVersion;
        }

        public bool IsPriorVersion { get; set; }
        public PageName Page { get; set; }
        public Entry Entry { get; set; }
    }
}