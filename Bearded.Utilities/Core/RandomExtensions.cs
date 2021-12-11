using System;

namespace Bearded.Utilities;

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
    public static long NextLong(this Random random) => random.NextLong(0, long.MaxValue);

    /// <summary>
    /// Returns random (biased) long integer in the interval [0, upper bound[
    /// </summary>
    public static long NextLong(this Random random, long max) => random.NextLong(0, max);

    /// <summary>
    /// Returns random (biased) long integer in the interval [lower bound, upper bound[
    /// </summary>
    public static long NextLong(this Random random, long min, long max)
    {
        if (min == max)
            return min;

        if (min > max)
            throw new ArgumentException("Maximum must be larger or equal to minimum bound.");

        // "awmygawd this is so biased" - Tom Rijnbeek
        var buf = new byte[8];
        random.NextBytes(buf);
        var longRand = BitConverter.ToInt64(buf, 0);

        return Math.Abs(longRand % (max - min)) + min;
    }

    #endregion

    #region NextDouble()

    /// <summary>
    /// Returns a random double in the interval [0, upper bound[.
    /// </summary>
    public static double NextDouble(this Random random, double max) => random.NextDouble() * max;

    /// <summary>
    /// Returns a random double in the interval [lower bound, upper bound[.
    /// </summary>
    public static double NextDouble(this Random random, double min, double max)
        => random.NextDouble() * (max - min) + min;

    #endregion

    #region NormalDouble()

    /// <summary>
    /// Generates a random double using the standard normal distribution.
    /// </summary>
    public static double NormalDouble(this Random random)
    {
        // Box-Muller
        var u1 = random.NextDouble();
        var u2 = random.NextDouble();
        return Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
    }

    /// <summary>
    /// Generates a random double using the normal distribution with the given mean and deviation.
    /// </summary>
    public static double NormalDouble(this Random random, double mean, double deviation)
        => mean + deviation * random.NormalDouble();

    #endregion

    #region NextFloat()

    /// <summary>
    /// Returns random float in the interval [0, 1[.
    /// </summary>
    public static float NextFloat(this Random random) => (float)random.NextDouble();

    /// <summary>
    /// Returns a random float in the interval [0, upper bound[.
    /// </summary>
    public static float NextFloat(this Random random, float max)
        => (float)(random.NextDouble() * max);

    /// <summary>
    /// Returns a random float in the interval [lower bound, upper bound[.
    /// </summary>
    public static float NextFloat(this Random random, float min, float max)
        => (float)(random.NextDouble() * (max - min) + min);

    #endregion

    #region NormalFloat()

    /// <summary>
    /// Generates a random float using the standard normal distribution.
    /// </summary>
    public static float NormalFloat(this Random random)
        => (float)random.NormalDouble();

    /// <summary>
    /// Generates a random float using the normal distribution with the given mean and deviation.
    /// </summary>
    public static float NormalFloat(this Random random, float mean, float deviation)
        => mean + (float)(deviation * random.NormalDouble());

    #endregion

    #region Various

    /// <summary>
    /// Returns -1 or 1 randomly.
    /// </summary>
    public static int NextSign(this Random random) => 2 * random.Next(2) - 1;

    /// <summary>
    /// Returns true half the time, false otherwise.
    /// </summary>
    public static bool NextBool(this Random random)
        => random.NextDouble() < 0.5;

    /// <summary>
    /// Returns true with the given probability, and false otherwise.
    /// </summary>
    public static bool NextBool(this Random random, double probability)
        => random.NextDouble() < probability;

    /// <summary>
    /// Returns an integer with a given expected value. Will always return either the floor or ceil of the given value.
    /// </summary>
    public static int Discretise(this Random random, float value)
    {
        var i = (int)value;
        var rest = value - i;
        return random.NextBool(rest) ? i + 1 : i;
    }

    #endregion

}
