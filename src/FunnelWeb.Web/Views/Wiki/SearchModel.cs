using System.Collections.Generic;
using FunnelWeb.Model;
using FunnelWeb.Repositories;

namespace FunnelWeb.Web.Views.Wiki
{
    public class SearchModel
    {
        public SearchModel(string searchText, bool is404, PagedResult<EntryRevision> results)
        {
            SearchText = searchText;
            Is404 = is404;
            Results = results;
        }

        public string SearchText { get; set; }
        public bool Is404 { get; set; }
        public PagedResult<EntryRevision> Results { get; set; }
    }
}