using System;
using System.Collections.Generic;
using FunnelWeb.Model;
using FunnelWeb.Providers.Database;
using NHibernate;

namespace FunnelWeb.Repositories.Queries
{
    /// <summary>
    /// Switches between FullText and Simple search based on provider
    /// </summary>
    public class SwitchingSearchEntriesQuery : IPagedQuery<EntryRevision>
    {
        private readonly string searchText;
        private readonly EntriesSortColumn sortColumn;
        private readonly bool asc;

        public SwitchingSearchEntriesQuery(string searchText, EntriesSortColumn sortColumn = EntriesSortColumn.Published, bool asc = false)
        {
            this.searchText = searchText;
            this.sortColumn = sortColumn;
            this.asc = asc;
        }

        public string SearchText
        {
            get { return searchText; }
        }

        public PagedResult<EntryRevision> Execute(ISession session, IDatabaseProvider databaseProvider, int skip, int take)
        {
            if (string.IsNullOrEmpty(SearchText) || SearchText.Trim().Length == 0)
                return new PagedResult<EntryRevision>(new List<EntryRevision>(), 0, skip, take);

            if (databaseProvider.SupportsFullText)
                return new FullTextSearchEntriesQuery(searchText, sortColumn, asc).Execute(session, databaseProvider, skip, take);
            return new SimpleSearchEntriesQuery(searchText, sortColumn, asc).Execute(session, databaseProvider, skip, take);
        }
    }
}