using System;
using FluentAssertions;
using FluentAssertions.Execution;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Tests.Assertions
{
    sealed class Vector3Assertions
    {
        private readonly Vector3 subject;

        public Vector3Assertions(Vector3 subject)
        {
            this.subject = subject;
        }

        [CustomAssertion]
        public AndConstraint<Vector3Assertions> BeApproximately(
            Vector3 expectedValue,
            float precision,
            string because = "",
            params object[] becauseArgs)
        {
            var xDifference = Math.Abs(subject.X - expectedValue.X);
            var yDifference = Math.Abs(subject.Y - expectedValue.Y);
            var zDifference = Math.Abs(subject.Z - expectedValue.Z);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(xDifference <= precision && yDifference <= precision && zDifference <= precision)
                .FailWith(
                    "Expected {context:value} to be approximately {1} +/- {2}{reason}, " +
                    "but {0}'s coordinates differed by {3} (x), {4} (y), and {5} (z).",
                    subject, expectedValue, precision, xDifference, yDifference, zDifference);

            return new AndConstraint<Vector3Assertions>(this);
        }
    }
}
