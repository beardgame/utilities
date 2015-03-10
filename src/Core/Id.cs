using System;
using System.Threading;

namespace Bearded.Utilities
{
    /// <summary>
    /// Represents a type safe ID for a given type.
    /// Uses a 32 bit integer and thus only guarantees uint.MaxValue unique IDs.
    /// </summary>
    public struct Id<T> : IEquatable<Id<T>>
    {
// ReSharper disable once StaticFieldInGenericType
        private static int counter;

        /// <summary>
        /// Returns a new id.
        /// This call is not thread safe. Never use this together with NextThreadSafe().
        /// </summary>
        public static Id<T> Next()
        {
            return new Id<T>(++counter);
        }

        /// <summary>
        /// Returns a new id.
        /// This call is thread safe. Never use this together with Next().
        /// </summary>
        public static Id<T> NextThreadSafe()
        {
            return new Id<T>(Interlocked.Increment(ref counter));
        }

        private readonly int value;

        private Id(int value)
        {
            this.value = value;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public bool Equals(Id<T> other)
        {
            return this.value.Equals(other.value);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Id<T> && this.Equals((Id<T>)obj);
        }

        /// <summary>
        /// Compares two IDs for equality.
        /// </summary>
        public static bool operator ==(Id<T> id0, Id<T> id1)
        {
            return id0.Equals(id1);
        }

        /// <summary>
        /// Compares two IDs for inequality.
        /// </summary>
        public static bool operator !=(Id<T> id0, Id<T> id1)
        {
            return !(id0 == id1);
        }
    }
}
