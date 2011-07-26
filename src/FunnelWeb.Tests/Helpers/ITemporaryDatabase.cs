using System;
using DbUp.Helpers;
using FunnelWeb.DatabaseDeployer;
using FunnelWeb.Repositories;
using NHibernate;

namespace FunnelWeb.Tests.Helpers
{
    public interface ITemporaryDatabase : IDisposable, IConnectionStringProvider
    {
        void WithRepository(Action<IRepository> callback);
        void WithSession(Action<ISession> callback);
        AdHocSqlRunner AdHoc { get; }
        void CreateAndDeploy();
        ScriptedExtension ScriptProviderFor<T>(T extensionWithScripts) where T : IRequireDatabaseScripts;
    }
}