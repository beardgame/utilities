using System;
using OpenTK;

namespace Bearded.Utilities.Math.Geometry
{
    /// <summary>
    /// Represents a position in two-dimensional space using polar coordinates.
    /// </summary>
    public struct PolarPosition : IEquatable<PolarPosition>
    {
        #region Fields
        private readonly float r;
        private readonly Direction2 angle;
        #endregion

        #region Properties
        /// <summary>
        /// Distance from the origin.
        /// </summary>
        public float R
        {
            get { return this.r; }
        }

        /// <summary>
        /// Direction of the vector originating from the origin pointing towards the point.
        /// </summary>
        public Direction2 Angle
        {
            get { return this.angle; }
        }
        #endregion

        /// <summary>
        /// Creates a new polar position.
        /// </summary>
        /// <param name="r">The distance of the point to the origin.</param>
        /// <param name="angle">The direction of the vector originating in the origin pointing towards the point.</param>
        public PolarPosition(float r, Direction2 angle)
        {
            if (r < 0)
                throw new ArgumentException("The radius has to be non-negative.");

            this.r = r;
            this.angle = angle;
        }

        #region Conversion methods
        /// <summary>
        /// Converts the polar coordinates into Euclidean coordinates.
        /// </summary>
        /// <returns>Vector corresponding to the vector originating in the origin pointing towards this point.</returns>
        public Vector2 ToVector2()
        {
            return this.angle.Vector * this.r;
        }
        #endregion

        #region Static creators
        /// <summary>
        /// Converts an Euclidean position into polar coordinates.
        /// </summary>
        /// <param name="position">The Euclidean representation of the point.</param>
        /// <returns>The polar representation of the specified point.</returns>
        public static PolarPosition FromVector2(Vector2 position)
        {
            return new PolarPosition(position.Length, Direction2.Of(position));
        }
        #endregion

        #region IEquatable implementation
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(PolarPosition other)
        {
            return this.r == other.r && (this.r == 0 || this.angle == other.angle);
        }
        #endregion
    }
}