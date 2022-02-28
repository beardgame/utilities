namespace Bearded.Utilities.SpaceTime;

public static class AddDimensionExtensions
{
    public static Position3 WithZ(this Position2 pos, float z) => pos.WithZ(new Unit(z));

    public static Position3 WithZ(this Position2 pos, Unit z) => new Position3(pos.X, pos.Y, z);
}
