using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Bearded.Utilities.Tests
{
    public class MaybeTests
    {
        [Fact]
        public void ValueOrDefault_ReturnsDefaultOnNothing()
        {
            var maybe = Maybe.Nothing<int>();

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
            var maybe = Maybe.Nothing<int>();

            maybe.Select(i => i * 2).Should().Be(Maybe.Nothing<int>());
        }

        [Fact]
        public void Select_MapsValueToJust()
        {
            var maybe = Maybe.Just(100);

            maybe.Select(i => i * 2).Should().Be(Maybe.Just(200));
        }

        [Fact]
        public void SelectMany_MapsNothingToNothing()
        {
            var maybe = Maybe.Nothing<int>();

            maybe.SelectMany(i => Maybe.Just(i * 2)).Should().Be(Maybe.Nothing<int>());
        }

        [Fact]
        public void SelectMany_MapsValueToJust()
        {
            var maybe = Maybe.Just(100);

            maybe.SelectMany(i => Maybe.Just(i * 2)).Should().Be(Maybe.Just(200));
        }

        [Fact]
        public void SelectMany_MapsValueToNothing()
        {
            var maybe = Maybe.Just(100);

            maybe.SelectMany(i => Maybe.Nothing<int>()).Should().Be(Maybe.Nothing<int>());
        }

        [Fact]
        public void Match_CallsOnNothingOnNothing()
        {
            var maybe = Maybe.Nothing<int>();

            var isCalled = false;
            maybe.Match(
                onValue: (val) => throw new XunitException("Wrong method called"),
                onNothing: () => isCalled = true);

            isCalled.Should().BeTrue("onNothing should have been called");
        }
        
        [Fact]
        public void Match_CallsOnValueWithValueOnJust()
        {
            var maybe = Maybe.Just(100);

            var isCalled = false;
            maybe.Match(
                onValue: (val) =>
                {
                    val.Should().Be(100);
                    isCalled = true;
                },
                onNothing: () => throw new XunitException("Wrong method called"));
            
            isCalled.Should().BeTrue("onValue should have been called");
        }

        [Fact]
        public void FromNullable_OnReferenceTypeNull_ReturnsNothing()
        {
            Maybe.FromNullable((string) null).Should().Be(Maybe.Nothing<string>());
        }

        [Fact]
        public void FromNullable_OnReferenceTypeSet_ReturnsJust()
        {
            Maybe.FromNullable("the game").Should().Be(Maybe.Just("the game"));
        }
        
        [Fact]
        public void FromNullable_OnNullableNoValue_ReturnsNothing()
        {
            Maybe.FromNullable((int?) null).Should().Be(Maybe.Nothing<int>());
        }
        
        [Fact]
        public void FromNullable_OnNullableWithValue_ReturnsJust()
        {
            Maybe.FromNullable((int?) 10).Should().Be(Maybe.Just(10));
        }
    }
}
