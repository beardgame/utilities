using System.Diagnostics.CodeAnalysis;
using Bearded.Utilities.Testing.Monads;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Bearded.Utilities.Tests;

[SuppressMessage("ReSharper", "ArgumentsStyleAnonymousFunction")]
public sealed class MaybeTests
{
    public sealed class ValueOrDefault
    {
        public sealed class WithEagerDefault
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

        public sealed class WithLazyDefault
        {
            [Fact]
            public void ReturnsDefaultOnNothing()
            {
                var maybe = Maybe<int>.Nothing;

                maybe.ValueOrDefault(() => 100).Should().Be(100);
            }

            [Fact]
            public void ReturnsValueOnJust()
            {
                var maybe = Maybe.Just(200);

                maybe.ValueOrDefault(() => 100).Should().Be(200);
            }
        }
    }

    public sealed class ValueOrError
    {
        public sealed class WithEagerError
        {
            [Fact]
            public void ReturnsFailureOnNothing()
            {
                var maybe = Maybe<int>.Nothing;

                maybe.ValueOrFailure("something went wrong").Should().BeFailureWithError("something went wrong");
            }

            [Fact]
            public void ReturnsSuccessOnJust()
            {
                var maybe = Maybe.Just(200);

                maybe.ValueOrFailure("something went wrong").Should().BeSuccessWithResult(200);
            }
        }

        public sealed class WithLazyError
        {
            [Fact]
            public void ReturnsFailureOnNothing()
            {
                var maybe = Maybe<int>.Nothing;

                maybe.ValueOrFailure(() => "something went wrong").Should()
                    .BeFailureWithError("something went wrong");
            }

            [Fact]
            public void ReturnsSuccessOnJust()
            {
                var maybe = Maybe.Just(200);

                maybe.ValueOrFailure(() => "something went wrong").Should().BeSuccessWithResult(200);
            }
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

    public sealed class MatchWithOneParameter
    {
        [Fact]
        public void DoesNotCallOnValueWithNothing()
        {
            var maybe = Maybe<int>.Nothing;

            maybe.Match(onValue: (val) => throw new XunitException("Wrong method called"));
        }

        [Fact]
        public void CallsOnValueWithValueOnJust()
        {
            var maybe = Maybe.Just(100);

            var isCalled = false;
            maybe.Match(
                onValue: (val) =>
                {
                    val.Should().Be(100);
                    isCalled = true;
                });

            isCalled.Should().BeTrue("onValue should have been called");
        }
    }

    public sealed class MatchWithTwoParameters
    {
        [Fact]
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

    public sealed class ReturningMatch
    {
        [Fact]
        public void CallsOnNothingOnNothingAndReturnsItsValue()
        {
            var maybe = Maybe<int>.Nothing;
            var expectedResult = "expected result";

            var actualResult = maybe.Match(
                onValue: (val) => throw new XunitException("Wrong method called"),
                onNothing: () => expectedResult);

            actualResult.Should().Be(expectedResult, "onNothing should have been called");
        }

        [Fact]
        public void CallsOnValueWithValueOnJustAndReturnsItsValue()
        {
            var maybe = Maybe.Just(100);
            var expectedResult = "expected result";

            var actualResult = maybe.Match(
                onValue: (val) =>
                {
                    val.Should().Be(100);
                    return expectedResult;
                },
                onNothing: () => throw new XunitException("Wrong method called"));

            actualResult.Should().Be(expectedResult, "onValue should have been called");
        }
    }

    public sealed class FromNullable
    {
        [Fact]
        public void ReturnsNothingOnReferenceTypeNull()
        {
            Maybe.FromNullable((string?) null).Should().Be(Maybe<string>.Nothing);
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
