using System;
using System.Reflection;
using Bindable.Core.CommandLine;
using FunnelWeb.DatabaseDeployer.Commands;

namespace FunnelWeb.DatabaseDeployer
{
    internal static class Program
    {
        /// <summary>
        /// The entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        internal static void Main(string[] args)
        {
            try
            {
                var context = new CommandExecutionContext(
                    new CommandLocator(Assembly.GetExecutingAssembly()),
                    new HelpTopicLocator(Assembly.GetExecutingAssembly())
                    );
                var command = (args.Length == 0 ? new MenuCommand() : context.CommandLocator.FindCommand(args)) ?? new HelpCommand();
                command.Execute(context, args);
            }
            catch (CommandLineException e)
            {
                UltraConsole.WriteLine("________________________________________________________________________________");
                UltraConsole.WriteLine("ERROR: " + e);
                UltraConsole.WriteLine("________________________________________________________________________________");
                if (string.IsNullOrEmpty(e.HelpTopic))
                {
                    UltraConsole.WriteLine("Try help: database.exe help {0}", e.HelpTopic);
                }
                else
                {
                    UltraConsole.WriteLine("Try help: database.exe help");
                }
                UltraConsole.PressAnyKeyToContinue();
            }
            catch (Exception e)
            {
                UltraConsole.WriteLine("________________________________________________________________________________");
                UltraConsole.WriteLine("ERROR: " + e);
                UltraConsole.WriteLine("________________________________________________________________________________");
                UltraConsole.PressAnyKeyToContinue();
            }
        }

    }
}
