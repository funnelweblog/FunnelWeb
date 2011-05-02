using System;
using System.Collections.Generic;
using NHibernate;

namespace FunnelWeb.Repositories
{
    public interface IQueryWithCount<out T> : IQuery<T>
    {
        IEnumerable<T> Execute(ISession session, int skip, int take, out int totalCount);
    }
}