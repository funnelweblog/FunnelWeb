using System.Collections.Generic;
using NHibernate;

namespace FunnelWeb.Repositories
{
    public interface IQuery<out TResult>
    {
        IEnumerable<TResult> Execute(ISession session);
    }
}