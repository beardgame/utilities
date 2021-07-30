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
        public Direction2 End { get; }
        public bool IsShortArc { get; }

        public bool IsLongArc => !IsShortArc;
        public Angle Angle => IsShortArc ? End - Start : oppositeAngle(End - Start);
        public Vector2 StartPoint => Center + Radius * Start.Vector;
        public Vector2 EndPoint => Center + Radius * End.Vector;

        public float ArcLength => Angle.Radians * Radius;

        /// <summary>
        /// Returns the major arc if this arc is the minor arc, and returns the major arc if this is the minor arc.
        /// </summary>
        public CircularArc2 Opposite => new CircularArc2(Center, Radius, Start, End, !IsShortArc);

        /// <summary>
        /// Returns the same arc with the start and end directions swapped.
        /// </summary>
        public CircularArc2 Reversed => new CircularArc2(Center, Radius, End, Start, IsShortArc);

        /// <summary>
        /// Constructs the minor arc in the circle with specified center and radius between the given directions.
        /// </summary>
        public static CircularArc2 ShortArcBetweenDirections(
            Vector2 center, float radius, Direction2 start, Direction2 end)
        {
            return new CircularArc2(center, radius, start, end, true);
        }

        /// <summary>
        /// Constructs the major arc in the circle with specified center and radius between the given directions.
        /// </summary>
        public static CircularArc2 LongArcBetweenDirections(
            Vector2 center, float radius, Direction2 start, Direction2 end)
        {
            return new CircularArc2(center, radius, start, end, false);
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

            return new CircularArc2(center, radius, start, start + angle, angle.MagnitudeInRadians > MathConstants.Pi);
        }

        private CircularArc2(Vector2 center, float radius, Direction2 start, Direction2 end, bool isShortArc)
        {
            Center = center;
            Radius = radius;
            Start = start;
            End = end;
            IsShortArc = isShortArc;
        }

        public bool Equals(CircularArc2 other) =>
            Center.Equals(other.Center)
            && Radius.Equals(other.Radius)
            && Start.Equals(other.Start)
            && End.Equals(other.End)
            && IsShortArc.Equals(other.IsShortArc);

        public override bool Equals(object? obj) => obj is CircularArc2 other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Center, Radius, Start, End, IsShortArc);

        public static bool operator ==(CircularArc2 left, CircularArc2 right) => left.Equals(right);
        public static bool operator !=(CircularArc2 left, CircularArc2 right) => !left.Equals(right);

        public override string ToString() => $"{Center} × {Radius}; {arcString} {Start}-{End}";

        private string arcString => IsShortArc ? "short arc" : "long arc";

        private static Angle oppositeAngle(Angle original)
        {
            var originalMagnitude = original.Abs();
            var oppositeMagnitude = MathConstants.TwoPi.Radians() - originalMagnitude;

            var originalDirection = original.Sign();
            var oppositeDirection = -originalDirection;

            return oppositeDirection * oppositeMagnitude;
        }
    }
}
