using System.Collections.Generic;
using NHibernate;

namespace FunnelWeb.Repositories
{
    public interface IQuery<out TResult>
    {
        IEnumerable<TResult> Execute(ISession session);
        IEnumerable<TResult> Execute(ISession session, int skip, int take);
    }
}