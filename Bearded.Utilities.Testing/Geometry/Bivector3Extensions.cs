using Bearded.Utilities.Geometry;

namespace Bearded.Utilities.Testing.Geometry;

public static class Bivector3Extensions
{
    public static Bivector3Assertions Should(this Bivector3 instance) => new Bivector3Assertions(instance);
}
