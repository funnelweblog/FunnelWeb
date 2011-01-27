using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;

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

            return from tag in session.Linq<Tag>()
                   where tag.Name.Contains(tagName)
                   select tag;

        }

        public IEnumerable<Entry> GetTaggedItems(string tagName, int skip, int take)
        {
            var entryQuery = (ArrayList)session.CreateCriteria<TagItem>("ti")
                                            .CreateCriteria("ti.Tag", "tag")
                                            .CreateCriteria("ti.Entry", "entry")
                                            .CreateCriteria("entry.Revisions", "rev")
                                            .Add(Restrictions.EqProperty("rev.Id", Projections.SubQuery(
                                                DetachedCriteria.For<Revision>("rv")
                                                    .SetProjection(Projections.Property("rv.Id"))
                                                    .AddOrder(Order.Desc("rv.Revised"))
                                                    .Add(Restrictions.EqProperty("rv.Entry.Id", "entry.Id"))
                                                    .SetMaxResults(1))))
                                            .Add(Restrictions.Eq("tag.Name", tagName))
                                            .Add(Restrictions.Le("entry.Published", DateTime.UtcNow.Date.AddDays(1)))
                                            .AddOrder(Order.Desc("entry.Published"))
                                            .SetFirstResult(skip)
                                            .SetMaxResults(take)
                                            .SetResultTransformer(Transformers.AliasToEntityMap)
                                            .List();

            var results = new List<Entry>();
            foreach (var record in entryQuery.Cast<Hashtable>())
            {
                var entry = (Entry)record["entry"];
                var revision = (Revision)record["rev"];
                entry.LatestRevision = revision;
                results.Add(entry);
            }

            return results;
        }

        public int GetTaggedItemCount(string tagName)
        {
            return session.Linq<TagItem>().Where(i => i.Tag.Name == tagName).Count();
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
            return session.Linq<Tag>()
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        public Tag GetTag(string tagName)
        {
            return session.Linq<Tag>()
                .Where(x => x.Name.Contains(tagName))
                .FirstOrDefault();
        }
    }
}