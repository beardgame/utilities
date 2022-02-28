using OpenTK.Mathematics;

namespace Bearded.Utilities.Geometry;

/// <summary>
/// Represents a quadratic Bezier curve in two-dimensional space.
/// </summary>
// ReSharper disable once InconsistentNaming
public sealed class Bezier2nd2 : Arc2
{
    private readonly Vector2 p0;
    private readonly Vector2 p1;
    private readonly Vector2 p2;

    /// <summary>
    /// Initializes the Bezier curve with 100 segments.
    /// </summary>
    public Bezier2nd2(Vector2 p0, Vector2 p1, Vector2 p2)
        : this(p0, p1, p2, 100)
    {
    }

    /// <summary>
    /// Initializes the Bezier curve.
    /// </summary>
    public Bezier2nd2(Vector2 p0, Vector2 p1, Vector2 p2, int segments)
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
    protected override Vector2 getPointAt(float t)
    {
        return Interpolate.Bezier(p0, p1, p2, t);
    }
}
