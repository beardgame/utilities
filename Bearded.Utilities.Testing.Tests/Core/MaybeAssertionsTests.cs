using System;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Bearded.Utilities.Testing.Tests;

public sealed class MaybeAssertionsTests
{
    public sealed class BeJustWithNoParameter
    {
        [Fact]
        public void SucceedsWhenJust()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust();

            assertion.Should().NotThrow();
        }

        [Fact]
        public void FailsWhenNothing()
        {
            var maybe = Maybe<int>.Nothing;

            Action assertion = () => maybe.Should().BeJust();

            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void ReturnsAndConstraintThatSucceedsAsExpected()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust().And.Should().NotBeNull();

            assertion.Should().NotThrow();
        }

        [Fact]
        public void ReturnsAndConstraintThatFailsAsExpected()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust().And.Should().BeNull();

            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void FailsWhenNothingEvenIfAndConstraintSucceeds()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust().And.Should().NotBeNull();

            assertion.Should().NotThrow();
        }

        [Fact]
        public void ReturnsWhichConstraintThatSucceedsAsExpected()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust().Which.Should().Be(100);

            assertion.Should().NotThrow();
        }

        [Fact]
        public void ReturnsWhichConstraintThatFailsAsExpected()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust().Which.Should().Be(200);

            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void FailsWhenNothingEvenIfWhichConstraintSucceeds()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust().Which.Should().Be(100);

            assertion.Should().NotThrow();
        }
    }

    public sealed class BeJustWithValue
    {
        [Fact]
        public void SucceedsWhenJustComparedToSameValue()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust(100);

            assertion.Should().NotThrow();
        }

        [Fact]
        public void FailsWhenJustComparedToDifferentValue()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust(200);

            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void FailsWhenNothing()
        {
            var maybe = Maybe<int>.Nothing;

            Action assertion = () => maybe.Should().BeJust(200);

            assertion.Should().Throw<XunitException>();
        }
    }

    public sealed class BeNothing
    {
        [Fact]
        public void SucceedsWhenNothing()
        {
            var maybe = Maybe<int>.Nothing;

            Action assertion = () => maybe.Should().BeNothing();

            assertion.Should().NotThrow();
        }

        [Fact]
        public void FailsWhenJust()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeNothing();

            assertion.Should().Throw<XunitException>();
        }
    }
}
