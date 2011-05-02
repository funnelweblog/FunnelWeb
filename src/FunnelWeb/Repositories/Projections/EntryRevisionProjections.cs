using System;
using FunnelWeb.Model;
using NHibernate.Criterion.Lambda;

namespace FunnelWeb.Repositories.Projections
{
    public static class EntryRevisionProjections
    {
        public static Func<QueryOverProjectionBuilder<Entry>, QueryOverProjectionBuilder<Entry>> FromEntry()
        {
            var entryRevision = new EntryRevision();

            return list =>
                       {
                           list
                               .Select(e => e.Id).WithAlias(() => entryRevision.Id)
                               .Select(e => e.Author).WithAlias(() => entryRevision.Author)
                               .Select(e => e.LatestRevision.Author).WithAlias(()=>entryRevision.RevisionAuthor)
                               .Select(e => e.LatestRevision.Body).WithAlias(() => entryRevision.Body)
                               .Select(e => e.CommentCount).WithAlias(() => entryRevision.CommentCount)
                               .Select(e => e.LatestRevision.Format).WithAlias(() => entryRevision.Format)
                               .Select(e => e.HideChrome).WithAlias(() => entryRevision.HideChrome)
                               .Select(e => e.IsDiscussionEnabled).WithAlias(() => entryRevision.IsDiscussionEnabled)
                               .Select(e => e.LatestRevision.RevisionNumber).WithAlias(() => entryRevision.LatestRevisionNumber)
                               .Select(e => e.LatestRevision.RevisionNumber).WithAlias(() => entryRevision.RevisionNumber)
                               .Select(e => e.MetaDescription).WithAlias(() => entryRevision.MetaDescription)
                               .Select(e => e.MetaTitle).WithAlias(() => entryRevision.MetaTitle)
                               .Select(e => e.Title).WithAlias(() => entryRevision.Title)
                               .Select(e => e.Name).WithAlias(() => entryRevision.Name)
                               .Select(e => e.PageTemplate).WithAlias(() => entryRevision.PageTemplate)
                               .Select(e => e.Published).WithAlias(() => entryRevision.Published)
                               .Select(e => e.LatestRevision.Revised).WithAlias(() => entryRevision.Revised)
                               .Select(e => e.Status).WithAlias(() => entryRevision.Status)
                               .Select(e => e.Summary).WithAlias(() => entryRevision.Summary);

                           return list;
                       };
        }

        public static Func<QueryOverProjectionBuilder<Revision>, QueryOverProjectionBuilder<Revision>> FromRevision(Entry entryAlias)
        {
            var entryRevision = new EntryRevision();

            return list =>
                       {
                           list
                               .Select(e => e.Id).WithAlias(() => entryRevision.Id)
                               .Select(() => entryAlias.Author).WithAlias(() => entryRevision.Author)
                               .Select(e => e.Author).WithAlias(() => entryRevision.RevisionAuthor)
                               .Select(e => e.Body).WithAlias(() => entryRevision.Body)
                               .Select(() => entryAlias.CommentCount).WithAlias(() => entryRevision.CommentCount)
                               .Select(e => e.Format).WithAlias(() => entryRevision.Format)
                               .Select(() => entryAlias.HideChrome).WithAlias(() => entryRevision.HideChrome)
                               .Select(() => entryAlias.IsDiscussionEnabled).WithAlias(() => entryRevision.IsDiscussionEnabled)
                               .Select(() => entryAlias.LatestRevision.RevisionNumber).WithAlias(() => entryRevision.LatestRevisionNumber)
                               .Select(e => e.RevisionNumber).WithAlias(() => entryRevision.RevisionNumber)
                               .Select(() => entryAlias.MetaDescription).WithAlias(() => entryRevision.MetaDescription)
                               .Select(() => entryAlias.MetaTitle).WithAlias(() => entryRevision.MetaTitle)
                               .Select(() => entryAlias.Name).WithAlias(() => entryRevision.Name)
                               .Select(() => entryAlias.PageTemplate).WithAlias(() => entryRevision.PageTemplate)
                               .Select(() => entryAlias.Published).WithAlias(() => entryRevision.Published)
                               .Select(() => entryAlias.LatestRevision.Revised).WithAlias(() => entryRevision.Revised)
                               .Select(() => entryAlias.Status).WithAlias(() => entryRevision.Status)
                               .Select(() => entryAlias.Summary).WithAlias(() => entryRevision.Summary)
                               .Select(() => entryAlias.Title).WithAlias(() => entryRevision.Title);

                           return list;
                       };

            
        }
    }
}
