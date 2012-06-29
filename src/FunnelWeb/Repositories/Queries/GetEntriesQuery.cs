using System;
using System.Linq;
using FunnelWeb.Model;
using FunnelWeb.Providers.Database;
using FunnelWeb.Repositories.Projections;
using NHibernate;
using NHibernate.Criterion.Lambda;
using NHibernate.Transform;
using FunnelWeb.DatabaseDeployer.Infrastructure;

namespace FunnelWeb.Repositories.Queries
{
    public class GetEntriesQuery : IPagedQuery<EntrySummary>
    {
        private readonly string entryStatus;
        private readonly EntriesSortColumn sortColumn;
        private readonly bool asc;

        public GetEntriesQuery(string entryStatus = null, EntriesSortColumn sortColumn = EntriesSortColumn.Published, bool asc = false)
        {
            this.entryStatus = entryStatus;
            this.sortColumn = sortColumn;
            this.asc = asc;
        }

        public PagedResult<EntrySummary> Execute(ISession session, IDatabaseProvider databaseProvider, int skip, int take)
        {
            var totalQuery = session
                .QueryOver<Entry>();
            totalQuery.ApplyEntryStatusFilter(entryStatus);

            var total = totalQuery
               .ToRowCountQuery()
               .FutureValue<Entry, int>(databaseProvider);

            var entriesQuery = Query(session);
            entriesQuery.ApplyEntryStatusFilter(entryStatus);

            var entries = entriesQuery
                .Skip(skip)
                .Take(take)
                .Future<Entry, EntrySummary>(databaseProvider)
                .ToList();

            return new PagedResult<EntrySummary>(entries, total.Value, skip, take);
        }

        protected IQueryOver<Entry, Entry> Query(ISession session)
        {
            var entrySummaryAlias = new EntrySummary();

            var entries = session
                .QueryOver<Entry>()
                .SelectList(EntrySummaryProjections.FromEntry(entrySummaryAlias))
                .TransformUsing(Transformers.AliasToBean<EntrySummary>());

            IQueryOverOrderBuilder<Entry, Entry> orderBy = null;

            switch (sortColumn)
            {
                case EntriesSortColumn.Slug:
                    orderBy = entries.OrderBy(e => e.Name);
                    break;
                case EntriesSortColumn.Title:
                    orderBy = entries.OrderBy(e => e.Title);
                    break;
                case EntriesSortColumn.Comments:
                    orderBy = entries.OrderBy(e => e.CommentCount);
                    break;
                case EntriesSortColumn.Published:
                    orderBy = entries.OrderBy(e => e.Published);
                    break;
            }
            if (orderBy != null)
                entries = asc ? orderBy.Asc : orderBy.Desc;

            return entries;
        }
    }
}
