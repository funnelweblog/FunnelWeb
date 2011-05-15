using System;
using System.Linq;
using FunnelWeb.Model;
using NHibernate;
using NHibernate.Criterion.Lambda;
using NHibernate.Transform;

namespace FunnelWeb.Repositories.Queries
{
    public class GetEntriesQuery : IPagedQuery<EntrySummary>
    {
        private readonly string entryStatus;
        private readonly EntriesSortColumn sortColumn;
        private readonly bool asc;

        public GetEntriesQuery(string entryStatus, EntriesSortColumn sortColumn = EntriesSortColumn.Published, bool asc = false)
        {
            this.entryStatus = entryStatus;
            this.sortColumn = sortColumn;
            this.asc = asc;
        }

        public PagedResult<EntrySummary> Execute(ISession session, int skip, int take)
        {
            var total = session
               .QueryOver<Entry>()
               .Where(e => e.Status == entryStatus)
               .ToRowCountQuery()
               .FutureValue<int>();

            var entries = Query(session)
               .Where(e => e.Status == entryStatus)
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
                .SelectList(l =>
                            l
                                .Select(e => e.TagsCommaSeparated).WithAlias(() => entrySummaryAlias.TagsCommaSeparated)
                                .Select(e => e.CommentCount).WithAlias(() => entrySummaryAlias.CommentCount)
                                .Select(e => e.MetaDescription).WithAlias(() => entrySummaryAlias.MetaDescription)
                                .Select(e => e.Name).WithAlias(() => entrySummaryAlias.Name)
                                .Select(e => e.Published).WithAlias(() => entrySummaryAlias.Published)
                                .Select(e => e.Summary).WithAlias(() => entrySummaryAlias.Summary)
                                .Select(e => e.Title).WithAlias(() => entrySummaryAlias.Title)
                                .Select(e => e.LatestRevision.Revised).WithAlias(() => entrySummaryAlias.LastRevised))
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
