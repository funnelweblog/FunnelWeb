using System;
using System.Data.Common;
using System.Data.SqlClient;
using MvcMiniProfiler;
using MvcMiniProfiler.Data;

namespace FunnelWeb.DatabaseDeployer.DbProviders
{
    public class ProfiledSqlDbCommand : ProfiledDbCommand
    {
        public ProfiledSqlDbCommand(SqlCommand cmd, DbConnection conn, MiniProfiler profiler)
            : base(cmd, conn, profiler)
        {
            Command = cmd;
        }

        public SqlCommand Command { get; set; }

        private DbTransaction trans;

        protected override DbTransaction DbTransaction
        {
            get { return trans; }
            set
            {
                trans = value;
                var profiledSqlDbTransaction = value as ProfiledSqlDbTransaction;
                Command.Transaction = profiledSqlDbTransaction == null ? (SqlTransaction)value : profiledSqlDbTransaction.Transaction;
            }
        }
    }
}