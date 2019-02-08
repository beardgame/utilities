using System;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Bearded.Utilities.Testing.Tests
{
    public class MaybeAssertionsTests
    {
        [Fact]
        public void BeJust_SucceedsWhenMaybeHasValue()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust();
            assertion.Should().NotThrow();
        }
        
        [Fact]
        public void BeJust_FailsWhenMaybeIsNothing()
        {
            var maybe = Maybe.Nothing<int>();

            Action assertion = () => maybe.Should().BeJust();
            assertion.Should().Throw<XunitException>();
        }
        
        [Fact]
        public void BeJust_SucceedsWithAndConstraint()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust().Which.Should().Be(100);
            assertion.Should().NotThrow();
        }
        
        [Fact]
        public void BeJust_FailsWhenAndConstraintIsNotMet()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust().Which.Should().Be(200);
            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void BeJust_SucceedsWhenComparingToMaybeWithRightValue()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust(100);
            assertion.Should().NotThrow();
        }

        [Fact]
        public void BeJust_FailsWhenComparingToMaybeWithWrongValue()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust(200);
            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void BeJust_FailsWhenComparingToNothing()
        {
            var maybe = Maybe.Nothing<int>();

            Action assertion = () => maybe.Should().BeJust(200);
            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void BeNothing_SucceedsWhenMaybeIsNothing()
        {
            var maybe = Maybe.Nothing<int>();

            Action assertion = () => maybe.Should().BeNothing();
            assertion.Should().NotThrow();
        }

        [Fact]
        public void BeNothing_FailsWhenMaybeHasValue()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeNothing();
            assertion.Should().Throw<XunitException>();
        }
    }
}
