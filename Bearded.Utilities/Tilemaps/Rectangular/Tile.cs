using System;
using System.Collections.Generic;

namespace Bearded.Utilities.Tilemaps.Rectangular;

/// <summary>
/// Represents a reference to a specific tile of a rectangular tilemap.
/// </summary>
/// <typeparam name="TTileValue">The kind of data contained in the tilemap.</typeparam>
public readonly struct Tile<TTileValue>
    : IEquatable<Tile<TTileValue>>
{
    private readonly Tilemap<TTileValue> tilemap;
    private readonly int x;
    private readonly int y;

    /// <summary>
    /// Creates a new tile reference.
    /// </summary>
    /// <param name="tilemap">The tilemap.</param>
    /// <param name="x">X location of tile.</param>
    /// <param name="y">Y location of tile.</param>
    /// <exception cref="ArgumentNullException">Throws if tilemap is null.</exception>
    public Tile(Tilemap<TTileValue> tilemap, int x, int y)
    {
        if (tilemap == null)
            throw new ArgumentNullException("tilemap");

        this.tilemap = tilemap;
        this.x = x;
        this.y = y;
    }

    #region properties

    /// <summary>
    /// X location of the tile.
    /// </summary>
    public int X { get { return x; } }

    /// <summary>
    /// Y location of the tile.
    /// </summary>
    public int Y { get { return y; } }

    /// <summary>
    /// The data contained in the tile. Will throw if <see cref="IsValid"/> is false.
    /// </summary>
    public TTileValue Value { get { return tilemap[X, Y]; } }

    /// <summary>
    /// True if the tile is located within its tilemap. False otherwise.
    /// </summary>
    public bool IsValid
    {
        get { return tilemap != null && tilemap.IsValidTile(this); }
    }

    /// <summary>
    /// Returns all eight tiles neighbouring this one, except those outside the tilemap.
    /// </summary>
    public IEnumerable<Tile<TTileValue>> ValidNeighbours
    {
        get
        {
            for (int i = 1; i < 9; i++)
            {
                var tile = Neighbour(Extensions.DirectionDeltas[i]);
                if (tile.IsValid)
                    yield return tile;
            }
        }
    }

    /// <summary>
    /// Returns all eight tiles neighbouring this one, whether or now they are outside the tilemap.
    /// </summary>
    public IEnumerable<Tile<TTileValue>> PossibleNeighbours
    {
        get
        {
            for (int i = 1; i < 9; i++)
            {
                yield return Neighbour(Extensions.DirectionDeltas[i]);
            }
        }
    }

    #endregion

    #region public methods

    /// <summary>
    /// Returns the tile neighbouring this one in a given direction.
    /// </summary>
    public Tile<TTileValue> Neighbour(Direction direction)
    {
        return Neighbour(direction.Step());
    }

    internal Tile<TTileValue> Neighbour(Step step)
    {
        return new Tile<TTileValue>(
            tilemap,
            x + step.X,
            y + step.Y
        );
    }

    #endregion

    #region Equals, GetHashCode, euqality operators

    /// <summary>
    /// Indicates whether this instance references the same tile as another one.
    /// </summary>
    public bool Equals(Tile<TTileValue> other)
    {
        return x == other.x && y == other.y && tilemap == other.tilemap;
    }

    /// <summary>
    /// Indicates whether this instance and a specified object are equal.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        return obj is Tile<TTileValue> && Equals((Tile<TTileValue>)obj);
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (tilemap != null ? tilemap.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ x;
            hashCode = (hashCode * 397) ^ y;
            return hashCode;
        }
    }

    /// <summary>
    /// Indicates whether this instance references the same tile as another one.
    /// </summary>
    public static bool operator ==(Tile<TTileValue> t1, Tile<TTileValue> t2)
    {
        return t1.Equals(t2);
    }

    /// <summary>
    /// Indicates whether this instance references a different tile than another one.
    /// </summary>
    public static bool operator !=(Tile<TTileValue> t1, Tile<TTileValue> t2)
    {
        return !(t1 == t2);
    }

    #endregion
}
