﻿using System;
using OpenTK;

namespace Bearded.Utilities.SpaceTime
{
    /// <summary>
    /// A type-safe representation of an absolute 2d position vector.
    /// </summary>
    public struct Position2 : IBackedBy<Vector2>, IEquatable<Position2>
    {
        private readonly Vector2 value;

        #region construction

        /// <summary>
        /// Creates a new instance of the Position2 type.
        /// </summary>
        public Position2(Vector2 value)
        {
            this.value = value;
        }
        /// <summary>
        /// Creates a new instance of the Position2 type.
        /// </summary>
        public Position2(float x, float y)
            : this(new Vector2(x, y))
        {
        }
        /// <summary>
        /// Creates a new instance of the Position2 type.
        /// </summary>
        public Position2(Unit x, Unit y)
            : this(new Vector2(x.NumericValue, y.NumericValue))
        {
        }

        #endregion

        /// <summary>
        /// Returns the numeric vector value of the position vector.
        /// </summary>
        public Vector2 NumericValue { get { return this.value; } }

        /// <summary>
        /// Returns the X component of the position vector.
        /// </summary>
        public Unit X { get { return new Unit(this.value.X); } }
        /// <summary>
        /// Returns the Y component of the position vector.
        /// </summary>
        public Unit Y { get { return new Unit(this.value.Y); } }

        /// <summary>
        /// Returns a Position2 type with value 0.
        /// </summary>
        public Position2 Zero { get { return new Position2(0, 0); } }

        #region methods

        #region lerp

        /// <summary>
        /// Linearly interpolates between two typed position vectors.
        /// </summary>
        /// <param name="p0">The position vector at t = 0.</param>
        /// <param name="p1">The position vector at t = 1.</param>
        /// <param name="t">The interpolation scalar.</param>
        public static Position2 Lerp(Position2 p0, Position2 p1, float t)
        {
            return p0 + (p1 - p0) * t;
        }

        /// <summary>
        /// Linearly interpolates towards another typed position vector.
        /// </summary>
        /// <param name="p">The position vector at t = 1.</param>
        /// <param name="t">The interpolation scalar.</param>
        public Position2 LerpTo(Position2 p, float t)
        {
            return Lerp(this, p, t);
        }

        #endregion

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

        /// <summary>
        /// Adds a difference vector to an absolute position.
        /// </summary>
        public static Position2 operator +(Position2 p, Difference2 d)
        {
            return new Position2(p.value + d.NumericValue);
        }
        /// <summary>
        /// Adds a difference vector to an absolute position.
        /// </summary>
        public static Position2 operator +(Difference2 d, Position2 p)
        {
            return new Position2(p.value + d.NumericValue);
        }
        /// <summary>
        /// Subtracts a difference vector from an absolute position.
        /// </summary>
        public static Position2 operator -(Position2 p, Difference2 d)
        {
            return new Position2(p.value - d.NumericValue);
        }
        /// <summary>
        /// Subtracts two absolute positions, returning a difference vector.
        /// </summary>
        public static Difference2 operator -(Position2 p0, Position2 p1)
        {
            return new Difference2(p0.value - p1.value);
        }

        #endregion

        #region comparision

        /// <summary>
        /// Compares two position vectors for equality.
        /// </summary>
        public static bool operator ==(Position2 p0, Position2 p1)
        {
            return p0.value == p1.value;
        }
        /// <summary>
        /// Compares two position vectors for inequality.
        /// </summary>
        public static bool operator !=(Position2 p0, Position2 p1)
        {
            return p0.value != p1.value;
        }

        #endregion

        #endregion

    }
}