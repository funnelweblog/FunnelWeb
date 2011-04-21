using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FunnelWeb.Model;
using FunnelWeb.Model.Strings;
using Iesi.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace FunnelWeb.Repositories.Queries
{
    public class EntryByNameAndRevisionQuery : IQuery<Entry>
    {
        private readonly PageName name;
        private readonly int revision;

        public EntryByNameAndRevisionQuery(string name, int revision)
        {
            this.name = name;
            this.revision = revision;
        }

        public IList<Entry> Execute(ISession session)
        {
            var items = session.CreateCriteria<Entry>().List<Entry>();

            var entryQuery = (Hashtable)session.CreateCriteria<Entry>("entry")
                .Add(Restrictions.Eq("entry.Name", name))
                .CreateCriteria("Revisions", "rev")
                .Add(Restrictions.Eq("rev.RevisionNumber", revision))
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
            return new[] { entry };
        }
    }
}