using System;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using FluentNHibernate.Cfg.Db;

namespace FunnelWeb.DatabaseDeployer.DbProviders
{
    public class SqlCeDatabaseProvider : IDatabaseProvider
    {
        public string DefaultConnectionString
        {
            get { return "Data Source=FunnelWeb.sdf; Persist Security Info=False"; }
        }

        public bool SupportSchema
        {
            get { return false; }
        }

        public bool TryConnect(string connectionString, out string errorMessage)
        {
            //var file = ReplaceDataDirectory(new SqlCeConnectionStringBuilder(connectionString).DataSource);

            try
            {
                var csb = new SqlCeConnectionStringBuilder(connectionString);
                if (!File.Exists(csb.DataSource))
                {
                    new SqlCeEngine(connectionString).CreateDatabase();
                }

                errorMessage = "";
                using (var connection = new SqlCeConnection(csb.ConnectionString))
                {
                    connection.Open();

                    new SqlCeCommand("select 1", connection).ExecuteScalar();
                }
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }

        }

        public IPersistenceConfigurer GetDatabaseConfiguration(IConnectionStringProvider connectionStringProvider)
        {
            return MsSqlCeConfiguration.Standard.ConnectionString(connectionStringProvider.ConnectionString)
                .ShowSql();
        }

        public Func<IDbConnection> GetConnectionFactory(string connectionString)
        {
            return () => new SqlCeConnection(connectionString);
        }

        private static string ReplaceDataDirectory(string inputString)
        {
            var str = inputString.Trim();
            if (string.IsNullOrEmpty(inputString) || !inputString.StartsWith("|DataDirectory|", StringComparison.InvariantCultureIgnoreCase))
            {
                return str;
            }
            var data = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
            if (string.IsNullOrEmpty(data))
            {
                data = AppDomain.CurrentDomain.BaseDirectory;
            }
            if (string.IsNullOrEmpty(data))
            {
                data = string.Empty;
            }
            var length = "|DataDirectory|".Length;
            if ((inputString.Length > "|DataDirectory|".Length) && ('\\' == inputString["|DataDirectory|".Length]))
            {
                length++;
            }
            return Path.Combine(data, inputString.Substring(length));
        }

        
    }
}