using System;
using Bearded.Utilities.Monads;
using Bearded.Utilities.Testing;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Bearded.Utilities.Tests.Monads;

public sealed class ResultTests
{
    public sealed class ResultOrDefault
    {
        public sealed class WithEagerDefault
        {
            [Fact]
            public void ReturnsDefaultOnFailure()
            {
                var result = Result.Failure<int, string>("something went wrong");

                result.ResultOrDefault(100).Should().Be(100);
            }

            [Fact]
            public void ReturnsResultOnSuccess()
            {
                var result = Result.Success<int, string>(200);

                result.ResultOrDefault(100).Should().Be(200);
            }
        }

        public sealed class WithLazyDefault
        {
            [Fact]
            public void ReturnsDefaultOnFailure()
            {
                var result = Result.Failure<int, string>("something went wrong");

                result.ResultOrDefault(() => 100).Should().Be(100);
            }

            [Fact]
            public void ReturnsResultOnSuccess()
            {
                var result = Result.Success<int, string>(200);

                result.ResultOrDefault(() => 100).Should().Be(200);
            }
        }
    }

    public sealed class ResultOrThrow
    {
        [Fact]
        public void TrowsOnFailure()
        {
            var result = Result.Failure<int, string>("something went wrong");

            Action assertion = () => result.ResultOrThrow(_ => new ApplicationException());

            assertion.Should().Throw<ApplicationException>();
        }

        [Fact]
        public void ReturnsResultOnSuccess()
        {
            var result = Result.Success<int, string>(200);

            result.ResultOrThrow(_ => new ApplicationException()).Should().Be(200);
        }
    }

    public sealed class AsMaybe
    {
        [Fact]
        public void ReturnsNothingOnFailure()
        {
            var result = Result.Failure<int, string>("something went wrong");

            result.AsMaybe().Should().BeNothing();
        }

        [Fact]
        public void ReturnsJustResultOnSuccess()
        {
            var result = Result.Success<int, string>(200);

            result.AsMaybe().Should().BeJust(200);
        }
    }

    public sealed class Select
    {
        [Fact]
        public void MapsFailureToFailure()
        {
            var result = Result.Failure<int, string>("something went wrong");

            result.Select(i => i * 2).Should().Be(Result.Failure<int, string>("something went wrong"));
        }

        [Fact]
        public void MapsSuccessToSuccess()
        {
            var result = Result.Success<int, string>(100);

            result.Select(i => i * 2).Should().Be(Result.Success<int, string>(200));
        }
    }

    public sealed class SelectMany
    {
        [Fact]
        public void MapsFailureToFailure()
        {
            var result = Result.Failure<int, string>("something went wrong");

            result.SelectMany<int>(i => Result.Success(i * 2)).Should().Be(Result.Failure<int, string>("something went wrong"));
        }

        [Fact]
        public void MapsSuccessToSuccess()
        {
            var result = Result.Success<int, string>(100);

            result.SelectMany<int>(i => Result.Success(i * 2)).Should().Be(Result.Success<int, string>(200));
        }

        [Fact]
        public void MapsSuccessToFailure()
        {
            var result = Result.Success<int, string>(100);

            result.SelectMany<int>(i => Result.Failure("something went wrong"))
                .Should().Be(Result.Failure<int, string>("something went wrong"));
        }
    }

    public sealed class Match
    {
        public sealed class WithOneParameter
        {
            [Fact]
            public void DoesNotCallOnSuccessWithFailure()
            {
                var result = Result.Failure<int, string>("something went wrong");

                result.Match(onSuccess: _ => throw new XunitException("Wrong method called"));
            }

            [Fact]
            public void CallsOnSuccessWithResultOnSuccess()
            {
                var result = Result.Success<int, string>(100);

                var isCalled = false;
                result.Match(
                    onSuccess: r =>
                    {
                        r.Should().Be(100);
                        isCalled = true;
                    });

                isCalled.Should().BeTrue("onSuccess should have been called");
            }
        }

        public sealed class WithTwoParameters
        {
            [Fact]
            public void CallsOnFailureWithErrorOnFailure()
            {
                var result = Result.Failure<int, string>("something went wrong");

                var isCalled = false;
                result.Match(
                    onSuccess: r => throw new XunitException("Wrong method called"),
                    onFailure: error =>
                    {
                        error.Should().Be("something went wrong");
                        isCalled = true;
                    });

                isCalled.Should().BeTrue("onFailure should have been called");
            }

            [Fact]
            public void CallsOnSuccessWithResultOnSuccess()
            {
                var result = Result.Success<int, string>(100);

                var isCalled = false;
                result.Match(
                    onSuccess: r =>
                    {
                        r.Should().Be(100);
                        isCalled = true;
                    },
                    onFailure: _ => throw new XunitException("Wrong method called"));

                isCalled.Should().BeTrue("onSuccess should have been called");
            }
        }

        public sealed class Returning
        {
            [Fact]
            public void CallsOnFailureWithErrorOnFailureAndReturnsItsValue()
            {
                var result = Result.Failure<int, string>("something went wrong");
                const string expectedResult = "expected result";

                var actualResult = result.Match(
                    onSuccess: _ => throw new XunitException("Wrong method called"),
                    onFailure: error =>
                    {
                        error.Should().Be("something went wrong");
                        return expectedResult;
                    });

                actualResult.Should().Be(expectedResult, "onFailure should have been called");
            }

            [Fact]
            public void CallsOnSuccessWithResultOnSuccessAndReturnsItsValue()
            {
                var result = Result.Success<int, string>(100);
                const string expectedReturn = "expected result";

                var actualReturn = result.Match(
                    onSuccess: r =>
                    {
                        r.Should().Be(100);
                        return expectedReturn;
                    },
                    onFailure: _ => throw new XunitException("Wrong method called"));

                actualReturn.Should().Be(expectedReturn, "onSuccess should have been called");
            }
        }
    }
}
