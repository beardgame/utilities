using OpenTK.Mathematics;

namespace Bearded.Utilities.Tests.Assertions;

static class Vector2Extensions
{
    public static Vector2Assertions Should(this Vector2 subject) => new Vector2Assertions(subject);
}
