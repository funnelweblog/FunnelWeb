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
    public class FeedRepository : IFeedRepository
    {
        private readonly ISession session;

        public FeedRepository(ISession session)
        {
            this.session = session;
        }

        public int GetEntryCount()
        {
            return session.Linq<Entry>().Where(e => e.Status == EntryStatus.PublicBlog).Count();
        }

        public IEnumerable<Entry> GetRecentEntries(int skip, int take)
        {
            var entryQuery = (ArrayList)session.CreateCriteria<Entry>("entry")
                .CreateCriteria("entry.Revisions", "rev")
                .Add(Restrictions.EqProperty("rev.Id", Projections.SubQuery(
                    DetachedCriteria.For<Revision>("rv")
                        .SetProjection(Projections.Property("rv.Id"))
                        .AddOrder(Order.Desc("rv.Revised"))
                        .Add(Restrictions.EqProperty("rv.Entry.Id", "entry.Id"))
                        .SetMaxResults(1))))
                .Add(Restrictions.Not(Restrictions.Eq("entry.Status", EntryStatus.PublicPage)))
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

        public IEnumerable<Comment> GetRecentComments(int skip, int take)
        {
            return session.Linq<Comment>().Expand("Entry")
                .OrderByDescending(x => x.Posted)
                .Take((skip * take) + take * 10)
                .ToList()
                .Where(x => !x.IsSpam)
                .Skip(skip).Take(take);
        }
    }
}
