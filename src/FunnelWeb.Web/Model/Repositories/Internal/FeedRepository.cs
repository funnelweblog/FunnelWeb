using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using FunnelWeb.Web.Application.Validation;
using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Model.Repositories.Internal
{
    public class FeedRepository : IFeedRepository
    {
        private readonly ISession _session;
        private readonly IEntityValidator _validator;

        public FeedRepository(ISession session, IEntityValidator validator)
        {
            _session = session;
            _validator = validator;
        }

        public IQueryable<Feed> GetFeeds()
        {
            return _session.Linq<Feed>();
        }

        public IEnumerable<Entry> GetFeed(PageName feed, int skip, int take)
        {
            var entryQuery = (ArrayList)_session.CreateCriteria<FeedItem>("it")
                .CreateCriteria("it.Feed", "feed")
                .CreateCriteria("it.Entry", "entry")
                .CreateCriteria("entry.Revisions", "rev")
                .Add(Restrictions.EqProperty("rev.Id", Projections.SubQuery(
                    DetachedCriteria.For<Revision>("rv")
                        .SetProjection(Projections.Property("rv.Id"))
                        .AddOrder(Order.Desc("rv.Revised"))
                        .Add(Restrictions.EqProperty("rv.Entry.Id", "entry.Id"))
                        .SetMaxResults(1))))
                .Add(Restrictions.Eq("feed.Name", feed))
                .Add(Restrictions.Le("entry.Published", DateTime.UtcNow.Date.AddDays(1)))
                .AddOrder(Order.Desc("it.SortDate"))
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

        public IEnumerable<Comment> GetCommentFeed(int skip, int take)
        {
            return _session.Linq<Comment>().Expand("Entry")
                .OrderByDescending(x => x.Posted)
                .Take((skip * take) + take*10)
                .ToList()
                .Where(x => !x.IsSpam)
                .Skip(skip).Take(take);
        }

        public int GetFeedCount(PageName feed)
        {
            return _session.Linq<FeedItem>().Where(i => i.Feed.Name == feed.ToString()).Count(); 
        }

        public ValidationResult Save(Feed feed)
        {
            var results = _validator.Validate(feed);
            if (results.IsValid)
            {
                _session.SaveOrUpdate(feed);
            }
            return results;
        }

        public void Delete(Feed feed)
        {
            _session.Delete(feed);            
        }
    }
}
