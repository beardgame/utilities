using System;
using OpenTK;

namespace Bearded.Utilities.Math.Geometry
{
    /// <summary>
    /// Represents an axis-aligned rectangle.
    /// All properties assume the x axis pointing right and the y axis pointing down.
    /// Negative width or height rectangles are not allowed, and the constructor will throw with those values.
    /// </summary>
    public struct Rectangle
    {
        public float Left { get; }
        public float Top { get; }
        public float Width { get; }
        public float Height { get; }
        
        public float Right => Left + Width;
        public float Bottom => Top + Height;

        public Vector2 TopLeft => new Vector2(Left, Top);
        public Vector2 TopRight => new Vector2(Right, Top);
        public Vector2 BottomLeft => new Vector2(Left, Bottom);
        public Vector2 BottomRight => new Vector2(Right, Bottom);

        public Vector2 Center => new Vector2(Left + Width * 0.5f, Top + Height * 0.5f);

        public float Area => Width * Height;

        public Rectangle(float x, float y, float width, float height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentException("Width and height of the rectangle have to be non-negative.");

            Left = x;
            Top = y;
            Width = width;
            Height = height;
        }

        public Rectangle(Vector2 xy, float width, float height)
            : this(xy.X, xy.Y, width, height) { }
        
        public bool Contains(float x, float y)
            => Left <= x && x <= Right && Top <= y && y <= Bottom;

        public bool Contains(Vector2 xy) => Contains(xy.X, xy.Y);

        public bool Contains(Rectangle other)
            => Left <= other.Left && other.Right <= Right
                && Top <= other.Top && other.Bottom <= Bottom;

        public bool Intersects(Rectangle other)
            => !(other.Left > Right || other.Right < Left
                || other.Top > Bottom || other.Bottom < Top);
        
        public static Rectangle WithCorners(Vector2 upperLeft, Vector2 bottomRight)
            => new Rectangle(upperLeft.X, upperLeft.Y, bottomRight.X - upperLeft.X, bottomRight.Y - upperLeft.Y);

        public static Rectangle WithSides(float top, float right, float bottom, float left)
            => new Rectangle(top, left, bottom - top, right - left);
        
    }
}