﻿using System;
using System.Runtime.InteropServices;

namespace Bearded.Utilities
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Id<T> : IEquatable<Id<T>>
    {
        public int Value { get; }

        public Id(int value)
        {
            Value = value;
        }

        public bool IsValid => Value != 0;
        public static Id<T> Invalid => new Id<T>(0);

        public override int GetHashCode() => Value.GetHashCode();
        public bool Equals(Id<T> other) => Value.Equals(other.Value);
        public override bool Equals(object obj) => obj is Id<T> && Equals((Id<T>)obj);
        public static bool operator ==(Id<T> left, Id<T> right) => left.Equals(right);
        public static bool operator !=(Id<T> left, Id<T> right) => !(left == right);
    }
}
