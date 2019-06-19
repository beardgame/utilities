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
                var maybe = Maybe<int>.Nothing;

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
                var maybe = Maybe<int>.Nothing;

                maybe.Select(i => i * 2).Should().Be(Maybe<int>.Nothing);
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
                var maybe = Maybe<int>.Nothing;

                maybe.SelectMany(i => Maybe.Just(i * 2)).Should().Be(Maybe<int>.Nothing);
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

                maybe.SelectMany(i => Maybe<int>.Nothing).Should().Be(Maybe<int>.Nothing);
            }
        }

        public sealed class Where
        {
            [Fact]
            public void MapsNothingToNothingIfPredicateReturnsFalse()
            {
                var maybe = Maybe<int>.Nothing;

                maybe.Where(_ => false).Should().Be(Maybe<int>.Nothing);
            }
            
            [Fact]
            public void MapsNothingToNothingIfPredicateReturnsTrue()
            {
                var maybe = Maybe<int>.Nothing;

                maybe.Where(_ => true).Should().Be(Maybe<int>.Nothing);
            }
            
            [Fact]
            public void MapsJustToNothingIfPredicateReturnsFalse()
            {
                var maybe = Maybe.Just(100);

                maybe.Where(_ => false).Should().Be(Maybe<int>.Nothing);
            }
            
            [Fact]
            public void MapsJustToJustIfPredicateReturnsTrue()
            {
                var maybe = Maybe.Just(100);

                maybe.Where(_ => true).Should().Be(Maybe.Just(100));
            }
        }

        public sealed class Match
        {
            [Fact]
            [SuppressMessage("ReSharper", "ArgumentsStyleAnonymousFunction")]
            public void CallsOnNothingOnNothing()
            {
                var maybe = Maybe<int>.Nothing;

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
                Maybe.FromNullable((string) null).Should().Be(Maybe<string>.Nothing);
            }

            [Fact]
            public void ReturnsJustOnReferenceTypeSet()
            {
                Maybe.FromNullable("the game").Should().Be(Maybe.Just("the game"));
            }
        
            [Fact]
            public void ReturnsNothingOnNullableNoValue()
            {
                Maybe.FromNullable((int?) null).Should().Be(Maybe<int>.Nothing);
            }
        
            [Fact]
            public void ReturnsJustOnNullableWithValue()
            {
                Maybe.FromNullable((int?) 10).Should().Be(Maybe.Just(10));
            }
        }
    }
}
