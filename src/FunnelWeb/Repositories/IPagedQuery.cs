using System;
using FunnelWeb.DatabaseDeployer.DbProviders;
using NHibernate;

namespace FunnelWeb.Repositories
{
    public interface IPagedQuery<T>
    {
        PagedResult<T> Execute(ISession session, IDatabaseProvider databaseProvider, int skip, int take);
    }
}