using OpenTK.Mathematics;

namespace Bearded.Utilities.Noise;

public interface IProceduralTexture
{
    /// <summary>
    /// Returns the value of the noise map at the given coordinates in the noise map.
    /// This method only works for values within the [0, 1) x [0, 1) range only (upper bounds exclusive).
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public double ValueAt(double x, double y);

    /// <summary>
    /// Returns the value of the noise map at the given coordinates in the noise map.
    /// This method only works for values within the [0, 1) x [0, 1) range only (upper bounds exclusive).
    /// </summary>
    /// <param name="xy"></param>
    /// <returns></returns>
    public double ValueAt(Vector2d xy)
    {
        var (x, y) = xy;
        return ValueAt(x, y);
    }

    /// <summary>
    /// Transforms the noise map to a 2D array by dividing the entire noise map in a grid of width by height tiles.
    /// We then sample the center of each of the grid tiles to get the discrete value.
    /// </summary>
    /// <param name="width">The width of the resulting array. That is, its size in the first dimension.</param>
    /// <param name="height">The height of the resulting array. That is, its size in the second dimension.</param>
    /// <returns>A 2D array with the given width and height with evaluated values based on the noise map.</returns>
    public double[,] ToArray(int width, int height)
    {
        var tileWidth = 1.0 / width;
        var tileHeight = 1.0 / height;
        var halfTileWidth = 0.5 * tileWidth;
        var halfTileHeight = 0.5 * tileHeight;

        var result = new double[width, height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                result[x, y] = ValueAt(x * tileWidth + halfTileWidth, y * tileHeight + halfTileHeight);
            }
        }

        return result;
    }
}
