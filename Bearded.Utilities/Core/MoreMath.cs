using System;

namespace Bearded.Utilities;

public static class MoreMath
{
    private const double degreesToRadians = Math.PI / 180;
    private const double radiansToDegrees = 180 / Math.PI;

    /// <summary>
    /// Returns the lowest integral number higher than or equal to the specified number.
    /// </summary>
    public static int CeilToInt(double d) => (int)Math.Ceiling(d);

    /// <summary>
    /// Returns the highest integral number lower than or equal to the specified number.
    /// </summary>
    public static int FloorToInt(double d) => (int)Math.Floor(d);

    /// <summary>
    /// Returns the integral number closest to the specified number.
    /// </summary>
    public static int RoundToInt(double d) => (int)Math.Round(d);

    /// <summary>
    /// Converts an angle in radians to degrees.
    /// </summary>
    public static float RadiansToDegrees(float radians) => radians * (float) radiansToDegrees;

    /// <summary>
    /// Converts an angle in radians to degrees.
    /// </summary>
    public static double RadiansToDegrees(double radians) => radians * radiansToDegrees;

    /// <summary>
    /// Converts an angle in degrees to radians.
    /// </summary>
    public static float DegreesToRadians(float degrees) => degrees * (float) degreesToRadians;

    /// <summary>
    /// Converts an angle in degrees to radians.
    /// </summary>
    public static double DegreesToRadians(double degrees) => degrees * degreesToRadians;
}
