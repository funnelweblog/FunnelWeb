using System;
using System.Collections.Generic;
using FunnelWeb.Model;
using FunnelWeb.Model.Strings;
using FunnelWeb.Repositories.Projections;
using NHibernate;
using NHibernate.Transform;

namespace FunnelWeb.Repositories.Queries
{
    public class EntryByNameAndRevisionQuery : IQuery<EntryRevision>
    {
        private readonly PageName name;
        private readonly int revision;

        public EntryByNameAndRevisionQuery(string name, int revision)
        {
            this.name = name;
            this.revision = revision;
        }

        public IEnumerable<EntryRevision> Execute(ISession session)
        {
            var entryAlias = default(Entry);

            var entryQuery = session
               .QueryOver<Revision>()
               .Where(r => r.RevisionNumber == revision)
               .Left.JoinQueryOver(r => r.Entry, () => entryAlias)
               .Where(e => e.Name == name)
                //.WithSubquery.WhereProperty(e => e.Id).In(QueryOver.Of<Entry>().Where(e => e.Name == name))
               .SelectList(EntryRevisionProjections.FromRevision(entryAlias))
               .TransformUsing(Transformers.AliasToBean<EntryRevision>());

            return entryQuery.Future<EntryRevision>();
        }

        public IEnumerable<EntryRevision> Execute(ISession session, int skip, int take)
        {
            return Execute(session);
        }
    }
}