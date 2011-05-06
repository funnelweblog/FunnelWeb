using System;
using System.Collections.Generic;
using System.Linq;
using FunnelWeb.Model;
using FunnelWeb.Model.Strings;
using FunnelWeb.Repositories.Projections;
using NHibernate;
using NHibernate.Transform;

namespace FunnelWeb.Repositories.Queries
{
    public class EntryByNameQuery : IQuery<EntryRevision>
    {
        private readonly PageName name;

        public EntryByNameQuery(PageName name)
        {
            this.name = name;
        }

        public PageName PageName
        {
            get { return name; }
        }

        public IEnumerable<EntryRevision> Execute(ISession session)
        {
            var comments = session
                            .QueryOver<Comment>()
                            .JoinQueryOver(c => c.Entry)
                            .Where(e => e.Name == PageName)
                            .Future<Comment>();

            var pingbacks = session
                            .QueryOver<Pingback>()
                            .JoinQueryOver(c => c.Entry)
                            .Where(e => e.Name == PageName)
                            .Future<Pingback>();

            var tags = session
                        .QueryOver<Tag>()
                        .JoinQueryOver<Entry>(t => t.Entries)
                        .Where(e => e.Name == PageName)
                        .Future<Tag>();

            var singleOrDefault = session
                .QueryOver<Entry>()
                .Where(e => e.Name == PageName)
                .SelectList(EntryRevisionProjections.FromEntry())
                .TransformUsing(Transformers.AliasToBean<EntryRevision>())
                .Future<EntryRevision>()
                .SingleOrDefault();

            if (singleOrDefault == null)
                return Enumerable.Empty<EntryRevision>();

            singleOrDefault.Comments = comments.ToList();
            singleOrDefault.Tags = tags.ToList();
            singleOrDefault.Pingbacks = pingbacks.ToList();

            return new[]{singleOrDefault};
        }

        public IEnumerable<EntryRevision> Execute(ISession session, int skip, int take)
        {
            return Execute(session);
        }
    }
}
