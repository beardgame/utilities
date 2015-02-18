namespace Bearded.Utilities.Math
{
    /// <summary>
    /// Math extension methods.
    /// </summary>
    public static class Extensions
    {
        #region Clamped
        /// <summary>
        /// Clamps the value to a specified range.
        /// </summary>
        /// <param name="value">The value that should be restricted to the specified range.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
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
        /// <param name="value">The value that should be restricted to the specified range.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
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
        /// <param name="value">The value that should be restricted to the specified range.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
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
        /// <param name="a">The number.</param>
        /// <param name="n">The modulo.</param>
        /// <returns>a (mod n)</returns>
        public static int ModuloN(this int a, int n)
        {
            return ((a % n) + n) % n;
        }
        #endregion

        #region (Austin?) Powers
        /// <summary>
        /// Squares an integer.
        /// </summary>
        /// <param name="i">The integer.</param>
        /// <returns>The square.</returns>
        public static int Squared(this int i)
        {
            return i * i;
        }

        /// <summary>
        /// Squares a float.
        /// </summary>
        /// <param name="f">The float.</param>
        /// <returns>The square.</returns>
        public static float Squared(this float f)
        {
            return f * f;
        }

        /// <summary>
        /// Squares a double.
        /// </summary>
        /// <param name="d">The double.</param>
        /// <returns>The square.</returns>
        public static double Squared(this double d)
        {
            return d * d;
        }

        /// <summary>
        /// Cubes an integer.
        /// </summary>
        /// <param name="i">The integer.</param>
        /// <returns>The cube.</returns>
        public static int Cubed(this int i)
        {
            return i * i.Squared();
        }

        /// <summary>
        /// Squares a float.
        /// </summary>
        /// <param name="f">The float.</param>
        /// <returns>The square.</returns>
        public static float Cubed(this float f)
        {
            return f * f.Squared();
        }

        /// <summary>
        /// Squares a double.
        /// </summary>
        /// <param name="d">The double.</param>
        /// <returns>The cube.</returns>
        public static double Cubed(this double d)
        {
            return d * d.Squared();
        }
        #endregion

        #region Float math
        /// <summary>
        /// Returns the cosine of the specified angle.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Cos(this float f)
        {
            return (float)System.Math.Cos(f);
        }

        /// <summary>
        /// Returns the sine of the specified angle.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Sin(this float f)
        {
            return (float)System.Math.Sin(f);
        }

        /// <summary>
        /// Returns the tangent of the specified angle.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Tan(this float f)
        {
            return (float)System.Math.Tan(f);
        }

        /// <summary>
        /// Returns the angle whose cosine is the specified number.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Acos(this float f)
        {
            return (float)System.Math.Acos(f);
        }

        /// <summary>
        /// Returns the angle whose sine is the specified number.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Asin(this float f)
        {
            return (float)System.Math.Asin(f);
        }

        /// <summary>
        /// Returns the angle whose tangent is the specified number.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Atan(this float f)
        {
            return (float)System.Math.Atan(f);
        }

        /// <summary>
        /// Returns the square root of the specified number.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float Sqrted(this float f)
        {
            return (float)System.Math.Sqrt(f);
        }

        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static float Powed(this float b, float power)
        {
            return (float)System.Math.Pow(b, power);
        }

        /// <summary>
        /// Returns the lowest integral number higher than or equal to the specified number.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static int CeiledToInt(float f)
        {
            return (int)System.Math.Ceiling(f);
        }

        /// <summary>
        /// Returns the highest integral number lower than or equal to the specified number.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static int FlooredToInt(float f)
        {
            return (int)System.Math.Floor(f);
        }

        /// <summary>
        /// Returns the integral number closest to the specified number.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static int RoundedToInt(float f)
        {
            return (int)System.Math.Round(f);
        }

        /// <summary>
        /// Returns the lowest integral number higher than or equal to the specified number.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int CeiledToInt(double d)
        {
            return (int)System.Math.Ceiling(d);
        }

        /// <summary>
        /// Returns the highest integral number lower than or equal to the specified number.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int FlooredToInt(double d)
        {
            return (int)System.Math.Floor(d);
        }

        /// <summary>
        /// Returns the integral number closest to the specified number.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int RoundedToInt(double d)
        {
            return (int)System.Math.Round(d);
        }
        #endregion
    }
}