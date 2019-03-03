using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Bearded.Utilities.Tests
{
    public sealed class MaybeTests
    {
        public sealed class ValueOrDefault
        {
            [Fact]
            public void ReturnsDefaultOnNothing()
            {
                var maybe = Maybe.Nothing<int>();

                maybe.ValueOrDefault(100).Should().Be(100);
            }

            [Fact]
            public void ReturnsValueOnJust()
            {
                var maybe = Maybe.Just(200);

                maybe.ValueOrDefault(100).Should().Be(200);
            }
        }

        public sealed class Select
        {
            [Fact]
            public void MapsNothingToNothing()
            {
                var maybe = Maybe.Nothing<int>();

                maybe.Select(i => i * 2).Should().Be(Maybe.Nothing<int>());
            }

            [Fact]
            public void MapsValueToJust()
            {
                var maybe = Maybe.Just(100);

                maybe.Select(i => i * 2).Should().Be(Maybe.Just(200));
            }
        }

        public sealed class SelectMany
        {
            [Fact]
            public void MapsNothingToNothing()
            {
                var maybe = Maybe.Nothing<int>();

                maybe.SelectMany(i => Maybe.Just(i * 2)).Should().Be(Maybe.Nothing<int>());
            }

            [Fact]
            public void MapsValueToJust()
            {
                var maybe = Maybe.Just(100);

                maybe.SelectMany(i => Maybe.Just(i * 2)).Should().Be(Maybe.Just(200));
            }

            [Fact]
            public void MapsValueToNothing()
            {
                var maybe = Maybe.Just(100);

                maybe.SelectMany(i => Maybe.Nothing<int>()).Should().Be(Maybe.Nothing<int>());
            }
        }

        public sealed class Match
        {
            [Fact]
            [SuppressMessage("ReSharper", "ArgumentsStyleAnonymousFunction")]
            public void CallsOnNothingOnNothing()
            {
                var maybe = Maybe.Nothing<int>();

                var isCalled = false;
                maybe.Match(
                    onValue: (val) => throw new XunitException("Wrong method called"),
                    onNothing: () => isCalled = true);

                isCalled.Should().BeTrue("onNothing should have been called");
            }
        
            [Fact]
            [SuppressMessage("ReSharper", "ArgumentsStyleAnonymousFunction")]
            public void CallsOnValueWithValueOnJust()
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
        }

        public sealed class FromNullable
        {
            [Fact]
            public void ReturnsNothingOnReferenceTypeNull()
            {
                Maybe.FromNullable((string) null).Should().Be(Maybe.Nothing<string>());
            }

            [Fact]
            public void ReturnsJustOnReferenceTypeSet()
            {
                Maybe.FromNullable("the game").Should().Be(Maybe.Just("the game"));
            }
        
            [Fact]
            public void ReturnsNothingOnNullableNoValue()
            {
                Maybe.FromNullable((int?) null).Should().Be(Maybe.Nothing<int>());
            }
        
            [Fact]
            public void ReturnsJustOnNullableWithValue()
            {
                Maybe.FromNullable((int?) 10).Should().Be(Maybe.Just(10));
            }
        }
    }
}
