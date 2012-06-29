using System;
using System.Linq;
using FunnelWeb.Model;
using FunnelWeb.DatabaseDeployer.Infrastructure;
using FunnelWeb.Providers.Database;
using FunnelWeb.Repositories.Projections;
using NHibernate;
using NHibernate.Criterion.Lambda;
using NHibernate.Transform;

namespace FunnelWeb.Repositories.Queries
{
    public class GetEntriesByTagQuery : IPagedQuery<EntrySummary>
    {
        private readonly string tag;
        private readonly string entryStatus;
        private readonly EntriesSortColumn sortColumn;
        private readonly bool asc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="entryStatus">When null, will exclude private entries</param>
        /// <param name="sortColumn"></param>
        /// <param name="asc"></param>
        public GetEntriesByTagQuery(
            string tag, 
            string entryStatus = null, 
            EntriesSortColumn sortColumn = EntriesSortColumn.Published, 
            bool asc = false)
        {
            this.tag = tag;
            this.entryStatus = entryStatus;
            this.sortColumn = sortColumn;
            this.asc = asc;
        }

        public PagedResult<EntrySummary> Execute(ISession session, IDatabaseProvider databaseProvider, int skip, int take)
        {
            var totalEntries = session
                .QueryOver<Entry>()
                .ApplyEntryStatusFilter(entryStatus);

            var total = totalEntries
                .JoinQueryOver<Tag>(e => e.Tags).Where(t => t.Name == tag)
               .ToRowCountQuery()
               .FutureValue<Entry, int>(databaseProvider);

            var entriesQuery = Query(session)
                .ApplyEntryStatusFilter(entryStatus);

            var entries = entriesQuery
                .JoinQueryOver<Tag>(e=>e.Tags).Where(t=>t.Name == tag)
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
