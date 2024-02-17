// Copyright (c) .NET Foundation and contributors. All rights reserved.
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
    public void Diagram_directive_parses_as_the_primary_symbol(bool treatUnmatchedTokensAsErrors)
    {
        var diagramDirective = new DiagramDirective();
        var rootCommand = new CliRootCommand { diagramDirective };
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

        result.PrimarySymbol.Should().Be(diagramDirective);
    }

    [Fact]
    public void When_diagram_directive_is_used_the_help_is_not_the_primary_symbol()
    {
        DiagramDirective diagramDirective = new();
        CliRootCommand rootCommand = new()
        {
            diagramDirective
        };

        CliConfiguration config = new(rootCommand)
        {
            Output = new StringWriter(),
        };

        var result = rootCommand.Parse("[diagram] --help", config);

        result.PrimarySymbol.Should().Be(diagramDirective);
    }

    [Fact]
    public void When_diagram_directive_is_used_the_version_is_not_the_primary_symbol()
    {
        DiagramDirective diagramDirective = new();
        CliRootCommand rootCommand = new()
        {
            diagramDirective
        };

        CliConfiguration config = new(rootCommand)
        {
            Output = new StringWriter()
        };

        var result = rootCommand.Parse("[diagram] --version", config);

        result.PrimarySymbol.Should().Be(diagramDirective);
    }
}
