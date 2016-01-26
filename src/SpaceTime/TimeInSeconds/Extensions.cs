using Bearded.Utilities.Math;

namespace Bearded.Utilities.SpaceTime.TimeInSeconds
{
    static class Extensions
    {
        public static Speed PerSecond(this Unit unit)
        {
            return new Speed(unit.NumericValue);
        }
        public static Acceleration PerSecond(this Speed speed)
        {
            return new Acceleration(speed.NumericValue);
        }

        public static Velocity2 PerSecond(this Difference2 difference)
        {
            return new Velocity2(difference.NumericValue);
        }
        public static Acceleration2 PerSecond(this Velocity2 velocity)
        {
            return new Acceleration2(velocity.NumericValue);
        }

        public static AngularVelocity PerSecond(this Angle angle)
        {
            return AngularVelocity.FromRadians(angle.Radians);
        }
        public static AngularAcceleration PerSecond(this AngularVelocity velocity)
        {
            return AngularAcceleration.FromRadians(velocity.NumericValue);
        }
    }
}