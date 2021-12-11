namespace Bearded.Utilities.Tilemaps.Rectangular;

/// <summary>
/// Represents the eight directions of movement in a rectangular tilemap.
/// </summary>
public enum Direction : byte
{
    /// <summary>
    /// Unknown/no direction value. Steps in this direction have no effect.
    /// </summary>
    Unknown = 0,
    /// <summary>
    /// Step to the right (0 degrees).
    /// </summary>
    Right = 1,
    /// <summary>
    /// Step to the upper right. (45 degrees).
    /// </summary>
    UpRight = 2,
    /// <summary>
    /// Step upwards. (90 degrees).
    /// </summary>
    Up = 3,
    /// <summary>
    /// Step to the upper left (135 degrees).
    /// </summary>
    UpLeft = 4,
    /// <summary>
    /// Step to the left (180 degrees).
    /// </summary>
    Left = 5,
    /// <summary>
    /// Step to the lower left (225 degrees).
    /// </summary>
    DownLeft = 6,
    /// <summary>
    /// Downwards step. (270 degrees).
    /// </summary>
    Down = 7,
    /// <summary>
    /// Step to the lower right (315 degrees).
    /// </summary>
    DownRight = 8,
}
