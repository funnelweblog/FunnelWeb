using FunnelWeb.Model;
using FunnelWeb.Model.Strings;

namespace FunnelWeb.Web.Areas.Admin.Views.WikiAdmin
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