using FunnelWeb.Web.Model;
using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Features.Wikis.Views
{
    public class RevisionsModel
    {
        public RevisionsModel(PageName page, Entry entry)
        {
            Page = page;
            Entry = entry;
        }

        public PageName Page { get; set; }
        public Entry Entry { get; set; }
    }
}