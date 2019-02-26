using System;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Bearded.Utilities.Testing.Tests
{
    public class MaybeAssertionsTests
    {
        [Fact]
        public void BeJust_ForJust_Succeeds()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust();
            
            assertion.Should().NotThrow();
        }
        
        [Fact]
        public void BeJust_ForNothing_Fails()
        {
            var maybe = Maybe.Nothing<int>();

            Action assertion = () => maybe.Should().BeJust();
            
            assertion.Should().Throw<XunitException>();
        }
        
        [Fact]
        public void BeJust_ForJustWithAndConstraint_AndConstraintSucceeds_Succeeds()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust().Which.Should().Be(100);
            
            assertion.Should().NotThrow();
        }
        
        [Fact]
        public void BeJust_ForJustWithAndConstraint_AndConstraintFails_Fails()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust().Which.Should().Be(200);
            
            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void BeJust_ForJustWithValue_ComparedToSameValue_Succeeds()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust(100);
            
            assertion.Should().NotThrow();
        }

        [Fact]
        public void BeJust_ForJustWithValue_ComparedToDifferentValue_Succeeds()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeJust(200);
            
            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void BeJust_ForNothing_ComparedToValue_Fails()
        {
            var maybe = Maybe.Nothing<int>();

            Action assertion = () => maybe.Should().BeJust(200);
            
            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void BeNothing_ForNothing_Succeeds()
        {
            var maybe = Maybe.Nothing<int>();

            Action assertion = () => maybe.Should().BeNothing();
            
            assertion.Should().NotThrow();
        }

        [Fact]
        public void BeNothing_ForJust_Fails()
        {
            var maybe = Maybe.Just(100);

            Action assertion = () => maybe.Should().BeNothing();
            
            assertion.Should().Throw<XunitException>();
        }
    }
}
