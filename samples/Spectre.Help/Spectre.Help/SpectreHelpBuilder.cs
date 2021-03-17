using Spectre.Console;
using System;
using System.CommandLine;
using System.CommandLine.Help;

namespace Spectre.Help
{
    public class SpectreHelpBuilder : HelpBuilder
    {
        private Lazy<IAnsiConsole> AnsiConsole { get; } 

        public SpectreHelpBuilder(IConsole console) 
            : base(console)
        {
            AnsiConsole = new Lazy<IAnsiConsole>(() =>
            {
                return Spectre.Console.AnsiConsole.Create(new AnsiConsoleSettings
                {
                    Ansi = AnsiSupport.Detect,
                    ColorSystem = ColorSystemSupport.Detect,
                    Out = new ConsoleTextWriter(Console.Out),
                });
            });
        }

        public override void Write(ICommand command)
        {   
            var table = new Table()
                .Border(TableBorder.None)
                .Expand()
                .Title($"{command.Name} [yellow]{command.Description}[/]")
                .AddColumn(new TableColumn(""));
            table.ShowHeaders = false;

            var usageTable = new Table()
                .Expand()
                .AddColumns("", "")
                .AddRow(new Text(Resources.Instance.HelpUsageTile()), new Text(GetUsage(command)));
            usageTable.ShowHeaders = false;
            table.AddRow(usageTable);

            var argumentsTable = new Table()
                .Expand()
                .AddColumns("", "")
                .Title(Resources.Instance.HelpArgumentsTitle());
            argumentsTable.ShowHeaders = false;
            foreach (var (name, value) in GetCommandArguments(command))
            {
                argumentsTable.AddRow(new Text(name), new Text(value));
            }
            table.AddRow(argumentsTable);

            var optionTable = new Table()
                .Expand()
                .AddColumns("", "")
                .Title(Resources.Instance.HelpOptionsTitle());
            optionTable.ShowHeaders = false;
            foreach (var (name, value) in GetOptions(command))
            {
                optionTable.AddRow(new Text(name), new Text(value));
            }
            table.AddRow(optionTable);

            var subcommandsTable = new Table()
                .Expand()
                .AddColumns("", "")
                .Title(Resources.Instance.HelpCommandsTitle());
            subcommandsTable.ShowHeaders = false;
            foreach (var (name, value) in GetSubcommands(command))
            {
                subcommandsTable.AddRow(new Text(name), new Text(value));
            }
            table.AddRow(subcommandsTable);

            AnsiConsole.Value.Render(table);

            //base.Write(command);
        }
    }
}
