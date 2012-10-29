using System;
using System.Collections.Generic;
using System.Linq;
using FunnelWeb.Model;
using FunnelWeb.Providers.Database;
using FunnelWeb.Repositories.Projections;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.Transform;

namespace FunnelWeb.Repositories.Queries
{
    public class SimpleSearchEntriesQuery : IPagedQuery<EntryRevision>
    {
        private readonly string searchText;
        private readonly EntriesSortColumn sortColumn;
        private readonly bool asc;

        public SimpleSearchEntriesQuery(string searchText, EntriesSortColumn sortColumn = EntriesSortColumn.Published, bool asc = false)
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

            var searchTerms = new string(SearchText.Where(x => char.IsLetterOrDigit(x) || x == ' ' || x == '-').ToArray());
            searchTerms = searchTerms.Replace(" ", "%");

            var query = session.QueryOver<Entry>()
                .Where
                (
                    Restrictions.On<Entry>(e => e.LatestRevision.Body).IsLike(searchTerms, MatchMode.Anywhere)
                    ||
                    Restrictions.On<Entry>(e => e.Title).IsLike(searchTerms, MatchMode.Anywhere)
                )
                .And(e => e.Status != EntryStatus.Private);

            var total =
                query
                    .ToRowCountQuery()
                    .FutureValue<int>();

            query
                .SelectList(EntryRevisionProjections.FromEntry())
                .TransformUsing(Transformers.AliasToBean<EntryRevision>());

            IQueryOverOrderBuilder<Entry, Entry> orderBy = null;

            switch (sortColumn)
            {
                case EntriesSortColumn.Slug:
                    orderBy = query.OrderBy(e => e.Name);
                    break;
                case EntriesSortColumn.Title:
                    orderBy = query.OrderBy(e => e.Title);
                    break;
                case EntriesSortColumn.Comments:
                    orderBy = query.OrderBy(e => e.CommentCount);
                    break;
                case EntriesSortColumn.Published:
                    orderBy = query.OrderBy(e => e.Published);
                    break;
            }

            if (orderBy != null)
            {
                if (asc)
                    orderBy.Asc();
                else
                    orderBy.Desc();
            }

            var entries = query
                .Skip(skip)
                .Take(take)
                .List<EntryRevision>();

            return new PagedResult<EntryRevision>(entries, total.Value, skip, take);
        }
    }
}
