using OpenTK.Mathematics;

namespace Bearded.Utilities.Geometry;

/// <summary>
/// Represents a quadratic Bezier curve in three-dimensional space.
/// </summary>
// ReSharper disable once InconsistentNaming
public sealed class Bezier2nd3 : Arc3
{
    private readonly Vector3 p0;
    private readonly Vector3 p1;
    private readonly Vector3 p2;

    /// <summary>
    /// Initializes the Bezier curve with 100 segments.
    /// </summary>
    public Bezier2nd3(Vector3 p0, Vector3 p1, Vector3 p2)
        : this(p0, p1, p2, 100)
    {
    }

    /// <summary>
    /// Initializes the Bezier curve.
    /// </summary>
    public Bezier2nd3(Vector3 p0, Vector3 p1, Vector3 p2, int segments)
        : base(segments)
    {
        this.p0 = p0;
        this.p1 = p1;
        this.p2 = p2;
    }

    /// <summary>
    /// Calculates the point on the Bezier curve at parameter t.
    /// </summary>
    /// <param name="t">The arc parameter t.</param>
    /// <returns>The Euclidean coordinates of the point on the curve at parameter t.</returns>
    protected override Vector3 getPointAt(float t)
    {
        return Interpolate.Bezier(p0, p1, p2, t);
    }
}
