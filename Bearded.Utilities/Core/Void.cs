using System;

namespace Bearded.Utilities;

public readonly struct Void : IComparable<Void>, IEquatable<Void>
{
    // Behold its power http://xkcd.com/1486/

    public int CompareTo(Void _) => 0;
    public bool Equals(Void _) => true;
    public override bool Equals(object? obj) => obj is Void;
    public override int GetHashCode() => 0;

    // ReSharper disable UnusedParameter.Global
    public static bool operator ==(Void _, Void __) => true;
    public static bool operator !=(Void _, Void __) => false;
    public static bool operator >=(Void _, Void __) => true;
    public static bool operator <=(Void _, Void __) => true;
    public static bool operator >(Void _, Void __) => false;
    public static bool operator <(Void _, Void __) => false;
    // ReSharper restore UnusedParameter.Global
}
