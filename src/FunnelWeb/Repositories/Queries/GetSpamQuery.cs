using System.Collections.Generic;
using System.Linq;
using FunnelWeb.Model;
using FunnelWeb.Providers.Database;
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