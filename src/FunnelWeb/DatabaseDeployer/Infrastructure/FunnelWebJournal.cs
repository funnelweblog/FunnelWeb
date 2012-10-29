using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using DbUp.Engine;
using DbUp.Engine.Output;

namespace FunnelWeb.DatabaseDeployer.Infrastructure
{
    /// <summary>
    /// FunnelWeb has been around longer than DbUp, and has a few backwards compatability issues to deal with, along 
    /// with the concept of extensions. We do this using a custom DbUp Journal.
    /// </summary>
    public sealed class FunnelWebJournal : IJournal
    {
        private const string TableName = "SchemaVersions";
        private readonly string schemaTableName;
        private readonly string sourceIdentifier;
        private readonly Func<IDbConnection> connectionFactory;
        private readonly IUpgradeLog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunnelWebJournal"/> class.
        /// </summary>
        /// <param name="connectionFactory"></param>
        /// <param name="sourceIdentifier">The source identifier - usually the ID of the extension or FunnelWeb.DatabaseDeployer.</param>
        /// <param name="logger">The log.</param>
        /// <param name="schema">Database Schema</param>
        /// <example>
        /// var journal = new TableJournal("Server=server;Database=database;Trusted_Connection=True;");
        /// </example>
        public FunnelWebJournal(Func<IDbConnection> connectionFactory, string sourceIdentifier, IUpgradeLog logger, string schema)
        {

            schemaTableName = string.IsNullOrEmpty(schema) ? TableName : string.Format("{0}.{1}", schema, TableName);
            this.connectionFactory = connectionFactory;
            this.sourceIdentifier = sourceIdentifier;
            log = logger;
        }

        /// <summary>
        /// Recalls the version number of the database.
        /// </summary>
        /// <returns>All executed scripts.</returns>
        public string[] GetExecutedScripts()
        {
            log.WriteInformation("Fetching list of already executed scripts.");
            var exists = DoesTableExist();
            if (!exists)
            {
                log.WriteInformation(string.Format("The {0} table could not be found. The database is assumed to be at version 0.", schemaTableName));
                return new string[0];
            }

            DealWithLegacyScripts();

            var scripts = new List<string>();

            RunCommand(
                string.Format("select [ScriptName] from {0} where [SourceIdentifier] = @sourceIdentifier order by [ScriptName]", schemaTableName),
                cmd =>
                {
                    var parameter = cmd.CreateParameter();
                    parameter.ParameterName = "sourceIdentifier";
                    parameter.Value = sourceIdentifier;
                    cmd.Parameters.Add(parameter);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader == null)
                        {
                            var message = String.Format("Expected to be able to read from the database. Command execution failed to return a result.\r\nCommand Text:\t{0}", cmd.CommandText);
                            throw new InvalidOperationException(message);
                        }

                        while (reader.Read())
                            scripts.Add((string)reader[0]);
                    }
                });

            return scripts.ToArray();
        }

        private void DealWithLegacyScripts()
        {
            RunCommand(
                string.Format(@" 
                    UPDATE {0} 
                    set SourceIdentifier='FunnelWeb.DatabaseDeployer'           -- This is the new name
                    where SourceIdentifier LIKE 'PaulPad%'                      -- back before we called it FunnelWeb
                       OR SourceIdentifier LIKE 'FunnelWeb.DatabaseDeployer, Ver%' -- when we accidentally included version numbers 
                    ", schemaTableName),
                cmd => cmd.ExecuteNonQuery());

            RunCommand(
                string.Format(@" 
                    UPDATE {0} 
                    set ScriptName=REPLACE(ScriptName, 'PaulPad', 'FunnelWeb')
                    where ScriptName LIKE 'PaulPad%'
                    ", schemaTableName),
                cmd => cmd.ExecuteNonQuery());
        }

        /// <summary>
        /// Records a database upgrade for a database specified in a given connection string.
        /// </summary>
        /// <param name="script">The script.</param>
        public void StoreExecutedScript(SqlScript script)
        {
            var exists = DoesTableExist();
            if (!exists)
            {
                log.WriteInformation(string.Format("Creating the {0} table", schemaTableName));

                RunCommand(
                    string.Format(
                    @"create table {0} (
	                    [SchemaVersionId] int identity(1,1) not null constraint PK_SchemaVersions_Id primary key nonclustered ,
	                    [VersionNumber] int null,
                        [SourceIdentifier] nvarchar(255) not null,
                        [ScriptName] nvarchar(255) not null, 
	                    [Applied] datetime not null
                    )", schemaTableName),
                    cmd => cmd.ExecuteNonQuery());

                log.WriteInformation(string.Format("The {0} table has been created", schemaTableName));
            }

            DealWithLegacyScripts();

            RunCommand(
                string.Format("insert into {0} (VersionNumber, SourceIdentifier, ScriptName, Applied) values (-1, @sourceIdentifier, @scriptName, @now)", schemaTableName),
                cmd =>
                {
                    var scriptName = cmd.CreateParameter();
                    scriptName.ParameterName = "scriptName";
                    scriptName.Value = script.Name;
                    cmd.Parameters.Add(scriptName);
                    var sourceIdentifierParameter = cmd.CreateParameter();
                    sourceIdentifierParameter.ParameterName = "sourceIdentifier";
                    sourceIdentifierParameter.Value = sourceIdentifier;
                    cmd.Parameters.Add(sourceIdentifierParameter);
                    var nowParameter = cmd.CreateParameter();
                    nowParameter.ParameterName = "now";
                    nowParameter.Value = DateTime.UtcNow;
                    cmd.Parameters.Add(nowParameter);

                    cmd.ExecuteNonQuery();
                });
        }

        private bool DoesTableExist()
        {
            try
            {
                using (var connection = connectionFactory())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = string.Format("select count(*) from {0}", schemaTableName);
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        command.ExecuteScalar();
                        return true;
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
            catch (DbException)
            {
                return false;
            }
        }

        private void RunCommand(string commandText, Action<IDbCommand> executeCallback)
        {
            using (var connection = connectionFactory())
            {
                connection.Open();
                using (var txn = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = commandText;
                        command.CommandType = CommandType.Text;
                        command.Transaction = txn;

                        executeCallback(command);
                    }
                    txn.Commit();
                }
            }
        }
    }
}