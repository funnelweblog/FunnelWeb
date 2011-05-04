using System;
using FunnelWeb.Model;
using FunnelWeb.Repositories.Projections;
using NHibernate;
using NHibernate.Criterion.Lambda;
using NHibernate.Transform;

namespace FunnelWeb.Repositories.Queries
{
    public class GetEntriesQuery : IPagedQuery<EntryRevision>
    {
        private readonly EntriesSortColumn sortColumn;
        private readonly bool asc;

        public GetEntriesQuery(EntriesSortColumn sortColumn = EntriesSortColumn.Published, bool asc = false)
        {
            this.sortColumn = sortColumn;
            this.asc = asc;
        }

        public PagedResult<EntryRevision> Execute(ISession session, int skip, int take)
        {
            var total = session
               .QueryOver<Entry>()
               .ToRowCountQuery()
               .FutureValue<int>();

            var entries = Query(session)
                .Skip(skip)
                .Take(take)
                .List<EntryRevision>();

            return new PagedResult<EntryRevision>(entries, total.Value, skip, take);
        }

        protected IQueryOver<Entry, Entry> Query(ISession session)
        {
            var entries = session
                .QueryOver<Entry>()
                .SelectList(EntryRevisionProjections.FromEntry())
                .TransformUsing(Transformers.AliasToBean<EntryRevision>());

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
