using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bindable.Core.Language;
using FunnelWeb.Model.Strings;
using Iesi.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;

namespace FunnelWeb.Model.Repositories.Internal
{
    public class EntryRepository : IEntryRepository
    {
        private readonly ISession session;
        
        public EntryRepository(ISession session)
        {
            this.session = session;
        }

        public IQueryable<Entry> GetEntries()
        {
            return session.Query<Entry>();
        }

        public IEnumerable<Entry> GetUnpublished()
        {
            return session.Query<Entry>().Where(x => x.Status != EntryStatus.PublicBlog)
                .OrderByDescending(x => x.Published);
        }

        public Entry GetEntry(int id)
        {
            return session.QueryOver<Entry>()
                .Where(x => x.Id == id)
                .Fetch(x => x.Revisions).Eager()
                .Fetch(x => x.Tags).Eager()
                .SingleOrDefault();
        }

        public Entry GetEntry(PageName name)
        {
            var entry = session
                .QueryOver<Entry>()
                .Where(e => e.Name == name)
                .Left.JoinQueryOver(e => e.Comments)
                .SingleOrDefault<Entry>();
            return entry;
        }

        public Entry GetEntry(PageName name, int revisionNumber)
        {
            if (revisionNumber <= 0) 
                return GetEntry(name);

            var entryQuery = (Hashtable)session.CreateCriteria<Entry>("entry")
                .Add(Restrictions.Eq("entry.Name", name))
                .CreateCriteria("Revisions", "rev")
                    .Add(Restrictions.Eq("rev.RevisionNumber", revisionNumber))
                    .AddOrder(Order.Desc("rev.Revised"))
                .SetMaxResults(1)
                .SetResultTransformer(Transformers.AliasToEntityMap)
                .UniqueResult();

            var entry = (Entry)entryQuery["entry"];
            entry.LatestRevision = (Revision)entryQuery["rev"];

            var comments = session.CreateFilter(entry.Comments, "")
                .SetFirstResult(0)
                .SetMaxResults(500)
                .List();
            entry.Comments = new HashedSet<Comment>(comments.Cast<Comment>().ToList());
            return entry;
        }

        public Redirect GetClosestRedirect(PageName name)
        {
            var nameSoundEx = SoundEx.Evaluate(name);
            var redirects = session.Query<Redirect>()
                .ToList();
            return redirects.Where(x => SoundEx.Evaluate(x.From) == nameSoundEx).FirstOrDefault();
        }

        public void Save(Entry entry)
        {
            session.SaveOrUpdate(entry);
            if (entry.LatestRevision.RevisionNumber == 0)
            {
                entry.LatestRevision.RevisionNumber = session.Query<Revision>().Where(x => x.Entry.Id == entry.Id).Count();
            }
        }

        public IEnumerable<Entry> Search(string searchText)
        {
            if (string.IsNullOrEmpty(searchText) || searchText.Trim().Length == 0)
            {
                return new Entry[0];
            }

            var isFullTextEnabled = session.CreateSQLQuery("SELECT FullTextServiceProperty('IsFullTextInstalled')").List()[0];
            return (int) isFullTextEnabled == 0 
                ? SearchUsingLike(searchText) 
                : SearchUsingFullText(searchText);
        }

        private IEnumerable<Entry> SearchUsingFullText(string searchText)
        {
            var searchTerms = searchText.Split(' ', '-', '_').Where(x => !string.IsNullOrEmpty(x)).Select(x => "\"" + x + "*\"");
            var searchQuery = string.Join(" OR ", searchTerms.ToArray());
            var query = session.CreateSQLQuery(
                @"select {e.*} from [Entry] {e}
                    inner join (
                        select z.*, [Rank] from [Entry] z
                            inner join [Revision] rv on z.Id = rv.EntryId
                            inner join CONTAINSTABLE([Revision], *, :searchString) as searchTable1 on searchTable1.[Key] = rv.Id
                        union all 
                        select z.*, [Rank] from [Entry] z
                            inner join CONTAINSTABLE([Entry], *, :searchString) as searchTable2 on searchTable2.[Key] = z.Id
                    ) as Entries on Entries.Id = e.Id
                    where e.Status != '" + EntryStatus.Private + @"'
                    order by [Rank] desc")
                .SetMaxResults(300)
                .SetString("searchString", searchQuery)
                .SetReadOnly(true)
                .List<Entry>()
                .Distinct()
                .Take(15)
                .ToList();
            return query;
        }

        public IEnumerable<Entry> SearchUsingLike(string searchText)
        {
            var searchTerms = "%" + new string(searchText.Where(x => char.IsLetterOrDigit(x) || x == ' ').ToArray()) + "%";
            searchTerms.Replace(" ", "%");

            var entryQuery = (ArrayList)session.CreateCriteria<Entry>("entry")
                .CreateCriteria("entry.Revisions", "rev")
                .Add(Restrictions.EqProperty("rev.Id", Projections.SubQuery(
                    DetachedCriteria.For<Revision>("rv")
                        .SetProjection(Projections.Property("rv.Id"))
                        .AddOrder(Order.Desc("rv.Revised"))
                        .Add(Restrictions.EqProperty("rv.Entry.Id", "entry.Id"))
                        .SetMaxResults(1))))
                .Add(new OrExpression(
                    Restrictions.Like("entry.Title", searchTerms),
                    Restrictions.Like("rev.Body", searchTerms)
                    ))
                .Add(Restrictions.Not(Restrictions.Eq("entry.Status", EntryStatus.Private)))
                .SetFirstResult(0)
                .SetMaxResults(15)
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
    }
}
