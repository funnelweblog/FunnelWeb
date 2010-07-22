using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Bindable.Core.CommandLine;

namespace FunnelWeb.DatabaseDeployer.Commands
{
    [Name("menu")]
    [Description("Provides a prompted menu for users unfamiliar with the command line")]
    [Definition("menu [COMMAND]")]
    [Example("menu", true)]
    [Example("menu install")]
    public class MenuCommand : AbstractCommand
    {
        public string Command { get; set; }

        protected override void InitializeOptions()
        {
            OptionsParser.Ignore("menu");
            OptionsParser.AddUnnamed("COMMAND", "The command to use.", value => Command = value);
        }

        internal static IPromptedCommand GetCommand(string mode, CommandExecutionContext context)
        {
            mode = mode.ToLower(CultureInfo.CurrentCulture);
            var command = context.CommandLocator.FindCommand(mode) as IPromptedCommand;
            if (command == null)
            {
                throw new InvalidOperationException(string.Format("Invalid mode ('{0}')", mode));
            }
            return command;
        }

        protected override void Execute(CommandExecutionContext context)
        {
            while (true)
            {
                var mode = UltraConsole.PromptForOptions("Please select a command:",
                    new Dictionary<string, string>
                    {
                        { "i", "[I]nstall" },
                        { "s", "[S]tatus" },
                        { "e", "[E]xit"}
                    });

                if (mode == "e") return;
                var command = GetCommand(mode, context);
                command.ExecutePrompted(context);
            }
        }
    }
}