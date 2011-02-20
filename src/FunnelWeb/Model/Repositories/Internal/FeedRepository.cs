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
            return session.Query<Entry>().Where(e => e.Status == EntryStatus.PublicBlog).Count();
        }

        public IEnumerable<Entry> GetRecentEntries(int skip, int take)
        {
            var entryQuery = session
                .QueryOver<Entry>()
                .Where(e=>e.Status == EntryStatus.PublicBlog)
                .And(e=>e.Published < DateTime.UtcNow.Date.AddDays(1))
                .OrderBy(e=>e.Published).Desc
                .Skip(skip)
                .Take(take)
                .List();

            return entryQuery;
        }

        public IEnumerable<Comment> GetRecentComments(int skip, int take)
        {
            return session
                .QueryOver<Comment>()
                .Fetch(x=>x.Entry).Eager()
                .OrderBy(x=>x.Posted).Desc()
                .Where(x => x.Status != 0)
                .Take((skip * take) + take * 10)
                .Skip(skip)
                .Take(take)
                .List();
        }
    }
}
