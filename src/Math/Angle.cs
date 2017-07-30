using System;
using OpenTK;

namespace Bearded.Utilities.Math
{
    /// <summary>
    /// A typesafe representation of a signed angle.
    /// </summary>
    public struct Angle : IEquatable<Angle>
    {
        private readonly float radians;

        #region Constructing

        private Angle(float radians)
        {
            this.radians = radians;
        }

        /// <summary>
        /// Initialises an angle from a relative angle value in radians.
        /// </summary>
        public static Angle FromRadians(float radians)
        {
            return new Angle(radians);
        }

        /// <summary>
        /// Initialises an angle from a relative angle value in degrees.
        /// </summary>
        public static Angle FromDegrees(float degrees)
        {
            return new Angle(Mathf.DegreesToRadians(degrees));
        }

        /// <summary>
        /// Initialises an angle as the signed difference between two directional unit vectors in the 2D plane.
        /// If the vectors are not unit length the result is undefined.
        /// </summary>
        public static Angle Between(Vector2 from, Vector2 to)
        {
            float perpDot = from.Y * to.X - from.X * to.Y;

            return Angle.FromRadians(Mathf.Atan2(perpDot, Vector2.Dot(from, to)));
        }

        /// <summary>
        /// Initialises an angle as the signed difference between two directions in the 2D plane.
        /// The returned value is the smallest possible angle from one direction to the other.
        /// </summary>
        public static Angle Between(Direction2 from, Direction2 to)
        {
            return to - from;
        }

        /// <summary>
        /// Initialises an angle as the signed difference between two directions in the 2D plane.
        /// The returned value is the smallest positive angle from one direction to the other.
        /// </summary>
        public static Angle BetweenPositive(Direction2 from, Direction2 to)
        {
            var a = Angle.Between(from, to);
            if (a.radians < 0)
                a += Mathf.TwoPi.Radians();
            return a;
        }

        /// <summary>
        /// Initialises an angle as the signed difference between two directions in the 2D plane.
        /// The returned value is the smallest negative angle from one direction to the other.
        /// </summary>
        public static Angle BetweenNegative(Direction2 from, Direction2 to)
        {
            var a = Angle.Between(from, to);
            if (a.radians > 0)
                a -= Mathf.TwoPi.Radians();
            return a;
        }

        #endregion

        #region Static Fields

        /// <summary>
        /// The default zero angle.
        /// </summary>
        public static readonly Angle Zero = new Angle(0);

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets the value of the angle in radians.
        /// </summary>
        public float Radians { get { return this.radians; } }

        /// <summary>
        /// Gets the value of the angle in degrees.
        /// </summary>
        public float Degrees { get { return Mathf.RadiansToDegrees(this.radians); } }

        /// <summary>
        /// Gets a 2x2 rotation matrix that rotates vectors by this angle.
        /// </summary>
        public Matrix2 Transformation { get { return Matrix2.CreateRotation(this.radians); } }

        /// <summary>
        /// Gets the magnitude (absolute value) of the angle in radians.
        /// </summary>
        public float MagnitudeInRadians { get { return System.Math.Abs(this.radians); } }

        /// <summary>
        /// Gets the magnitude (absolute value) of the angle in degrees.
        /// </summary>
        public float MagnitudeInDegrees { get { return System.Math.Abs(this.Degrees); } }


        #endregion
        
        #region Methods

        #region Arithmetic

        /// <summary>
        /// Returns the Sine of the angle.
        /// </summary>
        public float Sin()
        {
            return Mathf.Sin(this.radians);
        }
        /// <summary>
        /// Returns the Cosine of the angle.
        /// </summary>
        public float Cos()
        {
            return Mathf.Cos(this.radians);
        }
        /// <summary>
        /// Returns the Tangent of the angle.
        /// </summary>
        public float Tan()
        {
            return Mathf.Tan(this.radians);
        }
        /// <summary>
        /// Returns the Sign of the angle.
        /// </summary>
        public int Sign()
        {
            return System.Math.Sign(this.radians);
        }
        /// <summary>
        /// Returns the absolute value of the angle.
        /// </summary>
        public Angle Abs()
        {
            return new Angle(System.Math.Abs(this.radians));
        }
        /// <summary>
        /// Returns a new Angle with |value| == 1 radians and the same sign as this angle.
        /// Returns a new Angle with value 0 if the angle is zero.
        /// </summary>
        public Angle Normalized()
        {
            return new Angle(System.Math.Sign(this.radians));
        }
        /// <summary>
        /// Clamps this angle between a minimum and a maximum angle.
        /// </summary>
        public Angle Clamped(Angle min, Angle max)
        {
            return Angle.Clamp(this, min, max);
        }

