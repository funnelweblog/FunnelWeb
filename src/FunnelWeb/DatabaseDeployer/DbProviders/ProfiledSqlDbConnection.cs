using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using MvcMiniProfiler;
using MvcMiniProfiler.Data;

namespace FunnelWeb.DatabaseDeployer.DbProviders
{
    public class ProfiledSqlDbConnection : ProfiledDbConnection
    {
        public ProfiledSqlDbConnection(SqlConnection connection, MiniProfiler profiler)
            : base(connection, profiler)
        {
            Connection = connection;
        }

        public SqlConnection Connection { get; set; }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new ProfiledSqlDbTransaction(Connection.BeginTransaction(isolationLevel), this);
        }

    }
}