using System.Collections.Generic;
using FunnelWeb.Providers.Database;
using NHibernate;

namespace FunnelWeb.Repositories
{
    public interface IQuery<out TResult>
    {
        IEnumerable<TResult> Execute(ISession session, IDatabaseProvider databaseProvider);
    }
}