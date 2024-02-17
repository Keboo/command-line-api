using Xunit;

namespace System.CommandLine.Extended.Tests;

public class ParserTests
{
    [Fact]
    public void A_subcommand_wont_overflow_when_checking_maximum_argument_capacity()
    {
        // Tests bug identified in https://github.com/dotnet/command-line-api/issues/997

        var argument1 = new CliArgument<string>("arg1");

        var argument2 = new CliArgument<string[]>("arg2");

        var command = new CliCommand("subcommand")
        {
            argument1,
            argument2
        };

        var rootCommand = new CliRootCommand
        {
            command
        };

        var parseResult = rootCommand.Parse("subcommand arg1 arg2");

        Assert.True(false, "Uncomment Below Code");
        //Action act = () => parseResult.GetCompletions();
        //act.Should().NotThrow();
    }
}
