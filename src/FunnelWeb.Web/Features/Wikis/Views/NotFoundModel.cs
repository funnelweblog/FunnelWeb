using System.Collections.Generic;
using FunnelWeb.Web.Model;

namespace FunnelWeb.Web.Features.Wikis.Views
{
    public class NotFoundModel
    {
        public NotFoundModel(string searchText, bool is404, IEnumerable<Entry> results)
        {
            SearchText = searchText;
            Is404 = is404;
            Results = results;
        }

        public string SearchText { get; set; }
        public bool Is404 { get; set; }
        public IEnumerable<Entry> Results { get; set; }
    }
}