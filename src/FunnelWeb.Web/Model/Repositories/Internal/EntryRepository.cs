using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bindable.Core.Language;
using Iesi.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using FunnelWeb.Web.Application.Validation;
using FunnelWeb.Web.Model.Strings;

namespace FunnelWeb.Web.Model.Repositories.Internal
{
    public class EntryRepository : IEntryRepository
    {
        private readonly ISession _session;
        private readonly IEntityValidator _validator;
        
        public EntryRepository(ISession session, IEntityValidator validator)
        {
            _session = session;
            _validator = validator;
        }

        public IQueryable<Entry> GetEntries()
        {
            return _session.Linq<Entry>();
        }

        public IEnumerable<Entry> GetUnpublished()
        {
            var feedItemsCriteria = DetachedCriteria.For<FeedItem>("item")
                .SetProjection(Projections.Property("item.Entry.Id"));
            return _session.CreateCriteria<Entry>()
                .Add(Subqueries.PropertyNotIn("Id", feedItemsCriteria))
                .AddOrder(Order.Desc("Published"))
                .List<Entry>();
        }

        public Entry GetEntry(PageName name)
        {
            var entryQuery = (Hashtable)_session.CreateCriteria<Entry>("entry")
                .Add(Restrictions.Eq("entry.Name", name))
                .CreateCriteria("Revisions", "rev")
                    .AddOrder(Order.Desc("rev.Revised"))
                .SetMaxResults(1)
                .SetResultTransformer(Transformers.AliasToEntityMap)
                .UniqueResult();

            if (entryQuery == null) return null;

            var entry = (Entry)entryQuery["entry"];
            entry.LatestRevision = (Revision)entryQuery["rev"];

            var comments = _session.CreateFilter(entry.Comments, "")
                .SetFirstResult(0)
                .SetMaxResults(500)
                .List();
            entry.Comments = new HashedSet<Comment>(comments.Cast<Comment>().ToList());

            return entry;
        }

        public Entry GetEntry(PageName name, int revisionNumber)
        {
            if (revisionNumber <= 0) 
                return GetEntry(name);

            var entryQuery = (Hashtable)_session.CreateCriteria<Entry>("entry")
                .Add(Restrictions.Eq("entry.Name", name))
                .CreateCriteria("Revisions", "rev")
                    .Add(Restrictions.Eq("rev.RevisionNumber", revisionNumber))
                    .AddOrder(Order.Desc("rev.Revised"))
                .SetMaxResults(1)
                .SetResultTransformer(Transformers.AliasToEntityMap)
                .UniqueResult();

            var entry = (Entry)entryQuery["entry"];
            entry.LatestRevision = (Revision)entryQuery["rev"];

            var comments = _session.CreateFilter(entry.Comments, "")
                .SetFirstResult(0)
                .SetMaxResults(500)
                .List();
            entry.Comments = new HashedSet<Comment>(comments.Cast<Comment>().ToList());
            return entry;
        }

        public Redirect GetClosestRedirect(PageName name)
        {
            var nameSoundEx = SoundEx.Evaluate(name);
            var redirects = _session.Linq<Redirect>()
                .ToList();
            return redirects.Where(x => SoundEx.Evaluate(x.From) == nameSoundEx).FirstOrDefault();
        }

        public ValidationResult Save(Entry entry)
        {
            var results = _validator.Validate(entry);
            if (results.IsValid)
            {
                _session.SaveOrUpdate(entry);

                CaptureChangeSummary(entry);
                if (entry.LatestRevision.RevisionNumber == 0)
                {
                    entry.LatestRevision.RevisionNumber = _session.Linq<Revision>().Where(x => x.Entry.Id == entry.Id).Count();
                }
                _session.Update(entry.LatestRevision);
            }
            return results;
        }

        public IEnumerable<Entry> Search(string searchText)
        {
            if (string.IsNullOrEmpty(searchText) || searchText.Trim().Length == 0)
            {
                return new Entry[0];
            }

            var isFullTextEnabled = _session.CreateSQLQuery("SELECT FullTextServiceProperty('IsFullTextInstalled')").List()[0];
            return (int) isFullTextEnabled == 0 
                ? SearchUsingLike(searchText) 
                : SearchUsingFullText(searchText);
        }

        private IEnumerable<Entry> SearchUsingFullText(string searchText)
        {
            var searchTerms = searchText.Split(' ', '-', '_').Where(x => !string.IsNullOrEmpty(x)).Select(x => "\"" + x + "*\"");
            var searchQuery = string.Join(" OR ", searchTerms.ToArray());
            var query = _session.CreateSQLQuery(
                @"select {e.*} from [Entry] {e}
                    inner join (
                        select z.*, [Rank] from [Entry] z
                            inner join [Revision] rv on z.Id = rv.EntryId
                            inner join CONTAINSTABLE([Revision], *, :searchString) as searchTable1 on searchTable1.[Key] = rv.Id
                        union all 
                        select z.*, [Rank] from [Entry] z
                            inner join CONTAINSTABLE([Entry], *, :searchString) as searchTable2 on searchTable2.[Key] = z.Id
                    ) as Entries on Entries.Id = e.Id
                    order by [Rank] desc",
                "e",
                typeof(Entry))
                .SetMaxResults(300)
                .SetString("searchString", searchQuery)
                .SetReadOnly(true)
                .List()
                .OfType<Entry>().Distinct().Take(15).ToList();
            return query;
        }


        public IEnumerable<Entry> SearchUsingLike(string searchText)
        {
            var searchTerms = "%" + new string(searchText.Where(x => char.IsLetterOrDigit(x) || x == ' ').ToArray()) + "%";
            searchTerms.Replace(" ", "%");

            var entryQuery = (ArrayList)_session.CreateCriteria<Entry>("entry")
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

        private void CaptureChangeSummary(Entry entry)
        {
            var original = entry.Revisions.Skip(1).FirstOrDefault();
            entry.LatestRevision.ChangeSummary = string.Empty;
        }
    }
}
