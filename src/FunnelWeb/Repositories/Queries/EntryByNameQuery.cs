using System;
using System.Collections.Generic;
using System.Linq;
using FunnelWeb.Model;
using FunnelWeb.Model.Strings;
using FunnelWeb.Providers.Database;
using FunnelWeb.Repositories.Projections;
using FunnelWeb.DatabaseDeployer.Infrastructure;
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

        public IEnumerable<EntryRevision> Execute(ISession session, IDatabaseProvider databaseProvider)
        {
            var comments = session
                            .QueryOver<Comment>()
                            .JoinQueryOver(c => c.Entry)
                            .Where(e => e.Name == PageName)
                            .Future(databaseProvider);

            var pingbacksQuery = session
                .QueryOver<Pingback>()
                .Where(p=>!p.IsSpam)
                .OrderBy(e=>e.Received).Desc
                .JoinQueryOver(c => c.Entry)
                .Where(e => e.Name == PageName);

            var pingbackCountQuery = pingbacksQuery
                .ToRowCountQuery()
                .FutureValue<Pingback, int>(databaseProvider);

            var pingbacks = pingbacksQuery
                            .Take(10)
                            .Future(databaseProvider);

            var tags = session
                        .QueryOver<Tag>()
                        .JoinQueryOver<Entry>(t => t.Entries)
                        .Where(e => e.Name == PageName)
                        .Future(databaseProvider);

            var singleOrDefault = session
                .QueryOver<Entry>()
                .Where(e => e.Name == PageName)
                .SelectList(EntryRevisionProjections.FromEntry())
                .TransformUsing(Transformers.AliasToBean<EntryRevision>())
                .Future<Entry, EntryRevision>(databaseProvider)
                .SingleOrDefault();

            if (singleOrDefault == null)
                return Enumerable.Empty<EntryRevision>();

            singleOrDefault.Comments = comments.ToList();
            singleOrDefault.Tags = tags.ToList();
            singleOrDefault.Pingbacks = pingbacks.ToList();
            singleOrDefault.PingbackCount = pingbackCountQuery.Value;
            singleOrDefault.Entry = session
                .QueryOver<Entry>()
                .Where(e => e.Name == PageName)
                .FutureValue<Entry>();

            return new[]{singleOrDefault};
        }
    }
}
