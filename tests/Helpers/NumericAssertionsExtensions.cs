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
            if(bothMatch(IsNaN) || bothMatch(IsPositiveInfinity) || bothMatch(IsNegativeInfinity))
                // this will always be true and is only used to get the correct return type
                return parent.NotBe(0); 

            return parent.BeApproximately(expectedValue,
                Math.Abs(expectedValue) * (float)Math.Pow(0.1, precisionDigits),
                because, becauseArgs);

            bool bothMatch(Func<float, bool> predicate) => predicate(expectedValue) && predicate((float)parent.Subject);
        }
    }
}