using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace FunnelWeb.Providers.Database.Sql
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