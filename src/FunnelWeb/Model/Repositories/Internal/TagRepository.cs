using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace FunnelWeb.Model.Repositories.Internal
{
    public class TagRepository : ITagRepository
    {
        private readonly ISession session;

        public TagRepository(ISession session)
        {
            this.session = session;
        }

        public IQueryable<Tag> GetTags()
        {
            return GetTags(string.Empty);
        }

        public IQueryable<Tag> GetTags(string tagName)
        {
            tagName = tagName ?? string.Empty;

            return from tag in session.Query<Tag>()
                   where tag.Name.Contains(tagName)
                   select tag;

        }

        public IEnumerable<Entry> GetTaggedItems(string tagName, int skip, int take)
        {
            var tags = session.QueryOver<Tag>()
                .Where(t => t.Name == tagName);

            var entries = tags
                .Left
                    .JoinQueryOver<Entry>(i => i.Entries);

            var results = entries
                .Where(e => e.Published < DateTime.UtcNow.Date.AddDays(1))
                .OrderBy(e => e.Published).Desc;

            return results.List<Tag>()
                .First()
                .Entries;
        }

        public int GetTaggedItemCount(string tagName)
        {
            return session.Query<Tag>().Where(i => i.Name == tagName).Count();
        }

        public void Save(Tag feed)
        {
            session.SaveOrUpdate(feed);
        }

        public void Delete(Tag feed)
        {
            session.Delete(feed);
        }

        public Tag GetTag(int id)
        {
            return session.Query<Tag>()
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        public Tag GetTag(string tagName)
        {
            return session.Query<Tag>()
                .Where(x => x.Name.Contains(tagName))
                .FirstOrDefault();
        }
    }
}