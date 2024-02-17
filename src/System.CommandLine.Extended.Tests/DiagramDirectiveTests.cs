﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace System.CommandLine.Tests;

public class DiagramDirectiveTests
{
    private readonly ITestOutputHelper output;

    public DiagramDirectiveTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Diagram_directive_writes_parse_diagram(bool treatUnmatchedTokensAsErrors)
    {
        var rootCommand = new CliRootCommand { new DiagramDirective() };
        var subcommand = new CliCommand("subcommand");
        rootCommand.Subcommands.Add(subcommand);
        var option = new CliOption<int>("-c", "--count");
        subcommand.Options.Add(option);
        subcommand.TreatUnmatchedTokensAsErrors = treatUnmatchedTokensAsErrors;

        CliConfiguration config = new(rootCommand)
        {
            Output = new StringWriter()
        };

        var result = rootCommand.Parse("[diagram] subcommand -c 34 --nonexistent wat", config);

        string diagram = result.Diagram();
        output.WriteLine(diagram);

        string expected = treatUnmatchedTokensAsErrors
                              ? $"[ {CliRootCommand.ExecutableName} ![ subcommand [ -c <34> ] ] ]   ???--> --nonexistent wat" + Environment.NewLine
                              : $"[ {CliRootCommand.ExecutableName} [ subcommand [ -c <34> ] ] ]   ???--> --nonexistent wat" + Environment.NewLine;

        diagram.Should().Be(expected);
    }

    [Fact]
    public async Task When_diagram_directive_is_used_the_help_is_not_displayed()
    {
        CliRootCommand rootCommand = new()
        {
            new DiagramDirective()
        };

        CliConfiguration config = new(rootCommand)
        {
            Output = new StringWriter(),
        };

        var result = rootCommand.Parse("[diagram] --help", config);

        output.WriteLine(result.Diagram());

        //await result.InvokeAsync();

        config.Output
               .ToString()
               .Should()
               .Be($"[ {CliRootCommand.ExecutableName} [ --help ] ]" + Environment.NewLine);
    }

    [Fact]
    public async Task When_diagram_directive_is_used_the_version_is_not_displayed()
    {
        CliRootCommand rootCommand = new()
        {
            new DiagramDirective()
        };

        CliConfiguration config = new(rootCommand)
        {
            Output = new StringWriter()
        };

        var result = rootCommand.Parse("[diagram] --version", config);

        output.WriteLine(result.Diagram());

        //await result.InvokeAsync();

        config.Output
              .ToString()
              .Should()
              .Be($"[ {CliRootCommand.ExecutableName} [ --version ] ]" + Environment.NewLine);
    }

    [Fact]
    public async Task When_there_are_no_errors_then_diagram_directive_sets_exit_code_0()
    {
        CliRootCommand command = new ()
        {
            new CliOption<int>("-x"),
            new DiagramDirective()
        };

        CliConfiguration config = new(command)
        {
            Output = new StringWriter(),
        };

        //var exitCode = await command.Parse("[diagram] -x 123", config).InvokeAsync();
        Assert.True(false, "Uncomment Below Code");
        //exitCode.Should().Be(0);
    }

    [Fact]
    public async Task When_there_are_errors_then_diagram_directive_sets_exit_code_1()
    {
        CliRootCommand command = new()
        {
            new CliOption<int>("-x"),
            new DiagramDirective()
        };

        CliConfiguration config = new(command)
        {
            Output = new StringWriter(),
        };

        //var exitCode = await command.Parse("[diagram] -x not-an-int", config).InvokeAsync();
        Assert.True(false, "Uncomment Below Code");
        //exitCode.Should().Be(1);
    }

    [Fact]
    public async Task When_there_are_errors_then_diagram_directive_sets_exit_code_to_custom_value()
    {
        CliRootCommand command = new()
        {
            new CliOption<int>("-x"),
            new DiagramDirective
            {
                ParseErrorReturnValue = 42
            }
        };

        CliConfiguration config = new(command)
        {
            Output = new StringWriter()
        };

        //int exitCode = await config.InvokeAsync("[diagram] -x not-an-int");
        Assert.True(false, "Uncomment Below Code");
        //exitCode.Should().Be(42);
    }
}
