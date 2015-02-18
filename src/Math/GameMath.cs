using OpenTK;

namespace Bearded.Utilities.Math
{
    /// <summary>
    /// Collection of math-related functions.
    /// </summary>
    public static class GameMath
    {
        #region Constants
        /// <summary>
        /// Represents the square root of 2 (1.414213f).
        /// </summary>
        public const float Sqrt2 = 1.414213f;

        /// <summary>
        /// Represents the square root of32 (1.732051f).
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
        #endregion

        #region Float math
        /// <summary>
        /// Returns the cosine of the specified angle.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Cos(float f)
        {
            return (float)System.Math.Cos(f);
        }

        /// <summary>
        /// Returns the sine of the specified angle.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Sin(float f)
        {
            return (float)System.Math.Sin(f);
        }

        /// <summary>
        /// Returns the tangent of the specified angle.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Tan(float f)
        {
            return (float)System.Math.Tan(f);
        }

        /// <summary>
        /// Returns the angle whose cosine is the specified number.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Acos(float f)
        {
            return (float)System.Math.Acos(f);
        }

        /// <summary>
        /// Returns the angle whose sine is the specified number.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Asin(float f)
        {
            return (float)System.Math.Asin(f);
        }

        /// <summary>
        /// Returns the angle whose tangent is the specified number.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Atan(float f)
        {
            return (float)System.Math.Atan(f);
        }

        /// <summary>
        /// Returns the angle whose tangent is the quotient of two specified numbers.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Atan2(float y, float x)
        {
            return (float)System.Math.Atan2(y, x);
        }

        /// <summary>
        /// Returns the square root of the specified number.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Sqrt(float f)
        {
            return (float)System.Math.Sqrt(f);
        }

        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static float Pow(float b, float power)
        {
            return (float)System.Math.Pow(b, power);
        }

        /// <summary>
        /// Returns the lowest integral number higher than or equal to the specified number.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int CeilToInt(double d)
        {
            return (int)System.Math.Ceiling(d);
        }

        /// <summary>
        /// Returns the highest integral number lower than or equal to the specified number.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int FloorToInt(double d)
        {
            return (int)System.Math.Floor(d);
        }

        /// <summary>
        /// Returns the integral number closest to the specified number.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int RoundToInt(double d)
        {
            return (int)System.Math.Round(d);
        }
        #endregion

        #region Intervals
        /// <summary>
        /// Checks if two closed intervals overlap.
        /// </summary>
        /// <param name="amin">The (inclusive) lower bound of the first interval.</param>
        /// <param name="amax">The (inclusive) upper bound of the first interval.</param>
        /// <param name="bmin">The (inclusive) lower bound of the second interval.</param>
        /// <param name="bmax">The (inclusive) upper bound of the second interval.</param>
        /// <returns></returns>
        public static bool IntervalOverlap(double amin, double amax, double bmin, double bmax)
        {
            // Negation of bmin >= amax || amin >= bmax
            return bmin < amax && amin < bmax;
        }
        #endregion

        #region Vector stuff
        /// <summary>
        /// Turns the vector into a three-dimensional vector.
        /// </summary>
        /// <param name="xy">Original vector.</param>
        /// <param name="z">z-coordinate of the new vector.</param>
        /// <returns>A three-dimensional vector.</returns>
        public static Vector3 WithZ(this Vector2 xy, float z = 0)
        {
            return new Vector3(xy.X, xy.Y, z);
        }

        /// <summary>
        /// Turns the vector into a homogenuous vector.
        /// </summary>
        /// <param name="xyz">Original vector.</param>
        /// <param name="w">w-coordinate of the new vector.</param>
        /// <returns>A homogenuous vector.</returns>
        public static Vector4 WithW(this Vector3 xyz, float w)
        {
            return new Vector4(xyz, w);
        }

        /// <summary>
        /// Turns the vector into a homogenuous vector.
        /// </summary>
        /// <param name="xy">Original vector.</param>
        /// <param name="z">z-coordinate of the new vector.</param>
        /// <param name="w">w-coordinate of the new vector.</param>
        /// <returns>A homogenuous vector.</returns>
        public static Vector4 WithZw(this Vector2 xy, float z, float w)
        {
            return new Vector4(xy.X, xy.Y, z, w);
        }
        #endregion
    }
}