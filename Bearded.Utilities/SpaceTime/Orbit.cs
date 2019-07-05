using Bearded.Utilities.Geometry;
using OpenTK;

namespace Bearded.Utilities.SpaceTime
{
    static class Constants
    {
        public static GravitationalConstant G = GravitationalConstant.One;
    }

    sealed class Body
    {
        public Position2 Position { get; }
        public Mass Mass { get; }

        public Body(Position2 position, Mass mass)
        {
            Position = position;
            Mass = mass;
        }
    }

    sealed class Orbit
    {
        private readonly Body primary;
        private readonly Instant tPer;
        private readonly Angle longitudePer;
        private readonly float eccentricity;
        private readonly Unit semiMajorAxis;
        private readonly Unit semiMinorAxis;
        private readonly Difference2 ellipseCenterOffset;
        private readonly AngularVelocity meanAnomaly;

        public Orbit(
            Body primary, Unit periapsis, Unit apoapsis, Instant tPer, Angle longitudePer)
        {
            this.primary = primary;
            this.tPer = tPer;
            this.longitudePer = longitudePer;

            eccentricity = (apoapsis - periapsis) / (apoapsis + periapsis);
            semiMajorAxis = 0.5f * (periapsis + apoapsis);
            semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - eccentricity.Squared());
            ellipseCenterOffset = (periapsis - semiMajorAxis) * Vector2.UnitX;
            var orbitalPeriod = Mathf.TwoPi * (semiMajorAxis.Cubed / (Constants.G * primary.Mass)).Sqrt();
            meanAnomaly = Angle.FromRadians(Mathf.TwoPi) / orbitalPeriod;
        }

        public Position2 PositionAtTime(Instant currentTime)
        {
            // Calculate the true anomaly using Kepler's equation.
            var trueAnomaly = trueAnomalyAtTime(currentTime);

            // We calculate the distance vector from the primary to the satellite
            // under the assumption that longitude of periapsis is 0.
            var preRotatedOffset = ellipseCenterOffset + new Difference2(
                                       semiMajorAxis * trueAnomaly.Cos(),
                                       semiMinorAxis * trueAnomaly.Sin());

            // We then rotate the point around based on the longitude of periapsis.
            var rotatedOffset = preRotatedOffset.Rotated(longitudePer);

            // Finally, we translate this offset to be in the space of the primary.
            return primary.Position + rotatedOffset;
        }

        private Angle trueAnomalyAtTime(Instant currentTime)
        {
            const int numIterations = 30; // Change depending on expected eccentricities.

            // Pre-calculate the current mean motion because it doesn't change.
            var M = (currentTime - tPer) * meanAnomaly;
            // Make an initial guess based on the eccentricity of the orbit.
            var E = eccentricity > 0.8 ? Angle.FromRadians(Mathf.Pi) : M;

            for (var i = 0; i < numIterations; i++)
            {
                var numerator = Angle.FromRadians(eccentricity * E.Sin()) + M - E;
                var denominator = eccentricity * E.Cos() - 1;
                E -= numerator / denominator;
            }

            return E;
        }
    }
}
