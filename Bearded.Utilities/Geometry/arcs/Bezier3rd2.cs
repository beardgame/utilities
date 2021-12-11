using OpenTK.Mathematics;

namespace Bearded.Utilities.Geometry;

/// <summary>
/// Represents a cubic Bezier curve in two-dimensional space.
/// </summary>
// ReSharper disable once InconsistentNaming
public sealed class Bezier3rd2 : Arc2
{
    private readonly Vector2 p0;
    private readonly Vector2 p1;
    private readonly Vector2 p2;
    private readonly Vector2 p3;

    /// <summary>
    /// Initializes the Bezier curve with 100 segments.
    /// </summary>
    public Bezier3rd2(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        : this(p0, p1, p2, p3, 100)
    {
    }

    /// <summary>
    /// Initializes the Bezier curve.
    /// </summary>
    public Bezier3rd2(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int segments)
        : base(segments)
    {
        this.p0 = p0;
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
    }

    /// <summary>
    /// Calculates the point on the Bezier curve at parameter t.
    /// </summary>
    /// <param name="t">The arc parameter t.</param>
    /// <returns>The Euclidean coordinates of the point on the curve at parameter t.</returns>
    protected override Vector2 getPointAt(float t)
    {
        return Interpolate.Bezier(p0, p1, p2, p3, t);
    }
}
