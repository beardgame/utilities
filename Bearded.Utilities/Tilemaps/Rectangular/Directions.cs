using System;

namespace Bearded.Utilities.Tilemaps.Rectangular;

/// <summary>
/// Represents any combinations of the eight directions in a rectangular tilemap.
/// </summary>
[Flags]
public enum Directions : byte
{
    /// <summary>
    /// No direction.
    /// </summary>
    None = 0,
    /// <summary>
    /// Step to the right (0 degrees).
    /// </summary>
    Right = 1 << (Direction.Right - 1),
    /// <summary>
    /// Step to the upper right. (45 degrees).
    /// </summary>
    UpRight = 1 << (Direction.UpRight - 1),
    /// <summary>
    /// Step upwards. (90 degrees).
    /// </summary>
    Up = 1 << (Direction.Up - 1),
    /// <summary>
    /// Step to the upper left (135 degrees).
    /// </summary>
    UpLeft = 1 << (Direction.UpLeft - 1),
    /// <summary>
    /// Step to the left (180 degrees).
    /// </summary>
    Left = 1 << (Direction.Left - 1),
    /// <summary>
    /// Step to the lower left (225 degrees).
    /// </summary>
    DownLeft = 1 << (Direction.DownLeft - 1),
    /// <summary>
    /// Downwards step. (270 degrees).
    /// </summary>
    Down = 1 << (Direction.Down - 1),
    /// <summary>
    /// Step to the lower right (315 degrees).
    /// </summary>
    DownRight = 1 << (Direction.DownRight - 1),

    /// <summary>
    /// All directions.
    /// </summary>
    All = Right | UpRight | Up | UpLeft | Left | DownLeft | Down | DownRight,
}
