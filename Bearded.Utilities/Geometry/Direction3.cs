using OpenTK;

namespace Bearded.Utilities.Geometry
{
    /// <summary>
    /// A typesafe representation of a direction in three dimensional space.
    /// </summary>
    public struct Direction3
    {
        // TODO

        public static Direction3 Of(Vector3 vector) => new Direction3();

        public Vector3 Vector => Vector3.Zero;
    }
}
