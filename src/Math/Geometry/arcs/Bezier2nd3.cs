using OpenTK;

namespace Bearded.Utilities.Math.Geometry
{
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
        /// Initializes the Bezier curve.
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="segments">The amount of linear segments the arc is split in. A larger amount of segments results in higher precision for length and remapping.</param>
        public Bezier2nd3(Vector3 p0, Vector3 p1, Vector3 p2, int segments = 100)
            : base(segments)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
        }

        protected override Vector3 getPointAt(float t)
        {
            return Interpolate.Bezier(p0, p1, p2, t);
        }
    }
}