using System;
using System.Collections.Generic;
using System.Linq;
using FunnelWeb.Model.Strings;
using FunnelWeb.Repositories.Queries;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace FunnelWeb.Model.Repositories.Internal
{
    public class EntryRepository : IEntryRepository
    {
        private readonly ISession session;

        public EntryRepository(ISession session)
        {
            this.session = session;
        }

        public System.Tuple<IEnumerable<EntryRevision>, int> GetEntries(int skip, int? take)
        {
            var total = default(int);
            var entries = take == null ? 
                new GetEntriesQuery().Execute(session) : 
                new GetEntriesQuery().Execute(session, skip, take.Value, out total);

            return Tuple.Create(entries.Select(e =>
                                                   {
                                                       //Entry property is lazy loaded, as majority of the time we don't need to access it
                                                       e.Entry = session.QueryOver<Entry>().Where(entry => entry.Id == e.Id).FutureValue();
                                                       return e;
                                                   }), total);
        }

        public Entry GetEntry(int id)
        {
            return session
                .QueryOver<Entry>()
                .Where(x => x.Id == id)
                .SingleOrDefault();
        }

        public EntryRevision GetEntry(PageName name)
        {
            return new EntryByNameQuery(name).Execute(session).FirstOrDefault();
        }

        public EntryRevision GetEntry(PageName name, int revisionNumber)
        {
            return new EntryByNameAndRevisionQuery(name, revisionNumber).Execute(session).FirstOrDefault();
        }

        public void Delete(int id)
		{
			var entry = GetEntry(id);
			if (entry != null)
				session.Delete(entry);
		}

        public void Save(Entry entry)
        {
            session.SaveOrUpdate(entry);
            if (entry.LatestRevision.RevisionNumber == 0)
            {
                entry.LatestRevision.RevisionNumber = session.Query<Revision>().Where(x => x.Entry.Id == entry.Id).Count();
            }
        }

        public IEnumerable<EntryRevision> Search(string searchText)
        {
            if (string.IsNullOrEmpty(searchText) || searchText.Trim().Length == 0)
            {
                return new EntryRevision[0];
            }

            var isFullTextEnabled = session.CreateSQLQuery(
                "SELECT FullTextServiceProperty('IsFullTextInstalled') + OBJECTPROPERTY(OBJECT_ID('Entry'), 'TableFullTextChangeTrackingOn')")
                .List()[0];

            return (int) isFullTextEnabled == 2
                ? SearchUsingFullText(searchText)
                : SearchUsingLike(searchText);
        }

        private IEnumerable<EntryRevision> SearchUsingFullText(string searchText)
        {
            var searchTerms = searchText.Split(' ', '-', '_').Where(x => !string.IsNullOrEmpty(x)).Select(x => "\"" + x + "*\"");
            var searchQuery = string.Join(" OR ", searchTerms.ToArray());

            var query = session.QueryOver<Entry>()
                .Where(Expression.Sql("CONTAINS(*, ?)", searchQuery, NHibernateUtil.String))
                .And(e => e.Status != EntryStatus.Private)
                .Take(15)
                .List<EntryRevision>();

            return query;
        }

        public IEnumerable<EntryRevision> SearchUsingLike(string searchText)
        {
            var searchTerms = new string(searchText.Where(x => char.IsLetterOrDigit(x) || x == ' ').ToArray());
            searchTerms = searchTerms.Replace(" ", "%");

            var query = session.QueryOver<Entry>()
                .Where
                (
                    Restrictions.On<Entry>(e => e.LatestRevision.Body).IsLike(searchTerms, MatchMode.Anywhere) 
                    ||
                    Restrictions.On<Entry>(e => e.Title).IsLike(searchTerms, MatchMode.Anywhere)
                )
                .Take(15)
                .List<EntryRevision>();

            return query;
        }
    }
}
