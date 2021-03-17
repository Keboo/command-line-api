using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

namespace Spectre.Help
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = new RootCommand()
            {
                new Argument<string>("path", "The path to process"),
                new Option<string>("--name", "What's in a name? That which we call a rose by any other name would smell as sweet.")
                {
                    IsRequired = true
                },
                new Command("create-pun", "Create text for a command that is under the weather")
            };
            Parser parser = new CommandLineBuilder(root)
                .UseDefaults()
                .UseHelpBuilder(ctx => new SpectreHelpBuilder(ctx.Console))
                .Build();
            parser.Invoke(args);
        }
    }
}
