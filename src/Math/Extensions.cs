using System;
using OpenTK;

namespace Bearded.Utilities.Math
{
    public static class Extensions
    {
        #region Clamped
        /// <summary>
        /// Clamps the value to a specified range.
        /// </summary>
        public static int Clamped(this int value, int min, int max)
        {
            if (value <= min)
                return min;
            if (value >= max)
                return max;
            return value;
        }

        /// <summary>
        /// Clamps the value to a specified range.
        /// </summary>
        public static float Clamped(this float value, float min, float max)
        {
            if (value <= min)
                return min;
            if (value >= max)
                return max;
            return value;
        }

        /// <summary>
        /// Clamps the value to a specified range.
        /// </summary>
        public static double Clamped(this double value, double min, double max)
        {
            if (value <= min)
                return min;
            if (value >= max)
                return max;
            return value;
        }
        #endregion

        #region Modulo
        /// <summary>
        /// Gives the number projected to Zn.
        /// </summary>
        public static int ModuloN(this int a, int n) => ((a % n) + n) % n;

        #endregion

        #region Powers
        /// <summary>
        /// Squares an integer.
        /// </summary>
        public static int Squared(this int i) => i * i;

        /// <summary>
        /// Squares a float.
        /// </summary>
        public static float Squared(this float f) => f * f;

        /// <summary>
        /// Squares a double.
        /// </summary>
        public static double Squared(this double d) => d * d;


        /// <summary>
        /// Cubes an integer.
        /// </summary>
        public static int Cubed(this int i) => i * i * i;

        /// <summary>
        /// Cubes a float.
        /// </summary>
        public static float Cubed(this float f) => f * f * f;

        /// <summary>
        /// Cubes a double.
        /// </summary>
        public static double Cubed(this double d) => d * d * d;


        /// <summary>
        /// Returns the square root of the specified number.
        /// </summary>
        public static float Sqrted(this float f) => (float)System.Math.Sqrt(f);

        /// <summary>
        /// Returns the square root of the specified number.
        /// </summary>
        public static double Sqrted(this double d) => System.Math.Sqrt(d);


        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// </summary>
        public static float Powed(this float b, float power) => (float)System.Math.Pow(b, power);

        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// </summary>
        public static double Powed(this double b, double power) => System.Math.Pow(b, power);

        #endregion

        #region Trigonomitry

        #region Float
        /// <summary>
        /// Returns the cosine of the specified angle.
        /// </summary>
        public static float Cos(this float f) => (float)System.Math.Cos(f);

        /// <summary>
        /// Returns the sine of the specified angle.
        /// </summary>
        public static float Sin(this float f) => (float)System.Math.Sin(f);

        /// <summary>
        /// Returns the tangent of the specified angle.
        /// </summary>
        public static float Tan(this float f) => (float)System.Math.Tan(f);

        /// <summary>
        /// Returns the angle whose cosine is the specified number.
        /// </summary>
        public static float Acos(this float f) => (float)System.Math.Acos(f);

        /// <summary>
        /// Returns the angle whose sine is the specified number.
        /// </summary>
        public static float Asin(this float f) => (float)System.Math.Asin(f);

        /// <summary>
        /// Returns the angle whose tangent is the specified number.
        /// </summary>
        public static float Atan(this float f) => (float)System.Math.Atan(f);

        #endregion

        #region Double
        /// <summary>
        /// Returns the cosine of the specified angle.
        /// </summary>
        public static double Cos(this double d) => System.Math.Cos(d);

        /// <summary>
        /// Returns the sine of the specified angle.
        /// </summary>
        public static double Sin(this double d) => System.Math.Sin(d);

        /// <summary>
        /// Returns the tangent of the specified angle.
        /// </summary>
        public static double Tan(this double d) => System.Math.Tan(d);

        /// <summary>
        /// Returns the angle whose cosine is the specified number.
        /// </summary>
        public static double Acos(this double d) => System.Math.Acos(d);

        /// <summary>
        /// Returns the angle whose sine is the specified number.
        /// </summary>
        public static double Asin(this double d) => System.Math.Asin(d);

        /// <summary>
        /// Returns the angle whose tangent is the specified number.
        /// </summary>
        public static double Atan(this double d) => System.Math.Atan(d);

        #endregion

        #endregion

        #region Rounding
        
        /// <summary>
        /// Returns the lowest integral number higher than or equal to the specified number.
        /// </summary>
        public static int CeiledToInt(float f) => (int)System.Math.Ceiling(f);

        /// <summary>
        /// Returns the lowest integral number higher than or equal to the specified number.
        /// </summary>
        public static int CeiledToInt(double d) => (int)System.Math.Ceiling(d);


        /// <summary>
        /// Returns the highest integral number lower than or equal to the specified number.
        /// </summary>
        public static int FlooredToInt(float f) => (int)System.Math.Floor(f);

        /// <summary>
        /// Returns the highest integral number lower than or equal to the specified number.
        /// </summary>
        public static int FlooredToInt(double d) => (int)System.Math.Floor(d);


