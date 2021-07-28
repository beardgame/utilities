using System;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Geometry
{
    /// <summary>
    /// Represents an arc of a circle between a pair of distinct points.
    /// </summary>
    public readonly struct CircularArc2 : IEquatable<CircularArc2>
    {
        public Vector2 Center { get; }
        public float Radius { get; }
        public Direction2 Start { get; }
        public Angle Angle { get; }

        public Direction2 End => Start + Angle;
        public Vector2 StartPoint => Center + Radius * Start.Vector;
        public Vector2 EndPoint => Center + Radius * End.Vector;

        public float ArcLength => Angle.Radians * Radius;

        /// <summary>
        /// Returns the major arc if this arc is the minor arc, and returns the major arc if this is the minor arc.
        /// </summary>
        public CircularArc2 Opposite => new CircularArc2(Center, Radius, Start, oppositeAngle(Angle));

        /// <summary>
        /// Returns the same arc with the start and end directions swapped.
        /// </summary>
        public CircularArc2 Reversed => new CircularArc2(Center, Radius, End, -Angle);

        /// <summary>
        /// Constructs the minor arc in the circle with specified center and radius between the given directions.
        /// </summary>
        public static CircularArc2 ShortestArcBetweenDirections(
            Vector2 center, float radius, Direction2 start, Direction2 end)
        {
            return new CircularArc2(center, radius, start, end - start);
        }

        /// <summary>
        /// Constructs the major arc in the circle with specified center and radius between the given directions.
        /// </summary>
        public static CircularArc2 LongestArcBetweenDirections(
            Vector2 center, float radius, Direction2 start, Direction2 end)
        {
            return new CircularArc2(center, radius, start, oppositeAngle(end - start));
        }

        /// <summary>
        /// Constructs the arc in the circle with specified center and radius starting in the given direction,
        /// subtending the given angle.
        /// </summary>
        public static CircularArc2 FromStartAndAngle(
            Vector2 center, float radius, Direction2 start, Angle angle)
        {
            if (angle.MagnitudeInRadians > MathConstants.TwoPi)
            {
                throw new ArgumentException("Angle must be between -360° and +360° inclusive.", nameof(angle));
            }

            return new CircularArc2(center, radius, start, angle);
        }

        private CircularArc2(Vector2 center, float radius, Direction2 start, Angle angle)
        {
            Center = center;
            Radius = radius;
            Start = start;
            Angle = angle;
        }

        public bool Equals(CircularArc2 other) =>
            Center.Equals(other.Center)
            && Radius.Equals(other.Radius)
            && Start.Equals(other.Start)
            && Angle.Equals(other.Angle);

        public override bool Equals(object? obj) => obj is CircularArc2 other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Center, Radius, Start, Angle);

        public static bool operator ==(CircularArc2 left, CircularArc2 right) => left.Equals(right);
        public static bool operator !=(CircularArc2 left, CircularArc2 right) => !left.Equals(right);

        public override string ToString() =>
            string.Join(System.Environment.NewLine,
                $"Center = {Center}", $"Radius = {Radius}", $"Start = {Start}", $"Angle = {Angle}");

        private static Angle oppositeAngle(Angle original)
        {
            return -original.Sign() * (MathConstants.TwoPi.Radians() - original.Abs());
        }
    }
}
