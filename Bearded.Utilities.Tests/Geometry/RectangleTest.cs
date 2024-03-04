using System;
using Bearded.Utilities.Geometry;
using Bearded.Utilities.Tests.Generators;
using FluentAssertions;
using FsCheck.Xunit;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Tests.Geometry;

public sealed class RectangleTest
{
    private const float epsilon = 0.01f;

    [Property(Arbitrary = new[] { typeof(FloatGenerators.ForArithmetic) })]
    public void RectangleMadeFromXyWidthHeightHasCorrectDimensions(float x, float y, float w, float h)
    {
        if (w < 0) w *= -1;
        if (h < 0) h *= -1;

        var rectangle = new Rectangle(x, y, w, h);

        // from input
        rectangle.Left.Should().BeApproximately(x, epsilon);
        rectangle.Top.Should().BeApproximately(y, epsilon);
        rectangle.Width.Should().BeApproximately(w, epsilon);
        rectangle.Height.Should().BeApproximately(h, epsilon);

        // derived
        rectangle.Right.Should().BeApproximately(x + w, epsilon);
        rectangle.Bottom.Should().BeApproximately(y + h, epsilon);
    }

    [Property(Arbitrary = new[] { typeof(FloatGenerators.ForArithmetic) })]
    public void RectangleMadeFromSidesHasCorrectDimensions(float x1, float x2, float y1, float y2)
    {
        var top = Math.Min(y1, y2);
        var right = Math.Max(x1, x2);
        var bottom = Math.Max(y1, y2);
        var left = Math.Min(x1, x2);

        var rectangle = Rectangle.WithSides(top, right, bottom, left);

        // from input
        rectangle.Top.Should().BeApproximately(top, epsilon);
        rectangle.Right.Should().BeApproximately(right, epsilon);
        rectangle.Bottom.Should().BeApproximately(bottom, epsilon);
        rectangle.Left.Should().BeApproximately(left, epsilon);

        // derived
        rectangle.Width.Should().BeApproximately(right - left, epsilon);
        rectangle.Height.Should().BeApproximately(bottom - top, epsilon);
    }

    [Property(Arbitrary = new[] { typeof(FloatGenerators.ForArithmetic) })]
    public void RectangleMadeFromCornersHasCorrectDimensions(float x1, float x2, float y1, float y2)
    {
        var top = Math.Min(y1, y2);
        var right = Math.Max(x1, x2);
        var bottom = Math.Max(y1, y2);
        var left = Math.Min(x1, x2);

        var rectangle = Rectangle.WithCorners(new Vector2(left, top), new Vector2(right, bottom));

        // from input
        rectangle.Top.Should().BeApproximately(top, epsilon);
        rectangle.Right.Should().BeApproximately(right, epsilon);
        rectangle.Bottom.Should().BeApproximately(bottom, epsilon);
        rectangle.Left.Should().BeApproximately(left, epsilon);

        // derived
        rectangle.Width.Should().BeApproximately(right - left, epsilon);
        rectangle.Height.Should().BeApproximately(bottom - top, epsilon);
    }
}
