using OpenTK.Mathematics;

namespace Bearded.Utilities;

public static class Interpolate
{
    #region Bezier 2D
    /// <summary>
    /// Performs a first order Bezier curve interpolation.
    /// </summary>
    public static Vector2 Bezier(Vector2 p0, Vector2 p1, float t)
    {
        if (t <= 0)
            return p0;
        if (t >= 1)
            return p1;

        return bezier(p0, p1, t);
    }

    private static Vector2 bezier(Vector2 p0, Vector2 p1, float t)
        => p0 + (p1 - p0) * t;

    /// <summary>
    /// Performs a second order Bezier curve interpolation.
    /// </summary>
    public static Vector2 Bezier(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        if (t <= 0)
            return p0;
        if (t >= 1)
            return p2;

        return bezier(p0, p1, p2, t);
    }

    private static Vector2 bezier(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        => bezier(bezier(p0, p1, t), bezier(p1, p2, t), t);

    /// <summary>
    /// Performs a third order Bezier curve interpolation.
    /// </summary>
    public static Vector2 Bezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        if (t <= 0)
            return p0;
        if (t >= 1)
            return p3;

        return bezier(p0, p1, p2, p3, t);
    }

    private static Vector2 bezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        => bezier(bezier(p0, p1, p2, t), bezier(p1, p2, p3, t), t);

    #endregion

    #region Bezier 3D
    /// <summary>
    /// Performs a first order Bezier curve interpolation.
    /// </summary>
    public static Vector3 Bezier(Vector3 p0, Vector3 p1, float t)
    {
        if (t <= 0)
            return p0;
        if (t >= 1)
            return p1;

        return bezier(p0, p1, t);
    }

    private static Vector3 bezier(Vector3 p0, Vector3 p1, float t)
        => p0 + (p1 - p0) * t;

    /// <summary>
    /// Performs a second order Bezier curve interpolation.
    /// </summary>
    public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        if (t <= 0)
            return p0;
        if (t >= 1)
            return p2;

        return bezier(p0, p1, p2, t);
    }

    private static Vector3 bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        => bezier(bezier(p0, p1, t), bezier(p1, p2, t), t);

    /// <summary>
    /// Performs a third order Bezier curve interpolation.
    /// </summary>
    public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        if (t <= 0)
            return p0;
        if (t >= 1)
            return p3;

        return bezier(p0, p1, p2, p3, t);
    }

    private static Vector3 bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        => bezier(bezier(p0, p1, p2, t), bezier(p1, p2, p3, t), t);

    #endregion

    #region Smooth
    /// <summary>
    /// Performs a clamped Hermite spline interpolation.
    /// </summary>
    public static float Hermite(float from, float fromTangent, float to, float toTangent, float t)
    {
        if (t <= 0)
            return from;
        if (t >= 1)
            return to;

        var d = from - to;

        return ((
                    (2 * d + fromTangent + toTangent) * t
                    - 3 * d - 2 * fromTangent - toTangent) * t +
                fromTangent) * t +
            from;
    }

    /// <summary>
    /// Performs a cubic interpolation between two values.
    /// </summary>
    public static float SmoothStep(float from, float to, float t)
    {
        if (t <= 0)
            return from;
        if (t >= 1)
            return to;

        return (2 * t - 3) * t * t * (from - to) + from;
    }
    #endregion

    #region Linear
    /// <summary>
    /// Performs a linear interpolation between two values.
    /// </summary>
    public static float Lerp(float from, float to, float t)
    {
        if (t <= 0)
            return from;
        if (t >= 1)
            return to;

        return from + (to - from) * t;
    }

    /// <summary>
    /// Performs a bilinear interpolation between four values.
    /// </summary>
    public static float BiLerp(float value00, float value10, float value01, float value11, float u, float v)
    {
        float first, second;

        if (u <= 0)
        {
            first = value00;
            second = value01;
        }
        else if (u >= 1)
        {
            first = value10;
            second = value11;
        }
        else
        {
            first = value00 + (value10 - value00) * u;
            second = value01 + (value11 - value01) * u;
        }

        if (v <= 0)
            return first;
        if (v >= 1)
            return second;

        return first + (second - first) * v;
    }

    /// <summary>
    /// Performs a bilinear interpolation between four values.
    /// </summary>
    public static Vector2 BiLerp(Vector2 value00, Vector2 value10, Vector2 value01, Vector2 value11, float u, float v)
    {
        Vector2 first, second;

        if (u <= 0)
        {
            first = value00;
            second = value01;
        }
        else if (u >= 1)
        {
            first = value10;
            second = value11;
        }
        else
        {
            first = value00 + (value10 - value00) * u;
            second = value01 + (value11 - value01) * u;
        }

        if (v <= 0)
            return first;
        if (v >= 1)
            return second;

        return first + (second - first) * v;
    }
    #endregion
}
