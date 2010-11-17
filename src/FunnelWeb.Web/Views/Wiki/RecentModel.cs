using System.Collections.Generic;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Views.Wiki
{
    public class RecentModel
    {
        public RecentModel(IEnumerable<Entry> revisions, int pageNumber, int totalPages)
        {
            Entries = revisions;
            PageNumber = pageNumber;
            TotalPages = totalPages;
        }

        public IEnumerable<Entry> Entries { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}