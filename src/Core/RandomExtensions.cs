using System;

namespace Bearded.Utilities
{
    /// <summary>
    /// This class adds a variety of extension methods for the Random class to expand its functionality.
    /// Note that several of these methods are slightly biased for the sake of performance.
    /// </summary>
    public static class RandomExtensions
    {
        #region NextLong()

        /// <summary>
        /// Returns random (biased) long integer.
        /// </summary>
        /// <param name="random">The Random instance to sample with.</param>
        public static long NextLong(this Random random)
        {
            return random.NextLong(0, long.MaxValue);
        }

        /// <summary>
        /// Returns random (biased) long integer in the interval [0, upper bound[
        /// </summary>
        /// <param name="random">The Random instance to sample with.</param>
        /// <param name="max">The exclusive upper bound.</param>
        public static long NextLong(this Random random, long max)
        {
            return random.NextLong(0, max);
        }

        /// <summary>
        /// Returns random (biased) long integer in the interval [lower bound, upper bound[
        /// </summary>
        /// <param name="random">The Random instance to sample with.</param>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The exclusive upper bound.</param>
        public static long NextLong(this Random random, long min, long max)
        {
            // "awmygawd this is so biased" - Tom Rijnbeek
            var buf = new byte[8];
            random.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (System.Math.Abs(longRand % (max - min)) + min);
        }

        #endregion

        #region NextDouble()

        /// <summary>
        /// Returns a random double in the interval [0, upper bound[.
        /// </summary>
        /// <param name="random">The Random instance to sample with.</param>
        /// <param name="max">The exclusive upper bound.</param>
        public static double NextDouble(this Random random, double max)
        {
            return random.NextDouble() * max;
        }

        /// <summary>
        /// Returns a random double in the interval [lower bound, upper bound[.
        /// </summary>
        /// <param name="random">The Random instance to sample with.</param>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The exclusive upper bound.</param>
        public static double NextDouble(this Random random, double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }

        #endregion

        #region NormalDouble()

        /// <summary>
        /// Generates a random double using the standard normal distribution.
        /// </summary>
        /// <param name="random">The Random instance to sample with.</param>
        public static double NormalDouble(this Random random)
        {
            // Box-Muller
            double u1 = random.NextDouble();
            double u2 = random.NextDouble();
            return System.Math.Sqrt(-2 * System.Math.Log(u1)) * System.Math.Cos(2 * System.Math.PI * u2);
        }

        /// <summary>
        /// Generates a random double using the normal distribution with the given mean and deviation.
        /// </summary>
        /// <param name="random">The Random instance to sample with.</param>
        /// <param name="mean">The expected value.</param>
        /// <param name="deviation">The standard deviation.</param>
        public static double NormalDouble(this Random random, double mean, double deviation)
        {
            return mean + deviation * random.NormalDouble();
        }

        #endregion

        #region NextFloat()

        /// <summary>
        /// Returns random float in the interval [0, 1[.
        /// </summary>
        /// <param name="random">The Random instance to sample with.</param>
        public static float NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }

        /// <summary>
        /// Returns a random float in the interval [0, upper bound[.
        /// </summary>
        /// <param name="random">The Random instance to sample with.</param>
        /// <param name="max">The exclusive upper bound.</param>
        public static float NextFloat(this Random random, float max)
        {
            return (float)(random.NextDouble() * max);
        }

        /// <summary>
        /// Returns a random float in the interval [lower bound, upper bound[.
        /// </summary>
        /// <param name="random">The Random instance to sample with.</param>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The exclusive upper bound.</param>
        public static float NextFloat(this Random random, float min, float max)
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }

        #endregion

        #region NormalFloat()

        /// <summary>
        /// Generates a random float using the standard normal distribution.
        /// </summary>
        /// <param name="random">The Random instance to sample with.</param>
        public static float NormalFloat(this Random random)
        {
            // Box-Muller
            double u1 = random.NextDouble();
            double u2 = random.NextDouble();
            return (float)(System.Math.Sqrt(-2 * System.Math.Log(u1)) * System.Math.Cos(2 * System.Math.PI * u2));
        }

        /// <summary>
        /// Generates a random float using the normal distribution with the given mean and deviation.
        /// </summary>
        /// <param name="random">The Random instance to sample with.</param>
        /// <param name="mean">The expected value.</param>
        /// <param name="deviation">The standard deviation.</param>
        public static float NormalFloat(this Random random, float mean, float deviation)
        {
            return mean + (float)(deviation * random.NormalDouble());
        }

        #endregion

        #region Various

        /// <summary>
        /// Returns -1 or 1 randomly.
        /// </summary>
        /// <param name="random">The Random instance to sample with.</param>
        public static int NextSign(this Random random)
        {
            return 2 * random.Next(2) - 1;
        }

        /// <summary>
        /// Returns a random boolean value with specified probability.
        /// </summary>
        /// <param name="random">The Random instance to sample with.</param>
        /// <param name="probability">The probability with which to return true.</param>
        /// <returns>Always returns true for probabilities greater or equal to 1 and false for probabilities less or equal to 0.</returns>
        public static bool NextBool(this Random random, double probability = 0.5)
        {
            return random.NextDouble() < probability;
        }

        /// <summary>
        /// Returns an integer with a given expected value. Will always return either the floor or ceil of the given value.
        /// </summary>
        /// <param name="random">The Random instance to sample with.</param>
        /// <param name="value">The expected value.</param>
        public static int Discretise(this Random random, float value)
        {
            var i = (int)value;
            var rest = value - i;
            if (random.NextBool(rest))
                i++;
            return i;
        }

        #endregion

    }
}
