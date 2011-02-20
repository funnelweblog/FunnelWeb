using System.Collections.Generic;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Areas.Admin.Views.Admin
{
    public enum PageListSortColumn
	{
		Slug,
		Title,
		Comments,
		Published
	}

    public class PageListModel
    {
        public PageListModel(IEnumerable<Entry> entries)
        {
            Entries = entries;
        }

        public IEnumerable<Entry> Entries { get; set; }
        public bool SortAscending { get; set; }
    }
}