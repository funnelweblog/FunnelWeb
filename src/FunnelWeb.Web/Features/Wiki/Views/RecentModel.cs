using System.Collections.Generic;
using FunnelWeb.Web.Model;

namespace FunnelWeb.Web.Features.Wiki.Views
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