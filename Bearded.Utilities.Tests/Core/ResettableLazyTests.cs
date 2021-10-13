using System;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace Bearded.Utilities.Tests
{
    public sealed class ResettableLazyTests
    {
        [Property]
        public void ValueReturnsValueReturnedFromFunc(object obj)
        {
            var lazy = ResettableLazy.From(() => obj);

            lazy.Value.Should().Be(obj);
        }

        [Property]
        public void ValueReturnsSameInstanceEveryTime(Func<object> objProvider)
        {
            var lazy = ResettableLazy.From(objProvider);

            var value1 = lazy.Value;
            var value2 = lazy.Value;

            value1.Should().BeSameAs(value2);
        }

        [Fact]
        public void ValueReturnsResultFromFunc()
        {
            var obj = new object();
            var lazy = ResettableLazy.From(() => obj);

            lazy.Value.Should().BeSameAs(obj);
        }

        [Fact]
        public void FuncShouldNotBeCalledIfValueIsNotCalled()
        {
            var funcWasCalled = false;
            ResettableLazy.From(() =>
            {
                funcWasCalled = true;
                return new object();
            });

            funcWasCalled.Should().BeFalse();
        }

        [Fact]
        public void ValueReturnsNewInstanceAfterResetting()
        {
            // Use our own func to ensure that we create new instances each time the func is called.
            var lazy = ResettableLazy.From(() => new object());
            var valueBeforeResetting = lazy.Value;
            lazy.Reset();

            lazy.Value.Should().NotBeSameAs(valueBeforeResetting);
        }
    }
}
