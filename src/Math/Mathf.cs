
namespace Bearded.Utilities.Math
{
    /// <summary>
    /// Collection of math-related functions.
    /// </summary>
    public static class Mathf
    {
        /// <summary>
        /// Represents the square root of 2 (1.414213f).
        /// </summary>
        public const float Sqrt2 = 1.414213f;

        /// <summary>
        /// Represents the square root of 3 (1.732051f).
        /// </summary>
        public const float Sqrt3 = 1.732051f;

        /// <summary>
        /// Represents the value of pi (3.14159274).
        /// </summary>
        public const float Pi = (float)System.Math.PI;

        /// <summary>
        /// Represents the value of pi divided by two (1.57079637).
        /// </summary>
        public const float PiOver2 = Pi / 2.0f;

        /// <summary>
        /// Represents the value of pi divided by four (0.7853982).
        /// </summary>
        public const float PiOver4 = Pi / 4.0f;

        /// <summary>
        /// Represents the value of pi times two (6.28318548).
        /// </summary>
        public const float TwoPi = 2 * Pi;

        /// <summary>
        /// Represents the value of tau (6.28318548).
        /// </summary>
        public const float Tau = 2 * Pi;
        
        private const float degreesToRadians = TwoPi / 360f;
        private const float radiansToDegrees = 360f / TwoPi;
        
        /// <summary>
        /// Returns the cosine of the specified angle.
        /// </summary>
        public static float Cos(float f) => (float)System.Math.Cos(f);

        /// <summary>
        /// Returns the sine of the specified angle.
        /// </summary>
        public static float Sin(float f) => (float)System.Math.Sin(f);

        /// <summary>
        /// Returns the tangent of the specified angle.
        /// </summary>
        public static float Tan(float f) => (float)System.Math.Tan(f);

        /// <summary>
        /// Returns the angle whose cosine is the specified number.
        /// </summary>
        public static float Acos(float f) => (float)System.Math.Acos(f);

        /// <summary>
        /// Returns the angle whose sine is the specified number.
        /// </summary>
        public static float Asin(float f) => (float)System.Math.Asin(f);

        /// <summary>
        /// Returns the angle whose tangent is the specified number.
        /// </summary>
        public static float Atan(float f) => (float)System.Math.Atan(f);

        /// <summary>
        /// Returns the angle whose tangent is the quotient of two specified numbers.
        /// </summary>
        public static float Atan2(float y, float x) => (float)System.Math.Atan2(y, x);

        /// <summary>
        /// Returns the square root of the specified number.
        /// </summary>
        public static float Sqrt(float f) => (float)System.Math.Sqrt(f);

        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// </summary>
        public static float Pow(float b, float power) => (float)System.Math.Pow(b, power);

        /// <summary>
        /// Returns the lowest integral number higher than or equal to the specified number.
        /// </summary>
        public static int CeilToInt(double d) => (int)System.Math.Ceiling(d);

        /// <summary>
        /// Returns the highest integral number lower than or equal to the specified number.
        /// </summary>
        public static int FloorToInt(double d) => (int)System.Math.Floor(d);

        /// <summary>
        /// Returns the integral number closest to the specified number.
        /// </summary>
        public static int RoundToInt(double d) => (int)System.Math.Round(d);

        /// <summary>
        /// Converts an angle in radians to degrees.
        /// </summary>
        public static float RadiansToDegrees(float radians) => radians * radiansToDegrees;

        /// <summary>
        /// Converts an angle in degrees to radians.
        /// </summary>
        public static float DegreesToRadians(float degrees) => degrees * degreesToRadians;
    }
}
