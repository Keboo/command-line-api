using FluentAssertions;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace System.CommandLine.Extended.Tests
{
    public class VersionOptionTests
    {
        private static readonly string version = (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly())
                                                 .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                                 .InformationalVersion;
        [Fact]
        public async Task Version_option_appears_in_help()
        {
            CliConfiguration configuration = new(new CliRootCommand())
            {
                Output = new StringWriter()
            };

            await configuration.InvokeAsync("--help");

            configuration.Output
                   .ToString()
                   .Should()
                   .Match("*Options:*--version*Show version information*");
        }

        [Fact]
        public async Task When_the_version_option_is_specified_and_there_are_default_options_then_the_version_is_written_to_standard_out()
        {
            var rootCommand = new CliRootCommand
            {
                new CliOption<bool>("-x")
                {
                    DefaultValueFactory = (_) => true
                },
            };
            rootCommand.SetAction((_) => { });

            CliConfiguration configuration = new(rootCommand)
            {
                Output = new StringWriter()
            };

            await configuration.InvokeAsync("--version");

            configuration.Output.ToString().Should().Be($"{version}{NewLine}");
        }

        [Fact]
        public async Task When_the_version_option_is_specified_and_there_are_default_arguments_then_the_version_is_written_to_standard_out()
        {
            CliRootCommand rootCommand = new()
            {
                new CliArgument<bool>("x") { DefaultValueFactory =(_) => true },
            };
            rootCommand.SetAction((_) => { });

            CliConfiguration configuration = new(rootCommand)
            {
                Output = new StringWriter()
            };

            await configuration.InvokeAsync("--version");

            configuration.Output.ToString().Should().Be($"{version}{NewLine}");
        }

        [Fact]
        public async Task When_the_version_option_is_specified_then_invocation_is_short_circuited()
        {
            var wasCalled = false;
            var rootCommand = new CliRootCommand();
            rootCommand.SetAction((_) => wasCalled = true);

            CliConfiguration configuration = new(rootCommand)
            {
                Output = new StringWriter()
            };

            await configuration.InvokeAsync("--version");

            wasCalled.Should().BeFalse();
        }

        [Fact]
        public async Task When_the_version_option_is_specified_then_the_version_is_written_to_standard_out()
        {
            CliConfiguration configuration = new(new CliRootCommand())
            {
                Output = new StringWriter()
            };

            await configuration.InvokeAsync("--version");

            configuration.Output.ToString().Should().Be($"{version}{NewLine}");
        }
    }
}
