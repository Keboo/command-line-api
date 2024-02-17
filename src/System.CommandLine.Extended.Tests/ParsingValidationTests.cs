using FluentAssertions;
using System.CommandLine.Parsing;
using Xunit;

namespace System.CommandLine.Extended.Tests;

public class ParsingValidationTests
{

    [Fact]
    public void A_command_with_subcommands_is_invalid_to_invoke_if_it_has_no_handler()
    {
        var outer = new CliCommand("outer")
        {
            new CliCommand("inner")
            {
                new CliCommand("inner-er")
            }
        };

        var result = outer.Parse("outer inner");

        result.Errors
              .Should()
              .ContainSingle(
                  e => e.Message.Equals(LocalizationResources.RequiredCommandWasNotProvided()) &&
                       ((CommandResult)e.SymbolResult).Command.Name.Equals("inner"));
    }

    [Fact]
    public void A_root_command_with_subcommands_is_invalid_to_invoke_if_it_has_no_handler()
    {
        var rootCommand = new CliRootCommand();
        var inner = new CliCommand("inner");
        rootCommand.Add(inner);

        var result = rootCommand.Parse("");

        result.Errors
              .Should()
              .ContainSingle(
                  e => e.Message.Equals(LocalizationResources.RequiredCommandWasNotProvided()) &&
                       ((CommandResult)e.SymbolResult).Command == rootCommand);
    }

    [Fact]
    public void A_command_with_subcommands_is_valid_to_invoke_if_it_has_a_handler()
    {
        var outer = new CliCommand("outer");
        var inner = new CliCommand("inner");
        inner.SetAction((_) => { });
        var innerer = new CliCommand("inner-er");
        outer.Subcommands.Add(inner);
        inner.Subcommands.Add(innerer);

        var result = outer.Parse("outer inner");

        result.Errors.Should().BeEmpty();
        result.CommandResult.Command.Should().BeSameAs(inner);
    }
}
