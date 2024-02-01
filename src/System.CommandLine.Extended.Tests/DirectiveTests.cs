using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace System.CommandLine.Extended.Tests
{
    public class DirectiveTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Multiple_instances_of_the_same_directive_can_be_invoked(bool invokeAsync)
        {
            var commandActionWasCalled = false;
            var directiveCallCount = 0;

            Action<ParseResult> incrementCallCount = _ => directiveCallCount++;
            Action<ParseResult> verifyActionWasCalled = _ => commandActionWasCalled = true;

            var testDirective = new TestDirective("test")
            {
                Action = invokeAsync
                             ? new AsynchronousTestAction(incrementCallCount, terminating: false)
                             : new SynchronousTestAction(incrementCallCount, terminating: false)
            };

            var config = new CliConfiguration(new CliRootCommand
            {
                Action = invokeAsync
                             ? new AsynchronousTestAction(verifyActionWasCalled, terminating: false)
                             : new SynchronousTestAction(verifyActionWasCalled, terminating: false),
                Directives = { testDirective }
            });

            if (invokeAsync)
            {
                await config.InvokeAsync("[test:1] [test:2]");
            }
            else
            {
                config.Invoke("[test:1] [test:2]");
            }

            using var _ = new AssertionScope();

            commandActionWasCalled.Should().BeTrue();
            directiveCallCount.Should().Be(2);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Multiple_different_directives_can_be_invoked(bool invokeAsync)
        {
            bool commandActionWasCalled = false;
            bool directiveOneActionWasCalled = false;
            bool directiveTwoActionWasCalled = false;

            var directiveOne = new TestDirective("one")
            {
                Action = new SynchronousTestAction(_ => directiveOneActionWasCalled = true, terminating: false)
            };
            var directiveTwo = new TestDirective("two")
            {
                Action = new SynchronousTestAction(_ => directiveTwoActionWasCalled = true, terminating: false)
            };
            var config = new CliConfiguration(new CliRootCommand
            {
                Action = new SynchronousTestAction(_ => commandActionWasCalled = true, terminating: false), Directives = { directiveOne, directiveTwo }
            });

            if (invokeAsync)
            {
                await config.InvokeAsync("[one] [two]");
            }
            else
            {
                config.Invoke("[one] [two]");
            }

            using var _ = new AssertionScope();

            commandActionWasCalled.Should().BeTrue();
            directiveOneActionWasCalled.Should().BeTrue();
            directiveTwoActionWasCalled.Should().BeTrue();
        }

        public class TestDirective : CliDirective
        {
            public TestDirective(string name) : base(name)
            {
            }
        }
    }
}
