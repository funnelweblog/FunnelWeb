using System;
using System.Linq;
using FunnelWeb.Model;
using FunnelWeb.Repositories.Projections;
using NHibernate;
using NHibernate.Criterion.Lambda;
using NHibernate.Transform;

namespace FunnelWeb.Repositories.Queries
{
    public class GetEntriesByTagQuery : IPagedQuery<EntrySummary>
    {
        private readonly string tag;
        private readonly EntriesSortColumn sortColumn;
        private readonly bool asc;

        public GetEntriesByTagQuery(string tag, EntriesSortColumn sortColumn = EntriesSortColumn.Published, bool asc = false)
        {
            this.tag = tag;
            this.sortColumn = sortColumn;
            this.asc = asc;
        }

        public PagedResult<EntrySummary> Execute(ISession session, int skip, int take)
        {
            var total = session
               .QueryOver<Entry>()
                .JoinQueryOver<Tag>(e => e.Tags).Where(t => t.Name == tag)
               .ToRowCountQuery()
               .FutureValue<int>();

            var entries = Query(session)
                .JoinQueryOver<Tag>(e=>e.Tags).Where(t=>t.Name == tag)
                .Skip(skip)
                .Take(take)
                .Future<EntrySummary>()
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
