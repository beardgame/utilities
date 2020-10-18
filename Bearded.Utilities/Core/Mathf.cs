
using System;

namespace Bearded.Utilities
{
    /// <summary>
    /// Collection of math-related functions.
    /// </summary>
    [Obsolete]
    public static class Mathf
    {
        /// <summary>
        /// Represents the square root of 2 (1.414213f).
        /// </summary>
        [Obsolete("Use MathConstants instead")]
        public const float Sqrt2 = MathConstants.Sqrt2;

        /// <summary>
        /// Represents the square root of 3 (1.732051f).
        /// </summary>
        [Obsolete("Use MathConstants instead")]
        public const float Sqrt3 = MathConstants.Sqrt3;

        /// <summary>
        /// Represents the value of pi (3.14159274).
        /// </summary>
        [Obsolete("Use MathConstants instead")]
        public const float Pi = MathConstants.Pi;

        /// <summary>
        /// Represents the value of pi divided by two (1.57079637).
        /// </summary>
        [Obsolete("Use MathConstants instead")]
        public const float PiOver2 = MathConstants.PiOver2;

        /// <summary>
        /// Represents the value of pi divided by four (0.7853982).
        /// </summary>
        [Obsolete("Use MathConstants instead")]
        public const float PiOver4 = MathConstants.PiOver4;

        /// <summary>
        /// Represents the value of pi times two (6.28318548).
        /// </summary>
        [Obsolete("Use MathConstants instead")]
        public const float TwoPi = MathConstants.TwoPi;

        /// <summary>
        /// Represents the value of tau (6.28318548).
        /// </summary>
        [Obsolete("Use MathConstants instead")]
        public const float Tau = MathConstants.Tau;

        /// <summary>
        /// Returns the cosine of the specified angle.
        /// </summary>
        [Obsolete("Use System.MathF instead")]
        public static float Cos(float f) => MathF.Cos(f);

        /// <summary>
        /// Returns the sine of the specified angle.
        /// </summary>
        [Obsolete("Use System.MathF instead")]
        public static float Sin(float f) => MathF.Sin(f);

        /// <summary>
        /// Returns the tangent of the specified angle.
        /// </summary>
        [Obsolete("Use System.MathF instead")]
        public static float Tan(float f) => MathF.Tan(f);

        /// <summary>
        /// Returns the angle whose cosine is the specified number.
        /// </summary>
        [Obsolete("Use System.MathF instead")]
        public static float Acos(float f) => MathF.Acos(f);

        /// <summary>
        /// Returns the angle whose sine is the specified number.
        /// </summary>
        [Obsolete("Use System.MathF instead")]
        public static float Asin(float f) => MathF.Asin(f);

        /// <summary>
        /// Returns the angle whose tangent is the specified number.
        /// </summary>
        [Obsolete("Use System.MathF instead")]
        public static float Atan(float f) => MathF.Atan(f);

        /// <summary>
        /// Returns the angle whose tangent is the quotient of two specified numbers.
        /// </summary>
        [Obsolete("Use System.MathF instead")]
        public static float Atan2(float y, float x) => MathF.Atan2(y, x);

        /// <summary>
        /// Returns the square root of the specified number.
        /// </summary>
        [Obsolete("Use System.MathF instead")]
        public static float Sqrt(float f) => MathF.Sqrt(f);

        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// </summary>
        [Obsolete("Use System.MathF instead")]
        public static float Pow(float b, float power) => MathF.Pow(b, power);

        /// <summary>
        /// Returns the lowest integral number higher than or equal to the specified number.
        /// </summary>
        [Obsolete("Use MoreMath instead")]
        public static int CeilToInt(double d) => MoreMath.CeilToInt(d);

        /// <summary>
        /// Returns the highest integral number lower than or equal to the specified number.
        /// </summary>
        [Obsolete("Use MoreMath instead")]
        public static int FloorToInt(double d) => MoreMath.FloorToInt(d);

        /// <summary>
        /// Returns the integral number closest to the specified number.
        /// </summary>
        [Obsolete("Use MoreMath instead")]
        public static int RoundToInt(double d) => MoreMath.RoundToInt(d);

        /// <summary>
        /// Converts an angle in radians to degrees.
        /// </summary>
        [Obsolete("Use MoreMath instead")]
        public static float RadiansToDegrees(float radians) => MoreMath.RadiansToDegrees(radians);

        /// <summary>
        /// Converts an angle in degrees to radians.
        /// </summary>
        [Obsolete("Use MoreMath instead")]
        public static float DegreesToRadians(float degrees) => MoreMath.DegreesToRadians(degrees);
    }
}
