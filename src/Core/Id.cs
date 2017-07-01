using System;
using System.Runtime.InteropServices;

namespace Bearded.Utilities
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Id<T> : IEquatable<Id<T>>
    {
        private readonly int value;

        public int Value => value;

        public Id(int value)
        {
            this.value = value;
        }

        public bool IsValid => value != 0;
        public static Id<T> Invalid => new Id<T>(0);

        public override int GetHashCode() => value.GetHashCode();
        public bool Equals(Id<T> other) => value.Equals(other.value);
        public override bool Equals(object obj) => obj is Id<T> && Equals((Id<T>)obj);
        public static bool operator ==(Id<T> left, Id<T> right) => left.Equals(right);
        public static bool operator !=(Id<T> left, Id<T> right) => !(left == right);
    }
}
