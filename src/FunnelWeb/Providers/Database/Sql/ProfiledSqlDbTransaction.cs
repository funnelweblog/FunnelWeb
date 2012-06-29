using System.Data.SqlClient;
using StackExchange.Profiling.Data;

namespace FunnelWeb.Providers.Database.Sql
{
    public class ProfiledSqlDbTransaction : ProfiledDbTransaction
    {
        public ProfiledSqlDbTransaction(SqlTransaction transaction, ProfiledDbConnection connection)
            : base(transaction, connection)
        {
            Transaction = transaction;
        }

        public SqlTransaction Transaction { get; set; }
    }
}