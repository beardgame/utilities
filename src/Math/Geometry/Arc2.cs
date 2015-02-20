using OpenTK;

namespace Bearded.Utilities.Math.Geometry
{
    /// <summary>
    /// Represents an arc in two-dimensional Euclidean space.
    /// </summary>
    public abstract class Arc2 : Arc<Vector2>
    {
        /// <summary>
        /// Initializes the arc.
        /// </summary>
        /// <param name="segments">The amount of linear segments the arc is split in. A larger amount of segments results in higher precision for length and remapping.</param>
        protected Arc2(int segments = 100)
            : base(segments) { }

        protected override float getDistanceBetween(Vector2 p1, Vector2 p2)
        {
            return (p2 - p1).Length;
        }
    }
}