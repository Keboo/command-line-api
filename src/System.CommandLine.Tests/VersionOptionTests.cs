// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using System.IO;
using System.Linq;
using Xunit;

namespace System.CommandLine.Tests;

public class VersionOptionTests
{
    [Fact]
    public void When_the_version_option_is_specified_then_the_version_is_parsed()
    {
        CliConfiguration configuration = new(new CliRootCommand());

        ParseResult parseResult = configuration.Parse("--version");

        parseResult.Errors.Should().BeEmpty();
        parseResult.GetValue(configuration.RootCommand.Options.OfType<VersionOption>().Single()).Should().BeTrue();
    }

    [Theory]
    [InlineData("--version -x")]
    [InlineData("--version subcommand")]
    public void Version_is_not_valid_with_other_tokens(string commandLine)
    {
        var subcommand = new CliCommand("subcommand");
        var rootCommand = new CliRootCommand
        {
            subcommand,
            new CliOption<bool>("-x")
        };

        CliConfiguration configuration = new(rootCommand)
        {
            Output = new StringWriter()
        };

        var result = rootCommand.Parse(commandLine, configuration);

        result.Errors.Should().Contain(e => e.Message == "--version option cannot be combined with other arguments.");
    }
    
    [Fact]
    public void Version_option_is_not_added_to_subcommands()
    {
        var childCommand = new CliCommand("subcommand");

        var rootCommand = new CliRootCommand
        {
            childCommand
        };

        CliConfiguration configuration = new(rootCommand)
        {
            Output = new StringWriter()
        };

        configuration
              .RootCommand
              .Subcommands
              .Single(c => c.Name == "subcommand")
              .Options
              .Should()
              .BeEmpty();
    }

    [Fact]
    public void Version_can_specify_additional_alias()
    {
        CliRootCommand rootCommand = new();

        VersionOption versionOption = new("-version", "-v");
        for (int i = 0; i < rootCommand.Options.Count; i++)
        {
            if (rootCommand.Options[i] is VersionOption)
                rootCommand.Options[i] = versionOption;
        }

        var parseResult = rootCommand.Parse("-version");
        var versionSpecified = parseResult.GetValue(versionOption);
        versionSpecified.Should().BeTrue();

        parseResult = rootCommand.Parse("-v");
        versionSpecified = parseResult.GetValue(versionOption);
        versionSpecified.Should().BeTrue();
    }

    [Fact]
    public void Version_is_not_valid_with_other_tokens_uses_custom_alias()
    {
        var childCommand = new CliCommand("subcommand");
        var rootCommand = new CliRootCommand
        {
            childCommand
        };

        rootCommand.Options[0] = new VersionOption("-v");

        CliConfiguration configuration = new(rootCommand)
        {
            Output = new StringWriter()
        };

        var result = rootCommand.Parse("-v subcommand", configuration);

        result.Errors.Should().ContainSingle(e => e.Message == "-v option cannot be combined with other arguments.");
    }
}
