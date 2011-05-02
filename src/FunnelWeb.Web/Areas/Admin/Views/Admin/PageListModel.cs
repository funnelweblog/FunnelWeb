using System.Collections.Generic;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Areas.Admin.Views.Admin
{
    public class PageListModel
    {
        public PageListModel(IEnumerable<EntryRevision> entries)
        {
            Entries = entries;
        }

        public IEnumerable<EntryRevision> Entries { get; set; }
        public bool SortAscending { get; set; }
    }
}