        /// <summary>
        /// Returns the integral number closest to the specified number.
        /// </summary>
        public static int RoundedToInt(float f) => (int)System.Math.Round(f);
        
        /// <summary>
        /// Returns the integral number closest to the specified number.
        /// </summary>
        public static int RoundedToInt(double d) => (int)System.Math.Round(d);

        #endregion

        #region NaN sanity checks
        /// <summary>
        /// Throws an exception if the specified double is NaN.
        /// </summary>
        public static void ThrowIfNaN(this double d,
            string exceptionString = "Double is NaN while it is not allowed to.")
        {
            if (double.IsNaN(d))
                throw new ArithmeticException(exceptionString);
        }

        /// <summary>
        /// Throws an exception if the specified float is NaN.
        /// </summary>
        public static void ThrowIfNaN(this float f,
            string exceptionString = "Float is NaN while it is not allowed to.")
        {
            if (float.IsNaN(f))
                throw new ArithmeticException(exceptionString);
        }

        /// <summary>
        /// Throws an exception if any of the vector components is NaN.
        /// </summary>
        public static void ThrowIfNaN(this Vector2 vector,
            string exceptionString = "Vector is NaN while it is not allowed to.")
        {
            if (vector.IsNaN())
                throw new ArithmeticException(exceptionString);
        }

        /// <summary>
        /// Throws an exception if any of the vector components is NaN.
        /// </summary>
        public static void ThrowIfNaN(this Vector3 vector,
            string exceptionString = "Vector is NaN while it is not allowed to.")
        {
            if (vector.IsNaN())
                throw new ArithmeticException(exceptionString);
        }

        /// <summary>
        /// Throws an exception if any of the vector components is NaN.
        /// </summary>
        public static void ThrowIfNaN(this Vector4 vector,
            string exceptionString = "Vector is NaN while it is not allowed to.")
        {
            if (vector.IsNaN())
                throw new ArithmeticException(exceptionString);
        }

        /// <summary>
        /// Checks whether any of the vector components is NaN.
        /// </summary>
        public static bool IsNaN(this Vector2 vector) => float.IsNaN(vector.X) || float.IsNaN(vector.Y);

        /// <summary>
        /// Checks whether any of the vector components is NaN.
        /// </summary>
        public static bool IsNaN(this Vector3 vector) => float.IsNaN(vector.X) || float.IsNaN(vector.Y) || float.IsNaN(vector.Z);

        /// <summary>
        /// Checks whether any of the vector components is NaN.
        /// </summary>
        public static bool IsNaN(this Vector4 vector) => float.IsNaN(vector.X) || float.IsNaN(vector.Y) || float.IsNaN(vector.Z) || float.IsNaN(vector.W);

        #endregion

        #region Vector
        /// <summary>
        /// Turns the vector into a three-dimensional vector.
        /// </summary>
        public static Vector3 WithZ(this Vector2 xy, float z = 0) => new Vector3(xy.X, xy.Y, z);

        /// <summary>
        /// Turns the vector into a homogenuous vector.
        /// </summary>
        public static Vector4 WithW(this Vector3 xyz, float w) => new Vector4(xyz, w);

        /// <summary>
        /// Turns the vector into a homogenuous vector.
        /// </summary>
        public static Vector4 WithZw(this Vector2 xy, float z, float w) => new Vector4(xy.X, xy.Y, z, w);

        /// <summary>
        /// Normalizes a vector. If all components of the vector are zero, no exception is thrown. Instead the zero vector is returned.
        /// </summary>
        public static Vector2 NormalizedSafe(this Vector2 vector)
        {
            var lSqrd = vector.LengthSquared;

            return lSqrd == 0 ? new Vector2() : vector / lSqrd.Sqrted();
        }
        /// <summary>
        /// Normalizes a vector. If all components of the vector are zero, no exception is thrown. Instead the zero vector is returned.
        /// </summary>
        public static Vector3 NormalizedSafe(this Vector3 vector)
        {
            var lSqrd = vector.LengthSquared;

            return lSqrd == 0 ? new Vector3() : vector / lSqrd.Sqrted();
        }
        /// <summary>
        /// Normalizes a vector. If all components of the vector are zero, no exception is thrown. Instead the zero vector is returned.
        /// </summary>
        public static Vector4 NormalizedSafe(this Vector4 vector)
        {
            var lSqrd = vector.LengthSquared;

            return lSqrd == 0 ? new Vector4() : vector / lSqrd.Sqrted();
        }
        #endregion

        #region Geometric
        /// <summary>
        /// Converts floating point value into a type safe angle representation in radians.
        /// </summary>
        public static Angle Radians(this float value) => Angle.FromRadians(value);

        /// <summary>
        /// Converts floating point value into a type safe angle representation in degrees.
        /// </summary>
        public static Angle Degrees(this float value) => Angle.FromDegrees(value);

        /// <summary>
        /// Converts an integer value into a type safe angle representation in degrees.
        /// </summary>
        public static Angle Degrees(this int value) => Angle.FromDegrees(value);

        #endregion
    }
}
