using System.Collections.Generic;
using FunnelWeb.Web.Model;
using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Features.Wikis.Views
{
    public class EditModel
    {
        public EditModel(PageName page, Entry entry, bool isNew, IEnumerable<Feed> feeds)
        {
            Page = page;
            Entry = entry;
            IsNew = isNew;
            Feeds = feeds;
        }

        public PageName Page { get; set; }
        public Entry Entry { get; set; }
        public IEnumerable<Feed> Feeds { get; set; }
        public bool IsNew { get; set; }
    }
}