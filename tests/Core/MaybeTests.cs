using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Bearded.Utilities.Tests.Core
{
    public class MaybeTests
    {
        [Fact]
        public void ValueOrDefault_ReturnsDefaultOnNothing()
        {
            var maybe = Maybe<int>.Nothing();

            maybe.ValueOrDefault(100).Should().Be(100);
        }

        [Fact]
        public void ValueOrDefault_ReturnsValueOnJust()
        {
            var maybe = Maybe.Just(200);

            maybe.ValueOrDefault(100).Should().Be(200);
        }

        [Fact]
        public void Select_MapsNothingToNothing()
        {
            var maybe = Maybe<int>.Nothing();

            maybe.Select(i => i * 2).Should().Be(Maybe<int>.Nothing());
        }

        [Fact]
        public void Select_MapsValueToJust()
        {
            var maybe = Maybe<int>.Just(100);

            maybe.Select(i => i * 2).Should().Be(Maybe.Just(200));
        }

        [Fact]
        public void SelectMany_MapsNothingToNothing()
        {
            var maybe = Maybe<int>.Nothing();

            maybe.SelectMany(i => Maybe.Just(i * 2)).Should().Be(Maybe<int>.Nothing());
        }

        [Fact]
        public void SelectMany_MapsValueToJust()
        {
            var maybe = Maybe<int>.Just(100);

            maybe.SelectMany(i => Maybe.Just(i * 2)).Should().Be(Maybe.Just(200));
        }

        [Fact]
        public void SelectMany_MapsValueToNothing()
        {
            var maybe = Maybe<int>.Just(100);

            maybe.SelectMany(i => Maybe<int>.Nothing()).Should().Be(Maybe<int>.Nothing());
        }

        [Fact]
        public void Match_CallsOnNothingOnNothing()
        {
            var maybe = Maybe<int>.Nothing();

            maybe.Match(onValue: (val) => throw new XunitException("Wrong method called"), onNothing: () => { });
        }
        
        [Fact]
        public void Match_CallsOnValueWithValueOnJust()
        {
            var maybe = Maybe<int>.Just(100);

            maybe.Match(
                onValue: (val) => val.Should().Be(100),
                onNothing: () => throw new XunitException("Wrong method called"));
        }
    }
}
