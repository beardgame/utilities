using OpenTK.Mathematics;

namespace Bearded.Utilities.Tests.Assertions
{
    static class Vector3Extensions
    {
        public static Vector3Assertions Should(this Vector3 subject) => new Vector3Assertions(subject);
    }
}
