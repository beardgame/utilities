using System;
using FluentAssertions;
using FluentAssertions.Numeric;
using static System.Single;

namespace Bearded.Utilities.Tests.Helpers
{
    public static class NumericAssertionsExtensions
    {
        public static AndConstraint<NumericAssertions<float>> BeApproximatelyOrBothNaNOrInfinity(
            this NumericAssertions<float> parent,
            float expectedValue,
            int precisionDigits = 5,
            string because = "",
            params object[] becauseArgs)
        {
            if (bothMatch(IsNaN) || bothMatch(IsPositiveInfinity) || bothMatch(IsNegativeInfinity))
                return new AndConstraint<NumericAssertions<float>>(parent);

            return parent.BeApproximately(expectedValue,
                Math.Abs(expectedValue) * (float)Math.Pow(0.1, precisionDigits),
                because, becauseArgs);

            bool bothMatch(Func<float, bool> predicate) => predicate(expectedValue) && predicate((float)parent.Subject);
        }
    }
}