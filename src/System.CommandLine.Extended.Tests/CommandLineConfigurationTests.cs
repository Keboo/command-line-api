// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FluentAssertions;
using Xunit;

namespace System.CommandLine.Tests;

public class CliConfigurationTests
{
    [Fact]
    public void It_can_be_subclassed_to_provide_additional_context()
    {
        var command = new CliRootCommand();
        var commandWasInvoked = false;
        command.SetAction(parseResult =>
        {
            var appConfig = (CustomAppConfiguration)parseResult.Configuration;

            // access custom config

            commandWasInvoked = true;

            return 0;
        });

        var config = new CustomAppConfiguration(command);

        config.Invoke("");

        commandWasInvoked.Should().BeTrue();
    }
}

public class CustomAppConfiguration : CliConfiguration
{
    public CustomAppConfiguration(CliRootCommand command) : base(command)
    {
    }

    public IServiceProvider ServiceProvider { get; }
}