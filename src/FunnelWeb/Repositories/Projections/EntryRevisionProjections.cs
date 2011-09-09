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

            return FromEntry(entryRevision);
        }

        public static Func<QueryOverProjectionBuilder<Entry>, QueryOverProjectionBuilder<Entry>> FromEntry(EntryRevision entryRevisionAlias)
        {
            return list =>
                       {
                           list
                               .Select(e => e.Id).WithAlias(() => entryRevisionAlias.Id)
                               .Select(e => e.Author).WithAlias(() => entryRevisionAlias.Author)
                               .Select(e => e.LatestRevision.Author).WithAlias(() => entryRevisionAlias.RevisionAuthor)
                               .Select(e => e.LatestRevision.Body).WithAlias(() => entryRevisionAlias.Body)
                               .Select(e => e.CommentCount).WithAlias(() => entryRevisionAlias.CommentCount)
                               .Select(e => e.LatestRevision.Format).WithAlias(() => entryRevisionAlias.Format)
                               .Select(e => e.HideChrome).WithAlias(() => entryRevisionAlias.HideChrome)
                               .Select(e => e.IsDiscussionEnabled).WithAlias(() => entryRevisionAlias.IsDiscussionEnabled)
                               .Select(e => e.LatestRevision.RevisionNumber).WithAlias(() => entryRevisionAlias.LatestRevisionNumber)
                               .Select(e => e.LatestRevision.RevisionNumber).WithAlias(() => entryRevisionAlias.RevisionNumber)
                               .Select(e => e.MetaDescription).WithAlias(() => entryRevisionAlias.MetaDescription)
                               .Select(e => e.MetaTitle).WithAlias(() => entryRevisionAlias.MetaTitle)
                               .Select(e => e.Title).WithAlias(() => entryRevisionAlias.Title)
                               .Select(e => e.Name).WithAlias(() => entryRevisionAlias.Name)
                               .Select(e => e.PageTemplate).WithAlias(() => entryRevisionAlias.PageTemplate)
                               .Select(e => e.Published).WithAlias(() => entryRevisionAlias.Published)
                               .Select(e => e.LatestRevision.Revised).WithAlias(() => entryRevisionAlias.Revised)
                               .Select(e => e.Status).WithAlias(() => entryRevisionAlias.Status)
                               .Select(e => e.Summary).WithAlias(() => entryRevisionAlias.Summary)
                               .Select(e => e.TagsCommaSeparated).WithAlias(()=> entryRevisionAlias.TagsCommaSeparated);

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
