using System;
using FunnelWeb.Model;
using NHibernate.Criterion.Lambda;

namespace FunnelWeb.Repositories.Projections
{
    public static class EntrySummaryProjections
    {
        public static Func<QueryOverProjectionBuilder<Entry>, QueryOverProjectionBuilder<Entry>> FromEntry(EntrySummary entrySummaryAlias)
        {
            return list =>
                       {
                           list
                               .Select(e => e.TagsCommaSeparated).WithAlias(() => entrySummaryAlias.TagsCommaSeparated)
                               .Select(e => e.CommentCount).WithAlias(() => entrySummaryAlias.CommentCount)
                               .Select(e => e.MetaDescription).WithAlias(() => entrySummaryAlias.MetaDescription)
                               .Select(e => e.Name).WithAlias(() => entrySummaryAlias.Name)
                               .Select(e => e.Published).WithAlias(() => entrySummaryAlias.Published)
                               .Select(e => e.Summary).WithAlias(() => entrySummaryAlias.Summary)
                               .Select(e => e.Title).WithAlias(() => entrySummaryAlias.Title)
                               .Select(e=>e.Status).WithAlias(()=>entrySummaryAlias.Status)
                               .Select(e => e.LatestRevision.Revised).WithAlias(() => entrySummaryAlias.LastRevised);

                           return list;
                       };
        }
    }
}