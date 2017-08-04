using System;

namespace Bearded.Utilities.Math
{
    /// <summary>
    /// Represents a generic arc.
    /// The class handles calculation of the approximate length and is able to normalise against the tangent length.
    /// </summary>
    /// <typeparam name="T">The coordinate type.</typeparam>
    public abstract class Arc<T>
    {
        #region Fields
        private bool initialized;
        private readonly float[] segmentLengths;
        private float length;
        #endregion

        #region Properties
        /// <summary>
        /// The (approximate) length of the arc.
        /// </summary>
        public float Length
        {
            get
            {
                this.ensureInitialized();
                return this.length;
            }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes the arc.
        /// </summary>
        /// <param name="segments">The amount of linear segments the arc is split in. A larger amount of segments results in higher precision for length and remapping.</param>
        protected Arc(int segments = 100)
        {
            this.segmentLengths = new float[segments + 1];
        }

        // This class is lazy as our children might need to set fields
        // in their constructor before being able to execute the
        // getPointAt and getDistanceBetween methods correctly.
        private void ensureInitialized()
        {
            if (this.initialized)
                return;

            this.segmentLengths[0] = 0;
            T prev = this.getPointAt(0);

            float totalLength = 0;
            float step = 1.0f / (this.segmentLengths.Length - 1);

            for (int i = 1; i < this.segmentLengths.Length; i++)
            {
                float t = i * step;
                T curr = this.getPointAt(t);
                totalLength += this.getDistanceBetween(prev, curr);
                this.segmentLengths[i] = totalLength;
                prev = curr;
            }

            this.length = totalLength;
            this.initialized = true;
        }
        #endregion

        #region Get methods
        /// <summary>
        /// Calculates the position of the point on the arc at parameter t.
        /// </summary>
        /// <param name="t">The arc parameter (should be between 0 and 1).</param>
        /// <returns>The coordinates of the point on the arc at the specified parameter.</returns>
        protected abstract T getPointAt(float t);

        /// <summary>
        /// Calculates the distance between a set of coordinates.
        /// </summary>
        /// <param name="p1">The first coordinate.</param>
        /// <param name="p2">The second coordinate.</param>
        /// <returns>The distance between the specified coordinates.</returns>
        protected abstract float getDistanceBetween(T p1, T p2);

        /// <summary>
        /// Gets the point at the specified parameter of the arc taking tangent speed into account.
        /// </summary>
        /// <param name="t">The arc parameter (should be between 0 and 1).</param>
        /// <returns>The coordinates of the point on the arc at the specified parameter.</returns>
        public T GetPointAt(float t)
        {
            return this.getPointAt(t);
        }

        /// <summary>
        /// Gets the point at the specified parameter of the arc normalised against tangent speed.
        /// </summary>
        /// <param name="t">The arc parameter (should be between 0 and 1).</param>
        /// <returns>The coordinates of the point on the arc at the specified parameter.</returns>
        public T GetPointAtNormalised(float t)
        {
            this.ensureInitialized();
            return this.getPointAt(this.mapDistance(t.Clamped(0, 1) * this.Length));
        }
        #endregion

        #region Distance mapping
        private float mapDistance(float distance)
        {
            if (distance <= 0)
                return 0;
            if (distance >= this.Length)
                return 1;

            int i = this.findSegmentLengthIndex(distance);
            float step = 1.0f / this.segmentLengths.Length;
            if (this.segmentLengths[i] == distance)
                return i * step;
            if (i == 0)
                return 0;

            float over = distance - this.segmentLengths[i - 1];
            float ratio = over / (this.segmentLengths[i] - this.segmentLengths[i - 1]);
            return (i + ratio) * step;
        }

        private int findSegmentLengthIndex(float distance)
        {
            int index = Array.BinarySearch(this.segmentLengths, distance);
            return index < 0 ? ~index : 0;
        }
        #endregion
    }
}
