using System;
using System.Collections.Generic;
using System.Linq;
using FunnelWeb.DatabaseDeployer.DbProviders;
using FunnelWeb.Model;
using FunnelWeb.Repositories.Projections;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.Transform;

namespace FunnelWeb.Repositories.Queries
{
    public class SearchEntriesQuery : IPagedQuery<EntryRevision>
    {
        private readonly string searchText;
        private readonly EntriesSortColumn sortColumn;
        private readonly bool asc;

        public SearchEntriesQuery(string searchText, EntriesSortColumn sortColumn = EntriesSortColumn.Published, bool asc = false)
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
            {
                return new PagedResult<EntryRevision>(new List<EntryRevision>(), 0, skip, take);
            }

            var searchQuery = Search(session);

            var total = 
                searchQuery
               .ToRowCountQuery()
               .FutureValue<int>();

            searchQuery
                .SelectList(EntryRevisionProjections.FromEntry())
                .TransformUsing(Transformers.AliasToBean<EntryRevision>());

            IQueryOverOrderBuilder<Entry, Entry> orderBy = null;

            switch (sortColumn)
            {
                case EntriesSortColumn.Slug:
                    orderBy = searchQuery.OrderBy(e => e.Name);
                    break;
                case EntriesSortColumn.Title:
                    orderBy = searchQuery.OrderBy(e => e.Title);
                    break;
                case EntriesSortColumn.Comments:
                    orderBy = searchQuery.OrderBy(e => e.CommentCount);
                    break;
                case EntriesSortColumn.Published:
                    orderBy = searchQuery.OrderBy(e => e.Published);
                    break;
            }

            if (orderBy != null)
            {
                if (asc) 
                    orderBy.Asc();
                else 
                    orderBy.Desc();
            }

            var entries = searchQuery
                .Skip(skip)
                .Take(take)
                .List<EntryRevision>();

            return new PagedResult<EntryRevision>(entries, total.Value, skip, take);
        }

        private IQueryOver<Entry, Entry> Search(ISession session)
        {
            var isFullTextEnabled = session.CreateSQLQuery(
                "SELECT FullTextServiceProperty('IsFullTextInstalled') + OBJECTPROPERTY(OBJECT_ID('Entry'), 'TableFullTextChangeTrackingOn')")
                .List()[0];

            return (int)isFullTextEnabled == 2
                ? SearchUsingFullText(session)
                : SearchUsingLike(session);
        }

        private IQueryOver<Entry, Entry> SearchUsingFullText(ISession session)
        {
            var searchTerms = SearchText.Split(' ', '-', '_').Where(x => !string.IsNullOrEmpty(x)).Select(x => "\"" + x + "*\"");
            var searchQuery = string.Join(" OR ", searchTerms.ToArray());

            var query = session.QueryOver<Entry>()
                .Where(Expression.Sql("CONTAINS(*, ?)", searchQuery, NHibernateUtil.String))
                .And(e => e.Status != EntryStatus.Private);

            return query;
        }

        public IQueryOver<Entry, Entry> SearchUsingLike(ISession session)
        {
            var searchTerms = new string(SearchText.Where(x => char.IsLetterOrDigit(x) || x == ' ' || x == '-').ToArray());
            searchTerms = searchTerms.Replace(" ", "%");

            var query = session.QueryOver<Entry>()
                .Where
                (
                    Restrictions.On<Entry>(e => e.LatestRevision.Body).IsLike(searchTerms, MatchMode.Anywhere)
                    ||
                    Restrictions.On<Entry>(e => e.Title).IsLike(searchTerms, MatchMode.Anywhere)
                );

            return query;
        }
    }
}
