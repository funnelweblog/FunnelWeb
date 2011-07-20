using System;
using NHibernate;
using NHibernate.AdoNet;
using NHibernate.Engine;

namespace FunnelWeb.DatabaseDeployer.DbProviders
{
    public class ProfiledSqlClientBatchingBatcherFactory : IBatcherFactory
    {
        public virtual IBatcher CreateBatcher(ConnectionManager connectionManager, IInterceptor interceptor)
        {
            return new ProfiledSqlClientBatchingBatcher(connectionManager, interceptor);
        }
    }
}