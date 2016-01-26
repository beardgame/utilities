using System;
using OpenTK;

namespace Bearded.Utilities.SpaceTime
{
    struct Position2 : IBackedBy<Vector2>, IEquatable<Position2>
    {
        private readonly Vector2 value;

        public Position2(Vector2 value)
        {
            this.value = value;
        }

        public Vector2 NumericValue { get { return this.value; } }

        #region methods

        #region equality and hashcode

        public bool Equals(Position2 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Position2 && this.Equals((Position2)obj);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        #endregion

        #endregion

        #region operators

        #region Difference2 interaction

        public static Position2 operator +(Position2 p, Difference2 d)
        {
            return new Position2(p.value + d.NumericValue);
        }
        public static Position2 operator +(Difference2 d, Position2 p)
        {
            return new Position2(p.value + d.NumericValue);
        }
        public static Position2 operator -(Position2 p, Difference2 d)
        {
            return new Position2(p.value - d.NumericValue);
        }
        public static Difference2 operator -(Position2 p0, Position2 p1)
        {
            return new Difference2(p0.value - p1.value);
        }

        #endregion

        #region comparision

        public static bool operator ==(Position2 p0, Position2 p1)
        {
            return p0.value == p1.value;
        }

        public static bool operator !=(Position2 p0, Position2 p1)
        {
            return p0.value != p1.value;
        }

        #endregion

        #endregion

    }
}