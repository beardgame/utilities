namespace Bearded.Utilities;

public static class Do
{
    public static Box<T> Box<T>(T value)
        where T : struct
        => new Box<T>(value);
}
