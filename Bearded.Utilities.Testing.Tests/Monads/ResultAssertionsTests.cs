using System;
using Bearded.Utilities.Monads;
using Bearded.Utilities.Testing.Monads;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Bearded.Utilities.Testing.Tests.Monads;

public sealed class ResultAssertionsTests
{
    public sealed class BeSuccess
    {
        [Fact]
        public void SucceedsWhenSuccess()
        {
            var result = Result.Success<int, string>(100);

            Action assertion = () => result.Should().BeSuccess();

            assertion.Should().NotThrow();
        }

        [Fact]
        public void FailsWhenFailure()
        {
            var result = Result.Failure<int, string>("something went wrong");

            Action assertion = () => result.Should().BeSuccess();

            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void ReturnsAndConstraintThatSucceedsAsExpected()
        {
            var result = Result.Success<int, string>(100);

            Action assertion = () => result.Should().BeSuccess().And.Should().NotBeNull();

            assertion.Should().NotThrow();
        }

        [Fact]
        public void ReturnsAndConstraintThatFailsAsExpected()
        {
            var result = Result.Success<int, string>(100);

            Action assertion = () => result.Should().BeSuccess().And.Should().BeNull();

            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void FailsWhenFailureEvenIfAndConstraintSucceeds()
        {
            var result = Result.Failure<int, string>("something went wrong");

            Action assertion = () => result.Should().BeSuccess().And.Should().NotBeNull();

            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void ReturnsWhichConstraintThatSucceedsAsExpected()
        {
            var result = Result.Success<int, string>(100);

            Action assertion = () => result.Should().BeSuccess().Which.Should().Be(100);

            assertion.Should().NotThrow();
        }

        [Fact]
        public void ReturnsWhichConstraintThatFailsAsExpected()
        {
            var result = Result.Success<int, string>(200);

            Action assertion = () => result.Should().BeSuccess().Which.Should().Be(100);

            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void FailsWhenFailureEvenIfWhichConstraintSucceeds()
        {
            var result = Result.Failure<int, string>("something went wrong");

            Action assertion = () => result.Should().BeSuccess().Which.Should().Be(100);

            assertion.Should().Throw<XunitException>();
        }
    }

    public sealed class BeSuccessWithResult
    {
        [Fact]
        public void SucceedsWhenSuccessWithSameResult()
        {
            var result = Result.Success<int, string>(100);

            Action assertion = () => result.Should().BeSuccessWithResult(100);

            assertion.Should().NotThrow();
        }

        [Fact]
        public void FailsWhenSuccessWithDifferentResult()
        {
            var result = Result.Success<int, string>(200);

            Action assertion = () => result.Should().BeSuccessWithResult(100);

            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void FailsWhenFailure()
        {
            var result = Result.Failure<int, string>("something went wrong");

            Action assertion = () => result.Should().BeSuccessWithResult(100);

            assertion.Should().Throw<XunitException>();
        }
    }

    public sealed class BeFailure
    {
        [Fact]
        public void SucceedsWhenFailure()
        {
            var result = Result.Failure<int, string>("something went wrong");

            Action assertion = () => result.Should().BeFailure();

            assertion.Should().NotThrow();
        }

        [Fact]
        public void FailsWhenSuccess()
        {
            var result = Result.Success<int, string>(100);

            Action assertion = () => result.Should().BeFailure();

            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void ReturnsAndConstraintThatSucceedsAsExpected()
        {
            var result = Result.Failure<int, string>("something went wrong");

            Action assertion = () => result.Should().BeFailure().And.Should().NotBeNull();

            assertion.Should().NotThrow();
        }

        [Fact]
        public void ReturnsAndConstraintThatFailsAsExpected()
        {
            var result = Result.Failure<int, string>("something went wrong");

            Action assertion = () => result.Should().BeFailure().And.Should().BeNull();

            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void FailsWhenSuccessEvenIfAndConstraintSucceeds()
        {
            var result = Result.Success<int, string>(100);

            Action assertion = () => result.Should().BeFailure().And.Should().NotBeNull();

            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void ReturnsWhichConstraintThatSucceedsAsExpected()
        {
            var result = Result.Failure<int, string>("something went wrong");

            Action assertion = () => result.Should().BeFailure().Which.Should().Be("something went wrong");

            assertion.Should().NotThrow();
        }

        [Fact]
        public void ReturnsWhichConstraintThatFailsAsExpected()
        {
            var result = Result.Failure<int, string>("something else went wrong");

            Action assertion = () => result.Should().BeFailure().Which.Should().Be("something went wrong");

            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void FailsWhenSuccessEvenIfWhichConstraintSucceeds()
        {
            var result = Result.Success<int, string>(100);

            Action assertion = () => result.Should().BeFailure().Which.Should().Be("something went wrong");

            assertion.Should().Throw<XunitException>();
        }
    }

    public sealed class BeFailureWithError
    {
        [Fact]
        public void SucceedsWhenFailureWithSameError()
        {
            var result = Result.Failure<int, string>("something went wrong");

            Action assertion = () => result.Should().BeFailureWithError("something went wrong");

            assertion.Should().NotThrow();
        }

        [Fact]
        public void FailsWhenFailureWithDifferentError()
        {
            var result = Result.Failure<int, string>("something else went wrong");

            Action assertion = () => result.Should().BeFailureWithError("something went wrong");

            assertion.Should().Throw<XunitException>();
        }

        [Fact]
        public void FailsWhenSuccess()
        {
            var result = Result.Success<int, string>(100);

            Action assertion = () => result.Should().BeFailureWithError("something went wrong");

            assertion.Should().Throw<XunitException>();
        }
    }
}
