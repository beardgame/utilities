using FluentAssertions;
using Xunit;

namespace Bearded.Utilities.Tests;

public sealed class ResettableLazyTests
{
    [Fact]
    public void ValueReturnsReferenceTypeFromFunc()
    {
        var obj = new object();
        var lazy = ResettableLazy.From(() => obj);

        lazy.Value.Should().Be(obj);
    }

    [Fact]
    public void ValueReturnsNullFromFunc()
    {
        var lazy = ResettableLazy.From(() => (object?) null);

        lazy.Value.Should().Be(null);
    }

    [Fact]
    public void ValueReturnsValueTypeFromFunc()
    {
        const int num = 5;
        var lazy = ResettableLazy.From(() => num);

        lazy.Value.Should().Be(num);
    }

    [Fact]
    public void ValueReturnsSameInstanceEveryTime()
    {
        var lazy = ResettableLazy.From(() => new object());

        var value1 = lazy.Value;
        var value2 = lazy.Value;

        value1.Should().BeSameAs(value2);
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
        var lazy = ResettableLazy.From(() => new object());
        var valueBeforeResetting = lazy.Value;
        lazy.Reset();

        lazy.Value.Should().NotBeSameAs(valueBeforeResetting);
    }
}
