using System.Collections.Generic;
using NHibernate;

namespace FunnelWeb.Repositories
{
    public interface IQuery<TResult>
    {
        IList<TResult> Execute(ISession session);
    }
}