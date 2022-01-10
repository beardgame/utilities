namespace Bearded.Utilities.Geometry;

/// <summary>
/// A collection of interval helper methods.
/// </summary>
public static class Interval
{
    /// <summary>
    /// Checks if two closed intervals overlap.
    /// </summary>
    /// <param name="amin">The (inclusive) lower bound of the first interval.</param>
    /// <param name="amax">The (inclusive) upper bound of the first interval.</param>
    /// <param name="bmin">The (inclusive) lower bound of the second interval.</param>
    /// <param name="bmax">The (inclusive) upper bound of the second interval.</param>
    /// <returns></returns>
    public static bool DoOverlap(double amin, double amax, double bmin, double bmax)
        // Negation of bmin > amax || amin > bmax
        => bmin <= amax && amin <= bmax;
}
