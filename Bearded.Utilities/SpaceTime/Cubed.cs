using System;
using System.Globalization;

namespace Bearded.Utilities.SpaceTime
{
    /// <summary>
    /// Represents a type-safe cubed value, backed by a float.
    /// </summary>
    /// <typeparam name="T">The cubed type.</typeparam>
    public struct Cubed<T> : IEquatable<Cubed<T>>, IComparable<Cubed<T>>, IFormattable
        where T : struct
    {
        private readonly float value;

        #region construcing

        internal Cubed(float value)
        {
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of the Cubed type, from a given root value.
        /// </summary>
        public static Cubed<T> FromRoot(float root)
        {
            return new Cubed<T>(root.Cubed());
        }

        /// <summary>
        /// Creates a new instance of the Cubed type, from a given value.
        /// </summary>
        public static Cubed<T> FromValue(float value)
        {
            return new Cubed<T>(value);
        }

        #endregion

        #region properties

        /// <summary>
        /// Returns the numeric value of the cube.
        /// </summary>
        public float NumericValue => value;

        /// <summary>
        /// Returns a Cubed type of value 0.
        /// </summary>
        public static Cubed<T> Zero => new Cubed<T>(0);

        /// <summary>
        /// Returns a Cubed type of value 1.
        /// </summary>
        public static Cubed<T> One => new Cubed<T>(1);

        #endregion

        #region methods

        #region equality and hashcode

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        public bool Equals(Cubed<T> other) => value == other.value;

        public override bool Equals(object obj) => obj is Cubed<T> && Equals((Cubed<T>)obj);

        public override int GetHashCode() => value.GetHashCode();

        #endregion

        #region compare

        public int CompareTo(Cubed<T> other) => value.CompareTo(other.value);

        #endregion
        
        #region tostring

        public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider)
            => $"?{value.ToString(format, formatProvider)}?";

        #endregion

        #endregion

        #region operators

        #region algebra

        /// <summary>
        /// Adds two cubes.
        /// </summary>
        public static Cubed<T> operator +(Cubed<T> c0, Cubed<T> c1) => new Cubed<T>(c0.value + c1.value);

        #endregion

        #region ratio

        /// <summary>
        /// Divides a cube by another, returning a type-less fraction.
        /// </summary>
        public static float operator /(Cubed<T> dividend, Cubed<T> divisor) => dividend.value / divisor.value;

        #endregion

        #region comparision

        /// <summary>
        /// Compares two cubes for equality.
        /// </summary>
        public static bool operator ==(Cubed<T> c0, Cubed<T> c1) => c0.Equals(c1);

        /// <summary>
        /// Compares two cubes for inequality.
        /// </summary>
        public static bool operator !=(Cubed<T> c0, Cubed<T> c1) => !(c0 == c1);

        /// <summary>
        /// Checks if one cube is smaller than another.
        /// </summary>
        public static bool operator <(Cubed<T> c0, Cubed<T> c1) => c0.value < c1.value;

        /// <summary>
        /// Checks if one cube is larger than another.
        /// </summary>
        public static bool operator >(Cubed<T> c0, Cubed<T> c1) => c0.value > c1.value;

        /// <summary>
        /// Checks if one cube is smaller or equal to another.
        /// </summary>
        public static bool operator <=(Cubed<T> c0, Cubed<T> c1) => c0.value <= c1.value;

        /// <summary>
        /// Checks if one cube is larger or equal to another.
        /// </summary>
        public static bool operator >=(Cubed<T> c0, Cubed<T> c1) => c0.value >= c1.value;

        #endregion

        #endregion

        #region static methods

        public static Cubed<T> Min(Cubed<T> s1, Cubed<T> s2)
            => new Cubed<T>(System.Math.Min(s1.NumericValue, s2.NumericValue));

        public static Cubed<T> Max(Cubed<T> s1, Cubed<T> s2)
            => new Cubed<T>(System.Math.Max(s1.NumericValue, s2.NumericValue));

        #endregion
    }
}
