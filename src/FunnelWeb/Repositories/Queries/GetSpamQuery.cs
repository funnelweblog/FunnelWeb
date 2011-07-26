using System.Collections.Generic;
using System.Linq;
using FunnelWeb.DatabaseDeployer.DbProviders;
using FunnelWeb.Model;
using NHibernate;
using NHibernate.Linq;

namespace FunnelWeb.Repositories.Queries
{
    public class GetSpamQuery : IQuery<Comment>
    {
        public IEnumerable<Comment> Execute(ISession session, IDatabaseProvider databaseProvider)
        {
            return session
                .Query<Comment>()
                .Where(x => x.Status == 0);
        }
    }
}