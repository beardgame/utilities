namespace Bearded.Utilities.Geometry;

public static class GeometryExtensions
{
    /// <summary>
    /// Converts floating point value into a type safe angle representation in radians.
    /// </summary>
    public static Angle Radians(this float value) => Angle.FromRadians(value);

    /// <summary>
    /// Converts floating point value into a type safe angle representation in degrees.
    /// </summary>
    public static Angle Degrees(this float value) => Angle.FromDegrees(value);

    /// <summary>
    /// Converts an integer value into a type safe angle representation in degrees.
    /// </summary>
    public static Angle Degrees(this int value) => Angle.FromDegrees(value);
}
