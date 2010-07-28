using System;
using System.ComponentModel;
using System.Data.SqlClient;
using Bindable.Core.CommandLine;
using Bindable.DatabaseManagement;

namespace FunnelWeb.DatabaseDeployer.Commands
{
    [Name("status", "s")]
    [Description("Provides information about the current database version")]
    [Definition("status [CONNECTIONSTRING] [options]")]
    [Example("status", true)]
    [Example("status Server=(local);Database=BindableCMS;Trusted_connection=true")]
    public class StatusCommand : AbstractPromptedCommand
    {
        protected string ConnectionString { get; private set; }
        protected bool NoWait { get; private set; }
        protected SqlConnectionStringBuilder ConnectionStringBuilder { get; private set; }
        
        protected override void InitializeOptions()
        {
            OptionsParser.Ignore("install");
            OptionsParser.Add("x|no-wait", "Do not wait for keyboard input after installation.", value => NoWait = true);
            OptionsParser.AddUnnamed("ConnectionString", "The SQL Server connection string to use.", next => ConnectionString = next);
        }

        protected override void InitializePrompts()
        {
            ConnectionStringBuilder = new SqlConnectionStringBuilder();
            ConnectionStringBuilder.DataSource = "(local)";
            ConnectionStringBuilder.InitialCatalog = "FunnelWeb";
            ConnectionStringBuilder.IntegratedSecurity = true;
            OptionsPrompter.AddQuestion("Please enter the name of the machine running SQL Server, or hit enter to use the local machine:", value => ConnectionStringBuilder.DataSource = value);
            OptionsPrompter.AddQuestion("Please enter the database name, or hit enter for 'BindableCMS':", value => ConnectionStringBuilder.InitialCatalog = value);
        }

        protected override void Execute(CommandExecutionContext context)
        {
            if (string.IsNullOrEmpty(ConnectionString) && ConnectionStringBuilder != null)
            {
                ConnectionString = ConnectionStringBuilder.ToString();
            }
            if (string.IsNullOrEmpty(ConnectionString) && ConnectionStringBuilder != null)
            {
                ConnectionString = "Server=(local);Database=FunnelWeb;trusted_connection=yes;";
            }

            try
            {
                // Validate the connection string
                SqlDatabaseHelper.ValidateConnectionStringOrThrow(ConnectionString);

                // Ensure the database exists - if not, create it
                UltraConsole.WriteHeading("Database upgrade");
                UltraConsole.WriteParagraph("  Connection string: {0}", ConnectionString);
                UltraConsole.WriteLine("Ensuring database exists...");
                var manager = new ApplicationDatabase(ConnectionString);
                if (!manager.DoesDatabaseExist())
                {
                    UltraConsole.WriteLine("Database does not exist. ");
                    return;
                }
                UltraConsole.WriteLine("Database exists.");

                UltraConsole.WriteHeading("Getting information about current setup...");
                using (UltraConsole.Indent())
                {
                    UltraConsole.WriteColumns("Database version:", manager.GetCurrentVersion().ToString());
                    UltraConsole.WriteColumns("Can be upgraded to:", manager.GetApplicationVersion().ToString());
                }

                // Display the result
                UltraConsole.WriteLine(ConsoleColor.Green, "Success");
            }
            catch (FormatException ex)
            {
                UltraConsole.WriteLine(ConsoleColor.Red, "Connection string was invalid: {0}", ex.Message);
                return;
            }
        }
    }
}