using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bearded.Utilities.Algorithms;

public static partial class BinPacking
{
    public interface IRectangle
    {
        public int Width { get; }
        public int Height { get; }
    }

    /// <summary>
    /// Input rectangle type for the packing algorithm.
    /// </summary>
    /// <typeparam name="T">Type of custom user data associated with the rectangle.</typeparam>
    public sealed class Rectangle<T> : IRectangle
    {
        public T Value { get; }
        public int Width { get; }
        public int Height { get; }

        public Rectangle(T value, int width, int height)
        {
            Value = value;
            Width = width;
            Height = height;
        }
    }

    /// <summary>
    /// Output rectangle type of the packing algorithm.
    /// </summary>
    /// <typeparam name="T">Type of custom user data associated with the rectangle.</typeparam>
    public sealed class PositionedRectangle<T>
    {
        private readonly Rectangle<T> rectangle;

        public int X { get; }
        public int Y { get; }
        public T Value => rectangle.Value;
        public int Width => rectangle.Width;
        public int Height => rectangle.Height;

        internal PositionedRectangle(Rectangle<T> rectangle, int x, int y)
        {
            this.rectangle = rectangle;
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// Result container of the rectangle packing algorithm.
    /// </summary>
    /// <typeparam name="T">Type of custom user data associated with the rectangle.</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// The list of packed rectangles.
        /// </summary>
        public ReadOnlyCollection<PositionedRectangle<T>> Rectangles { get; }

        /// <summary>
        /// The width of the containing rectangle.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// The height of the containing rectangle.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Number of pixels or grid cells of the containing rectangle wasted with this packing.
        /// </summary>
        public int EmptyPixels { get; }

        /// <summary>
        /// The total area of the containing rectangle (Width * Height).
        /// </summary>
        public int Area => Width * Height;

        /// <summary>
        /// The fraction of pixels or grid cells of the containing rectangle filled by the packed rectangles.
        /// Maximum is 1 for a perfect filling.
        /// </summary>
        public double Filled => 1 - EmptyPixels / (double) Area;

        internal static Result<T> Empty { get; } = new Result<T>(
            new ReadOnlyCollection<PositionedRectangle<T>>(new List<PositionedRectangle<T>>()),
            0, 0, 0);

        internal Result(ReadOnlyCollection<PositionedRectangle<T>> rectangles, int width, int height,
            int emptyPixels)
        {
            Rectangles = rectangles;
            Width = width;
            Height = height;
            EmptyPixels = emptyPixels;
        }
    }
}
