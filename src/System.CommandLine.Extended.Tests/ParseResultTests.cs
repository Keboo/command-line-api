// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.CommandLine.Parsing;
using FluentAssertions;
using Xunit;

namespace System.CommandLine.Tests;

public class ParseResultTests
{
    [Fact] // https://github.com/dotnet/command-line-api/pull/2030#issuecomment-1400275332
    public void ParseResult_GetCompletions_returns_global_options_of_given_command_only()
    {
        var leafCommand = new CliCommand("leafCommand")
        {
            new CliOption<string>("--one") { Description = "option one" },
            new CliOption<string>("--two") { Description = "option two" }
        };

        var midCommand1 = new CliCommand("midCommand1")
        {
            leafCommand
        };
        midCommand1.Options.Add(new CliOption<string>("--three1") { Description = "option three 1", Recursive = true });

        var midCommand2 = new CliCommand("midCommand2")
        {
            leafCommand
        };
        midCommand2.Options.Add(new CliOption<string>("--three2") { Description = "option three 2", Recursive = true });

        var rootCommand = new CliCommand("root")
        {
            midCommand1,
            midCommand2
        };

        var result = CliParser.Parse(rootCommand, "root midCommand2 leafCommand --");

        //var completions = result.GetCompletions();
        Assert.True(false, "Uncomment Below Code");
        //completions
        //    .Select(item => item.Label)
        //    .Should()
        //    .BeEquivalentTo("--one", "--two", "--three2");
    }

    [Fact]
    public void Handler_is_null_when_parsed_command_did_not_specify_handler()
        => Assert.True(false, "Uncomment This Code");//new CliRootCommand().Parse("").Action.Should().BeNull();

    [Fact]
    public void Handler_is_not_null_when_parsed_command_specified_handler()
    {
        bool handlerWasCalled = false;

        CliRootCommand command = new();
        //command.SetAction((_) => handlerWasCalled = true);

        ParseResult parseResult = command.Parse("");

        Assert.True(false, "Uncomment Below Code");
        //parseResult.Action.Should().NotBeNull();
        handlerWasCalled.Should().BeFalse();

        //((SynchronousCliAction)parseResult.Action!).Invoke(null!).Should().Be(0);
        handlerWasCalled.Should().BeTrue();
    }
}
