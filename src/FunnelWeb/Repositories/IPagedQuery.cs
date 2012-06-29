using System;
using FunnelWeb.Providers.Database;
using NHibernate;

namespace FunnelWeb.Repositories
{
    public interface IPagedQuery<T>
    {
        PagedResult<T> Execute(ISession session, IDatabaseProvider databaseProvider, int skip, int take);
    }
}