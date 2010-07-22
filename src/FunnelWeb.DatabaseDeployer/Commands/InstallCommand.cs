using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using Bindable.Core.CommandLine;
using Bindable.DatabaseManagement;

namespace FunnelWeb.DatabaseDeployer.Commands
{
    [Name("install", "upgrade", "i", "inst", "ins")]
    [Description("Installs or upgrades the database")]
    [Definition("install [CONNECTIONSTRING] [options]")]
    [Example("install", true)]
    [Example("install " + ApplicationDatabase.DefaultConnectionString)]
    [Example("install " + ApplicationDatabase.DefaultConnectionString + " --no-create --no-wait", true)]
    public class InstallCommand : AbstractPromptedCommand
    {
        public InstallCommand()
        {
            UserAccount = "NT AUTHORITY\\NETWORK SERVICE";
        }

        protected string ConnectionString { get; private set; }
        protected string UserAccount { get; private set; }
        protected bool NoCreate { get; private set; }
        protected bool NoWait { get; private set; }
        protected SqlConnectionStringBuilder ConnectionStringBuilder { get; private set; }
        
        protected override void InitializeOptions()
        {
            OptionsParser.Ignore("install");
            OptionsParser.Add("u=|user-account=", "The name of the account to grant permissions to.", value => UserAccount = value);
            OptionsParser.Add("n|no-create", "Do not create the database.", value => NoCreate = true);
            OptionsParser.Add("x|no-wait", "Do not wait for keyboard input after installation.", value => NoWait = true);
            OptionsParser.AddUnnamed("ConnectionString", "The SQL Server connection string to use.", next => ConnectionString = next);
        }

        protected override void InitializePrompts()
        {
            ConnectionStringBuilder = new SqlConnectionStringBuilder(ApplicationDatabase.DefaultConnectionString);

            OptionsPrompter.AddQuestion("Please enter the name of the machine running SQL Server, or hit enter to use the local machine:", value => ConnectionStringBuilder.DataSource = value);
            OptionsPrompter.AddQuestion("Please enter the database name, or hit enter for 'FunnelWeb':", value => ConnectionStringBuilder.InitialCatalog = value);
        }

        protected override void Execute(CommandExecutionContext context)
        {
            if (string.IsNullOrEmpty(ConnectionString) && ConnectionStringBuilder != null)
            {
                ConnectionString = ConnectionStringBuilder.ToString();
            }
            if (string.IsNullOrEmpty(ConnectionString))
            {
                ConnectionString = ApplicationDatabase.DefaultConnectionString;
            }

            try
            {
                SqlDatabaseHelper.ValidateConnectionStringOrThrow(ConnectionString);

                UltraConsole.WriteHeading("Database upgrade");
                UltraConsole.WriteParagraph("  Connection string: {0}", ConnectionString);
                UltraConsole.WriteLine("Ensuring database exists...");
                var manager = new ApplicationDatabase(ConnectionString);
                if (!manager.DoesDatabaseExist())
                {
                    UltraConsole.WriteLine("Database does not exist. ");
                    if (!NoCreate)
                    {
                        UltraConsole.WriteLine("Creating database...");
                        manager.CreateDatabase();
                        UltraConsole.WriteLine("Database created");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    UltraConsole.WriteLine("Database exists.");
                }

                UltraConsole.WriteHeading("Granting access to user '{0}'", UserAccount);
                manager.GrantAccessToLogin(UserAccount);

                UltraConsole.WriteHeading("Getting information about current setup...");
                using (UltraConsole.Indent())
                {
                    UltraConsole.WriteColumns("Current database version:", manager.GetCurrentVersion().ToString());
                    UltraConsole.WriteColumns("Will be upgraded to:", manager.GetApplicationVersion().ToString());
                }
                UltraConsole.WriteHeading("Performing upgrade...");
                var result = manager.PerformUpgrade();

                UltraConsole.WriteParagraph("Upgraded from {0} to {1}", result.OriginalVersion, result.UpgradedVersion);
                if (result.Scripts != null && result.Scripts.Count() > 0)
                {
                    UltraConsole.WriteHeading("The following scripts were executed:");
                    UltraConsole.WriteTable(
                        result.Scripts,
                        table => table.AddColumn("Version", script => script.VersionNumber)
                        );
                }

                if (result.Successful)
                {
                    UltraConsole.WriteLine(ConsoleColor.Green, "Database upgrade was successful.");
                }
                else
                {
                    UltraConsole.WriteLine(ConsoleColor.Red, "Database upgrade failed. Please see the list of scripts that were executed above and the error below.");
                    UltraConsole.WriteLine(result.Error.ToString());
                }
            }
            catch (FormatException ex)
            {
                UltraConsole.WriteLine(ConsoleColor.Red, "Connection string was invalid: {0}", ex.Message);
            }
        }
    }
}