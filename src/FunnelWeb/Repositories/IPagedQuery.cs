using System;
using NHibernate;

namespace FunnelWeb.Repositories
{
    public interface IPagedQuery<T>
    {
        PagedResult<T> Execute(ISession session, int skip, int take);
    }
}