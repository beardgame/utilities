using System;

namespace Bearded.Utilities;

/// <summary>
/// This static class offers a variety of pseudo random methods.
/// The class is threadsafe and uses a different internal random object for each thread.
/// Note that several of the methods are slightly biased for the sake of performance.
/// </summary>
/// <remarks>The actual implementations of the custom random methods can be found in RandomExtensions.</remarks>
public static class StaticRandom
{
    #region Threadsafe random
    [ThreadStatic]
    private static Random random;

    /// <summary>
    /// The thread safe instance of Random used by StaticRandom
    /// </summary>
    // is internal for use as default random in Linq.Extensions
    internal static Random Random => random ?? (random = new Random());

    /// <summary>
    /// Overrides the Random object for the calling thread by one with the given seed.
    /// </summary>
    public static void SeedWith(int seed)
    {
        random = new Random(seed);
    }

    #endregion

    #region Int()

    /// <summary>
    /// Returns a random integer.
    /// </summary>
    public static int Int() => Random.Next();

    /// <summary>
    /// Returns a (biased) random integer in the interval [0, upper bound[.
    /// </summary>
    public static int Int(int max) => Random.Next(max);

    /// <summary>
    /// Returns random (biased) integer in the interval [lower bound, upper bound[.
    /// </summary>
    public static int Int(int min, int max) => Random.Next(min, max);

    #endregion

    #region Long()

    /// <summary>
    /// Returns random (biased) long integer.
    /// </summary>
    public static long Long() => Random.NextLong();

    /// <summary>
    /// Returns random (biased) long integer in the interval [0, upper bound[.
    /// </summary>
    public static long Long(long max) => Random.NextLong(max);

    /// <summary>
    /// Returns random (biased) long integer in the interval [lower bound, upper bound[.
    /// </summary>
    public static long Long(long min, long max) => Random.NextLong(min, max);

    #endregion

    #region Double()

    /// <summary>
    /// Returns a random double in the interval [0, 1[.
    /// </summary>
    public static double Double() => Random.NextDouble();

    /// <summary>
    /// Returns a random double in the interval [0, upper bound[.
    /// </summary>
    public static double Double(double max) => Random.NextDouble(max);

    /// <summary>
    /// Returns a random double in the interval [lower bound, upper bound[.
    /// </summary>
    public static double Double(double min, double max) => Random.NextDouble(min, max);

    #endregion

    #region NormalDouble()

    /// <summary>
    /// Generates a random double using the standard normal distribution.
    /// </summary>
    public static double NormalDouble() => Random.NormalDouble();

    /// <summary>
    /// Generates a random double using the normal distribution with the given mean and deviation.
    /// </summary>
    public static double NormalDouble(double mean, double deviation) => Random.NormalDouble(mean, deviation);

    #endregion

    #region Float()
    /// <summary>
    /// Returns random float in the interval [0, 1[.
    /// </summary>
    public static float Float() => Random.NextFloat();

    /// <summary>
    /// Returns a random float in the interval [0, upper bound[.
    /// </summary>
    public static float Float(float max) => Random.NextFloat(max);

    /// <summary>
    /// Returns a random float in the interval [lower bound, upper bound[.
    /// </summary>
    public static float Float(float min, float max) => Random.NextFloat(min, max);

    #endregion

    #region NormalFloat()

    /// <summary>
    /// Generates a random float using the standard normal distribution.
    /// </summary>
    public static float NormalFloat() => Random.NormalFloat();

    /// <summary>
    /// Generates a random float using the normal distribution with the given mean and deviation.
    /// </summary>
    public static float NormalFloat(float mean, float deviation) => Random.NormalFloat(mean, deviation);

    #endregion

    #region Various

    /// <summary>
    /// Returns -1 or 1 randomly.
    /// </summary>
    public static int Sign() => Random.NextSign();

    /// <summary>
    /// Returns true half the time, false otherwise.
    /// </summary>
    public static bool Bool() => Random.NextBool();

    /// <summary>
    /// Returns true with the given probability, and false otherwise.
    /// </summary>
    public static bool Bool(double probability) => Random.NextBool(probability);


    /// <summary>
    /// Returns an integer with a given expected value. Will always return either the floor or ceil of the given value.
    /// </summary>
    public static int Discretise(float value) => Random.Discretise(value);

    #endregion
}
