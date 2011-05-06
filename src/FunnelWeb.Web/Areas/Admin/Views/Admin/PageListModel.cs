using System.Collections.Generic;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Areas.Admin.Views.Admin
{
    public class PageListModel
    {
        public PageListModel(IEnumerable<EntrySummary> entries)
        {
            Entries = entries;
        }

        public IEnumerable<EntrySummary> Entries { get; set; }
        public bool SortAscending { get; set; }
    }
}