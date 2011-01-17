using System.Collections.Generic;
using FunnelWeb.Model;

namespace FunnelWeb.Web.Views.Wiki
{
    public class SearchModel
    {
        public SearchModel(string searchText, bool is404, IEnumerable<Entry> results)
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