using OpenTK;

namespace Bearded.Utilities.Math.Geometry
{
    /// <summary>
    /// Represents an axis-aligned rectangle.
    /// All properties assume the x axis pointing right and the y axis pointing down.
    /// </summary>
    public struct Rectangle
    {
        private readonly float x, y, width, height;

        #region Properties
        /// <summary>
        /// The x-coodinate of the left boundary of the rectangle.
        /// </summary>
        public float Left
        {
            get { return this.x; }
        }

        /// <summary>
        /// The x-coordinate of the right boundary of the rectangle.
        /// </summary>
        public float Right
        {
            get { return this.x + this.width; }
        }

        /// <summary>
        /// The y-coordinate of the top boundary of the rectangle.
        /// </summary>
        public float Top
        {
            get { return this.y; }
        }

        /// <summary>
        /// The y-coordinate of the bottom boundary of the rectangle.
        /// </summary>
        public float Bottom
        {
            get { return this.y + this.height; }
        }

        /// <summary>
        /// The coordinates of the top left corner of the rectangle.
        /// </summary>
        public Vector2 TopLeft
        {
            get { return new Vector2(this.Left, this.Top); }
        }

        /// <summary>
        /// The coordinates of the top right corner of the rectangle.
        /// </summary>
        public Vector2 TopRight
        {
            get { return new Vector2(this.Right, this.Top); }
        }

        /// <summary>
        /// The coordinates of the bottom left corner of the rectangle.
        /// </summary>
        public Vector2 BottomLeft
        {
            get { return new Vector2(this.Left, this.Bottom); }
        }

        /// <summary>
        /// The coordinates of the bottom right corner of the rectangle.
        /// </summary>
        public Vector2 BottomRight
        {
            get { return new Vector2(this.Right, this.Bottom); }
        }

        /// <summary>
        /// The coordinates of the center of mass of the rectangle.
        /// </summary>
        public Vector2 Center
        {
            get { return new Vector2(this.x + this.width * 0.5f, this.y + this.height * 0.5f); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new rectangle.
        /// </summary>
        /// <param name="x">X-coordinate of the upper left corner.</param>
        /// <param name="y">Y-coordinate of the upper left corner.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        public Rectangle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Creates a new rectangle.
        /// </summary>
        /// <param name="xy">Coordinates of the upper left corner.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        public Rectangle(Vector2 xy, float width, float height)
            : this(xy.X, xy.Y, width, height) { }
        #endregion

        #region Collision checks
        /// <summary>
        /// Checks if the rectangle contains a point.
        /// </summary>
        /// <param name="x">The x-coordinate of the point to check.</param>
        /// <param name="y">The y-coordinate of the point to check.</param>
        /// <returns>True if the point is contained in the rectangle; false otherwise.</returns>
        public bool Contains(float x, float y)
        {
            return this.Left <= x && x <= this.Right && this.Top <= y && y <= this.Bottom;
        }

        /// <summary>
        /// Checks if the rectangle contains a point.
        /// </summary>
        /// <param name="xy">The point to check.</param>
        /// <returns>True if the point is contained in the rectangle; false otherwise.</returns>
        public bool Contains(Vector2 xy)
        {
            return this.Contains(xy.X, xy.Y);
        }

        /// <summary>
        /// Checks if the rectangle contains the specified rectangle.
        /// </summary>
        /// <param name="other">The rectangle to check.</param>
        /// <returns>True if the other rectangle is contained in this rectangle; false otherwise.</returns>
        public bool Contains(Rectangle other)
        {
            return this.Left <= other.Left && other.Right <= this.Right
                && this.Top <= other.Top && other.Bottom <= this.Bottom;
        }

        /// <summary>
        /// Checks if the rectangle intersects with the specified rectangle.
        /// </summary>
        /// <param name="other">The other rectangle.</param>
        /// <returns>True if the rectangles intersect; false otherwise.</returns>
        public bool Intersects(Rectangle other)
        {
            return !(other.Left > this.Right || other.Right < this.Left ||
                other.Top > this.Bottom || other.Bottom < this.Top);
        }
        #endregion

        #region Static creators
        /// <summary>
        /// Creates a new rectangle with the specified points as corners.
        /// </summary>
        /// <returns>A rectangle with the specified points as corners.</returns>
        public static Rectangle WithCorners(Vector2 upperLeft, Vector2 bottomRight)
        {
            return new Rectangle(upperLeft.X, upperLeft.Y, bottomRight.X - upperLeft.X, bottomRight.Y - upperLeft.Y);
        }

        /// <summary>
        /// Creates a new rectangle with its boundaries at the specified coordinates.
        /// </summary>
        /// <returns>A rectangle with the specified coordinates as boundaries.</returns>
        public static Rectangle WithSides(float top, float right, float bottom, float left)
        {
            return new Rectangle(top, left, bottom - top, right - left);
        }
        #endregion
    }
}