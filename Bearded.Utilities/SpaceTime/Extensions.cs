namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// Contains a variety of extension methods for the SpaceTime namespace.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Creates a new instance of the Unit type.
    /// </summary>
    public static Unit U(this double value) => new Unit((float)value);

    /// <summary>
    /// Creates a new instance of the Unit type.
    /// </summary>
    public static Unit U(this float value) => new Unit(value);

    /// <summary>
    /// Creates a new instance of the Unit type.
    /// </summary>
    public static Unit U(this int value) => new Unit(value);

    /// <summary>
    /// Returns the typed square root of the squared unit value.
    /// </summary>
    public static Unit Sqrt(this Squared<Unit> square) => new Unit(square.NumericValue.Sqrted());

    /// <summary>
    /// Returns the typed square root of the squared speed.
    /// </summary>
    public static Speed Sqrt(this Squared<Speed> square) => new Speed(square.NumericValue.Sqrted());

    /// <summary>
    /// Returns the typed square root of the squared acceleration.
    /// </summary>
    public static Acceleration Sqrt(this Squared<Acceleration> square) => new Acceleration(square.NumericValue.Sqrted());
}
