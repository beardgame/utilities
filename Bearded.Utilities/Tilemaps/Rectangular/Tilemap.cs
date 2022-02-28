using System.Collections;
using System.Collections.Generic;

namespace Bearded.Utilities.Tilemaps.Rectangular;

/// <summary>
/// A rectangular tilemap.
/// </summary>
/// <typeparam name="TTileValue">The kind of data contained in the tilemap.</typeparam>
public class Tilemap<TTileValue>
    : IReadOnlyCollection<Tile<TTileValue>>
{
    private readonly int width;
    private readonly int height;
    private readonly TTileValue[,] tiles;

    /// <summary>
    /// Creates a new rectangular tilemap.
    /// </summary>
    /// <param name="width">The width of the tilemap.</param>
    /// <param name="height">The height of the tilemap.</param>
    public Tilemap(int width, int height)
    {
        this.width = width;
        this.height = height;
        tiles = new TTileValue[width, height];
    }

    #region properties

    /// <summary>
    /// The width of the tilemap.
    /// </summary>
    public int Width
    {
        get { return width; }
    }

    /// <summary>
    /// The height of the tilemap.
    /// </summary>
    public int Height
    {
        get { return height; }
    }

    /// <summary>
    /// Gets the number of tiles in the tilemap.
    /// </summary>
    public int Count
    {
        get { return width * height; }
    }

    #endregion

    #region indexers

    /// <summary>
    /// Gets or sets the data of a tile by its coordinates.
    /// </summary>
    /// <param name="x">The X location of the tile.</param>
    /// <param name="y">The Y location of the tile.</param>
    public TTileValue this[int x, int y]
    {
        get { return tiles[x, y]; }
        set { tiles[x, y] = value; }
    }

    /// <summary>
    /// Gets or sets the data of a tile.
    /// </summary>
    /// <param name="tile">The tile.</param>
    public TTileValue this[Tile<TTileValue> tile]
    {
        get { return this[tile.X, tile.Y]; }
        set { this[tile.X, tile.Y] = value; }
    }

    #endregion

    #region public methods

    /// <summary>
    /// Checks whether a specific tile location is inside the tilemap.
    /// </summary>
    /// <param name="x">The X location of the tile.</param>
    /// <param name="y">The Y location of the tile.</param>
    public bool IsValidTile(int x, int y)
    {
        return x >= 0 && x < width
            && y >= 0 && y < height;
    }

    /// <summary>
    /// Checks whether a specific tile is located inside the tilemap.
    /// </summary>
    public bool IsValidTile(Tile<TTileValue> tile)
    {
        return IsValidTile(tile.X, tile.Y);
    }

    #endregion

    #region IEnumerable implementation

    /// <summary>
    /// Returns an enumerator that iterates all tiles in the tilemap.
    /// </summary>
    public IEnumerator<Tile<TTileValue>> GetEnumerator()
    {
        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            yield return new Tile<TTileValue>(this, x, y);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates all tiles in the tilemap.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}
