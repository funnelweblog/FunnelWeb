using System.Collections.Generic;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Views.Admin
{
    public class PageListModel
    {
        public PageListModel(IEnumerable<Entry> entries)
        {
            Entries = entries;
        }

        public IEnumerable<Entry> Entries { get; set; }
    }
}