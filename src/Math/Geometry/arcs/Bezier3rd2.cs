using OpenTK;

namespace Bearded.Utilities.Math.Geometry
{
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
        /// Initializes the Bezier curve.
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="segments">The amount of linear segments the arc is split in. A larger amount of segments results in higher precision for length and remapping.</param>
        public Bezier3rd2(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, int segments = 100)
            : base(segments)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        protected override Vector2 getPointAt(float t)
        {
            return Interpolate.Bezier(p0, p1, p2, p3, t);
        }
    }
}