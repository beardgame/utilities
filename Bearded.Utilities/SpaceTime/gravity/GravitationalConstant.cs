using System;

namespace Bearded.Utilities.SpaceTime;

public readonly struct GravitationalConstant
{
    private readonly float value;

    public GravitationalConstant(float value)
    {
        this.value = value;
    }

    #region properties

    public float NumericValue => value;

    public static GravitationalConstant Zero => new GravitationalConstant(0);

    public static GravitationalConstant One => new GravitationalConstant(1);

    #endregion

    #region accelerations

    public Acceleration2 AccelerationTowards(Mass mass, Difference2 differenceToOther)
    {
        var distanceSquared = differenceToOther.LengthSquared;
        var directionVector = differenceToOther / distanceSquared.Sqrt();
        var acceleration = AccelerationAtDistance(mass, distanceSquared);

        return acceleration * directionVector;
    }

    public Acceleration AccelerationTowards(Mass mass, Unit differenceToOther)
    {
        var directionVector = Math.Sign(differenceToOther.NumericValue);
        var acceleration = AccelerationAtDistance(mass, differenceToOther);

        return acceleration * directionVector;
    }

    public Acceleration AccelerationAtDistance(Mass mass, Difference2 differenceToOther)
        => AccelerationAtDistance(mass, differenceToOther.LengthSquared);

    public Acceleration AccelerationAtDistance(Mass mass, Unit distance)
        => AccelerationAtDistance(mass, distance.Squared);

    public Acceleration AccelerationAtDistance(Mass mass, Squared<Unit> distanceSquared)
        => new Acceleration(value * mass.NumericValue / distanceSquared.NumericValue);

    #endregion

}
