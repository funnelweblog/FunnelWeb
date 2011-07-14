using System;
using System.Collections.Generic;
using FunnelWeb.DatabaseDeployer.DbProviders;
using FunnelWeb.Model;
using NHibernate;

namespace FunnelWeb.Repositories.Queries
{
    public class GetEntryWithPingbacksQuery : IQuery<Entry>
    {
        private readonly string name;

        public GetEntryWithPingbacksQuery(string name)
        {
            this.name = name;
        }

        public IEnumerable<Entry> Execute(ISession session, IDatabaseProvider databaseProvider)
        {
            return session
                .QueryOver<Entry>()
                .Where(e => e.Name == name)
                .JoinQueryOver(p => p.Pingbacks)
                .List();
        }
    }
}
