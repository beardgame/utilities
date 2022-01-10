using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Bearded.Utilities.Geometry;

namespace Bearded.Utilities.Tilemaps.Rectangular;

/// <summary>
/// Extension methods for rectengular tilemaps.
/// </summary>
public static class Extensions
{
    #region Lookup Tables

    internal static readonly Step[] DirectionDeltas =
    {
        new Step(0, 0),
        new Step(1, 0),
        new Step(1, 1),
        new Step(0, 1),
        new Step(-1, 1),
        new Step(-1, 0),
        new Step(-1, -1),
        new Step(0, -1),
        new Step(1, -1),
    };
    private static readonly Direction[] directionOpposite =
    {
        Direction.Unknown,
        Direction.Left,
        Direction.DownLeft,
        Direction.Down,
        Direction.DownRight,
        Direction.Right,
        Direction.UpRight,
        Direction.Up,
        Direction.UpLeft,
    };

    private static readonly Direction[] directions =
    {
        Direction.Right,
        Direction.UpRight,
        Direction.Up,
        Direction.UpLeft,
        Direction.Left,
        Direction.DownLeft,
        Direction.Down,
        Direction.DownRight,
    };

    private static readonly ReadOnlyCollection<Direction> directionsAsReadOnly
        = Array.AsReadOnly(directions);

    /// <summary>
    /// A list of the eight directions in rectangular tilemaps.
    /// </summary>
    public static ReadOnlyCollection<Direction> Directions
    {
        get { return directionsAsReadOnly; }
    }

    #endregion

    #region Direction

    internal static Step Step(this Direction direction)
    {
        return DirectionDeltas[(int)direction];
    }

    /// <summary>
    /// Returns the direction opposite to this one.
    /// </summary>
    public static Direction Opposite(this Direction direction)
    {
        return directionOpposite[(int)direction];
    }

    /// <summary>
    /// Returns the closest of the eight directions inside a rectangular tilemap, by angle.
    /// </summary>
    public static Direction Octagonal(this Direction2 direction)
    {
        return (Direction)((int)Math.Floor(direction.Degrees * (1 / 45f) + 0.5f) % 8 + 1);
    }

    /// <summary>
    /// Returns the closest of the four non-diagonal directions inside a rectangular tilemap, by angle.
    /// </summary>
    public static Direction Quadrogonal(this Direction2 direction)
    {
        return (Direction)((
            (int)Math.Floor(direction.Degrees * (1 / 90f) + 0.5f) % 4
        ) * 2 + 1);
    }

    #endregion

    #region Directions

    /// <summary>
    /// Checks if any directions are set.
    /// </summary>
    /// <returns>True if any direction is set. False otherwise.</returns>
    public static bool Any(this Directions direction)
    {
        return direction != Rectangular.Directions.None;
    }

    /// <summary>
    /// Checks if any of the given directions are set, i.e. if the directions intersect.
    /// </summary>
    /// <param name="direction">The directions to check.</param>
    /// <param name="match">The directions to search for.</param>
    /// <returns>True if the directions intersect. False otherwise</returns>
    public static bool Any(this Directions direction, Directions match)
    {
        return direction.Intersect(match) != Rectangular.Directions.None;
    }

    /// <summary>
    /// Checks if all directions are set.
    /// </summary>
    /// <returns>True if all direction are set. False otherwise.</returns>
    public static bool All(this Directions direction)
    {
        return direction == Rectangular.Directions.All;
    }

    /// <summary>
    /// Checks if all of the given directions are set, i.e. if their intersection equals the parameter.
    /// </summary>
    /// <param name="direction">The directions to check.</param>
    /// <param name="match">The directions to search for.</param>
    /// <returns>True if all directions are included. False otherwise</returns>
    public static bool All(this Directions direction, Directions match)
    {
        return direction.HasFlag(match);
    }

    /// <summary>
    /// Returns the union of two sets of directions.
    /// </summary>
    public static Directions Union(this Directions directions, Directions directions2)
    {
        return directions | directions2;
    }

    /// <summary>
    /// Excludes one set of directions from another.
    /// </summary>
    /// <returns>The original value, without the given directions.</returns>
    public static Directions Except(this Directions directions, Directions directions2)
    {
        return directions & ~directions2;
    }

    /// <summary>
    /// Returns the intersection of two sets of directions.
    /// </summary>
    public static Directions Intersect(this Directions directions, Directions directions2)
    {
        return directions & directions2;
    }

    #endregion

    #region Direction and Directions

    private static Directions toDirections(this Direction direction)
    {
        return (Directions)(1 << ((int)direction - 1));
    }

    /// <summary>
    /// Enumerates all the set directions.
    /// </summary>
    public static IEnumerable<Direction> Enumerate(this Directions directions)
    {
        return Extensions.directions.Where(direction => directions.Includes(direction));
    }

    /// <summary>
    /// Checks if a specific direction is set.
    /// </summary>
    /// <returns>True if the given direction is set. False otherwise.</returns>
    public static bool Includes(this Directions directions, Direction direction)
    {
        return directions.HasFlag(direction.toDirections());
    }

    /// <summary>
    /// Sets a given direction.
    /// </summary>
    /// <returns>A union of the original value and the given direction.</returns>
    public static Directions And(this Directions directions, Direction direction)
    {
        return directions | direction.toDirections();
    }

    /// <summary>
    /// Un-sets a given direction.
    /// </summary>
    /// <returns>The original value, without the given direction.</returns>
    public static Directions Except(this Directions directions, Direction direction)
    {
        return directions & ~direction.toDirections();
    }

    #endregion
}