        #region Statics

        /// <summary>
        /// Returns the larger of two angles.
        /// </summary>
        public static Angle Max(Angle a1, Angle a2)
        {
            return a1 > a2 ? a1 : a2;
        }

        /// <summary>
        /// Returns the smaller of two angles.
        /// </summary>
        public static Angle Min(Angle a1, Angle a2)
        {
            return a1 < a2 ? a1 : a2;
        }

        /// <summary>
        /// Clamps one angle between a minimum and a maximum angle.
        /// </summary>
        public static Angle Clamp(Angle a, Angle min, Angle max)
        {
            return a < max ? a > min ? a : min : max;
        }

        #endregion

        #endregion

        #endregion
        
        #region Operators

        #region Arithmetic

        /// <summary>
        /// Adds two angles.
        /// </summary>
        public static Angle operator +(Angle angle1, Angle angle2)
        {
            return new Angle(angle1.radians + angle2.radians);
        }

        /// <summary>
        /// Substracts an angle from another.
        /// </summary>
        public static Angle operator -(Angle angle1, Angle angle2)
        {
            return new Angle(angle1.radians - angle2.radians);
        }

        /// <summary>
        /// Inverts an angle.
        /// </summary>
        public static Angle operator -(Angle angle)
        {
            return new Angle(-angle.radians);
        }

        /// <summary>
        /// Multiplies an angle with a scalar.
        /// </summary>
        public static Angle operator *(Angle angle, float scalar)
        {
            return new Angle(angle.radians * scalar);
        }

        /// <summary>
        /// Multiplies an angle with a scalar.
        /// </summary>
        public static Angle operator *(float scalar, Angle angle)
        {
            return new Angle(angle.radians * scalar);
        }

        /// <summary>
        /// Divides an angle by an inverse scalar.
        /// </summary>
        public static Angle operator /(Angle angle, float invScalar)
        {
            return new Angle(angle.radians / invScalar);
        }

        /// <summary>
        /// Linearly interpolates between two angles.
        /// </summary>
        public static Angle Lerp(Angle angle0, Angle angle1, float t)
        {
            return angle0 + (angle1 - angle0) * t;
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
        public bool Equals(Angle other)
        {
            return this.radians == other.radians;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
			return obj is Angle && this.Equals((Angle)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.radians.GetHashCode();
        }

        /// <summary>
        /// Checks two angles for equality.
        /// </summary>
        public static bool operator ==(Angle x, Angle y)
        {
            return x.Equals(y);
        }
        /// <summary>
        /// Checks two angles for inequality.
        /// </summary>
        public static bool operator !=(Angle x, Angle y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Checks whether one angle is smaller than another.
        /// </summary>
        public static bool operator <(Angle x, Angle y)
        {
            return x.radians < y.radians;
        }
        /// <summary>
        /// Checks whether one angle is greater than another.
        /// </summary>
        public static bool operator >(Angle x, Angle y)
        {
            return x.radians > y.radians;
        }

        /// <summary>
        /// Checks whether one angle is smaller or equal to another.
        /// </summary>
        public static bool operator <=(Angle x, Angle y)
        {
            return x.radians <= y.radians;
        }
        /// <summary>
        /// Checks whether one angle is greater or equal to another.
        /// </summary>
        public static bool operator >=(Angle x, Angle y)
        {
            return x.radians >= y.radians;
        }

        #endregion

        #region Casts

        /// <summary>
        /// Casts an angle to a direction in the 2D plane.
        /// This is the same as Direction.Zero + angle.
        /// </summary>
        public static explicit operator Direction2(Angle angle)
        {
            return Direction2.FromRadians(angle.radians);
        }

        #endregion

        #endregion
    }
}
