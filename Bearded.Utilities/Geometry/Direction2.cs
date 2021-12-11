using System;
using System.Globalization;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Geometry;

/// <summary>
/// A typesafe representation of a direction in two dimensional space.
/// </summary>
public readonly struct Direction2 : IEquatable<Direction2>, IFormattable
{
    private const long numSteps = (1L << 32);

    private const float fromRadians = numSteps / MathConstants.TwoPi;
    private const float toRadians = MathConstants.TwoPi / numSteps;

    private const float fromDegrees = numSteps / 360f;
    private const float toDegrees = 360f / numSteps;

    private readonly uint data;

    #region Constructing

    private Direction2(uint data)
    {
        this.data = data;
    }

    /// <summary>
    /// Initialises a direction from an absolute angle value in radians.
    /// </summary>
    public static Direction2 FromRadians(float radians)
    {
        return new Direction2((uint)(radians * fromRadians));
    }

    /// <summary>
    /// Initialises a direction from an absolute angle value in degrees.
    /// </summary>
    public static Direction2 FromDegrees(float degrees)
    {
        return new Direction2((uint)(degrees * fromDegrees));
    }

    /// <summary>
    /// Initialises a direction along a vector.
    /// </summary>
    public static Direction2 Of(Vector2 vector)
    {
        return FromRadians(MathF.Atan2(vector.Y, vector.X));
    }

    /// <summary>
    /// Initialises the direction between two points.
    /// </summary>
    /// <param name="from">The base point.</param>
    /// <param name="to">The point the directions "points" towards.</param>
    public static Direction2 Between(Vector2 from, Vector2 to)
    {
        return Of(to - from);
    }

    #endregion


    #region Static Fields

    /// <summary>
    /// Default base direction (along positive X axis).
    /// </summary>
    public static readonly Direction2 Zero = new Direction2(0);

    #endregion


    #region Properties

    /// <summary>
    /// Gets the absolute angle of the direction in radians between 0 and 2pi.
    /// </summary>
    public float Radians { get { return data * toRadians; } }

    /// <summary>
    /// Gets the absolute angle of the direction in degrees between 0 and 360.
    /// </summary>
    public float Degrees { get { return data * toDegrees; } }

    /// <summary>
    /// Gets the absolute angle of the direction in radians between -pi and pi.
    /// </summary>
    public float RadiansSigned { get { return (int)data * toRadians; } }

    /// <summary>
    /// Gets the absolute angle of the direction in degrees between -180 and 180.
    /// </summary>
    public float DegreesSigned { get { return (int)data * toDegrees; } }

    /// <summary>
    /// Gets the unit vector pointing in this direction.
    /// </summary>
    public Vector2 Vector
    {
        get
        {
            var radians = Radians;
            return new Vector2(MathF.Cos(radians), MathF.Sin(radians));
        }
    }

    #endregion


    #region Methods

    /// <summary>
    /// Returns this direction turnen towards a goal direction with a given maximum step length in radians.
    /// This will never overshoot the goal.
    /// </summary>
    /// <param name="goal">The goal direction.</param>
    /// <param name="maxStepInRadians">The maximum step length in radians. Negative values will return the original direction.</param>
    public Direction2 TurnedTowards(Direction2 goal, float maxStepInRadians)
    {
        if (maxStepInRadians <= 0)
            return this;

        var step = maxStepInRadians.Radians();

        var thisToGoal = goal - this;

        if (step > thisToGoal.Abs())
            return goal;

        step *= thisToGoal.Sign();

        return this + step;
    }

    #region Statics

    /// <summary>
    /// Linearly interpolates between two directions.
    /// This always interpolates along the shorter arc.
    /// </summary>
    /// <param name="d0">The first direction (at p == 0).</param>
    /// <param name="d1">The second direction (at p == 1).</param>
    /// <param name="p">The parameter.</param>
    public static Direction2 Lerp(Direction2 d0, Direction2 d1, float p)
    {
        return d0 + p * (d1 - d0);
    }

    #endregion

    #endregion


    #region Operators

    #region Arithmetic

    /// <summary>
    /// Adds an angle to a direction.
    /// </summary>
    public static Direction2 operator +(Direction2 direction, Angle angle)
    {
        return new Direction2((uint)(direction.data + angle.Radians * fromRadians));
    }

    /// <summary>
    /// Substracts an angle from a direction.
    /// </summary>
    public static Direction2 operator -(Direction2 direction, Angle angle)
    {
        return new Direction2((uint)(direction.data - angle.Radians * fromRadians));
    }

    /// <summary>
    /// Gets the signed difference between two directions.
    /// Always returns the angle of the shorter arc.
    /// </summary>
    public static Angle operator -(Direction2 direction1, Direction2 direction2)
    {
        return Angle.FromRadians(((int)direction1.data - (int)direction2.data) * toRadians);
    }

    /// <summary>
    /// Gets the inverse direction to a direction.
    /// </summary>
    public static Direction2 operator -(Direction2 direction)
    {
        return new Direction2(direction.data + (uint.MaxValue / 2 + 1));
    }

    #endregion

    #region Boolean

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
    /// </returns>
    public bool Equals(Direction2 other)
    {
        return data == other.data;
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
    /// <returns>
    ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        return obj is Direction2 direction2 && Equals(direction2);
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public override int GetHashCode()
    {
        return data.GetHashCode();
    }

    /// <summary>
    /// Checks two directions for equality.
    /// </summary>
    public static bool operator ==(Direction2 x, Direction2 y)
    {
        return x.Equals(y);
    }

    /// <summary>
    /// Checks two directions for inequality.
    /// </summary>
    public static bool operator !=(Direction2 x, Direction2 y)
    {
        return !(x == y);
    }

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"{Radians.ToString(format, formatProvider)} rad";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #region Casts

    #endregion

    #endregion

}
