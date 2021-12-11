using OpenTK.Mathematics;

namespace Bearded.Utilities.Geometry;

/// <summary>
/// Represents an arc in three-dimensional Euclidean space.
/// </summary>
public abstract class Arc3 : Arc<Vector3>
{
    /// <summary>
    /// Initializes the arc.
    /// </summary>
    /// <param name="segments">The amount of linear segments the arc is split in. A larger amount of segments results in higher precision for length and remapping.</param>
    protected Arc3(int segments = 100)
        : base(segments) { }

    /// <summary>
    /// Calculates the distance between two points using the Euclidean metric.
    /// </summary>
    /// <param name="p1">The first coordinate.</param>
    /// <param name="p2">The second coordinate.</param>
    /// <returns>The Euclidean distance between the specified points.</returns>
    protected override float getDistanceBetween(Vector3 p1, Vector3 p2)
    {
        return (p2 - p1).Length;
    }
}
