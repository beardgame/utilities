using Bearded.Utilities.Geometry;

namespace Bearded.Utilities.Testing.Geometry;

public static class Bivector2Extensions
{
    public static Bivector2Assertions Should(this Bivector2 instance) => new Bivector2Assertions(instance);
}
