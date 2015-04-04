using OpenTK;

namespace Bearded.Utilities.Math
{
    /// <summary>
    /// Interpolation functions.
    /// </summary>
    public static class Interpolate
    {
        #region Bezier 2D
        /// <summary>
        /// Performs a first order Bezier curve interpolation.
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="t">The amount of interpolation (between 0 and 1).</param>
        /// <returns>The result of the Bezier curve interpolation.</returns>
        public static Vector2 Bezier(Vector2 p0, Vector2 p1, float t)
        {
            if (t <= 0)
                return p0;
            if (t >= 1)
                return p1;

            return Interpolate.bezier(p0, p1, t);
        }

        private static Vector2 bezier(Vector2 p0, Vector2 p1, float t)
        {
            return (1 - t) * p0 + t * p1;
        }

        /// <summary>
        /// Performs a second order Bezier curve interpolation.
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="t">The amount of interpolation (between 0 and 1).</param>
        /// <returns>The result of the Bezier curve interpolation.</returns>
        public static Vector2 Bezier(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            if (t <= 0)
                return p0;
            if (t >= 1)
                return p2;

            return Interpolate.bezier(p0, p1, p2, t);
        }

        private static Vector2 bezier(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            return Interpolate.bezier(Interpolate.bezier(p0, p1, t), Interpolate.bezier(p1, p2, t), t);
        }

        /// <summary>
        /// Performs a third order Bezier curve interpolation.
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="t">The amount of interpolation (between 0 and 1).</param>
        /// <returns>The result of the Bezier curve interpolation.</returns>
        public static Vector2 Bezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            if (t <= 0)
                return p0;
            if (t >= 1)
                return p3;

            return Interpolate.bezier(p0, p1, p2, p3, t);
        }

        private static Vector2 bezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            return Interpolate.bezier(Interpolate.bezier(p0, p1, p2, t), Interpolate.bezier(p1, p2, p3, t), t);
        }
        #endregion

        #region Bezier 3D
        /// <summary>
        /// Performs a first order Bezier curve interpolation.
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="t">The amount of interpolation (between 0 and 1).</param>
        /// <returns>The result of the Bezier curve interpolation.</returns>
        public static Vector3 Bezier(Vector3 p0, Vector3 p1, float t)
        {
            if (t <= 0)
                return p0;
            if (t >= 1)
                return p1;

            return Interpolate.bezier(p0, p1, t);
        }

        private static Vector3 bezier(Vector3 p0, Vector3 p1, float t)
        {
            return (1 - t) * p0 + t * p1;
        }

        /// <summary>
        /// Performs a second order Bezier curve interpolation.
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="t">The amount of interpolation (between 0 and 1).</param>
        /// <returns>The result of the Bezier curve interpolation.</returns>
        public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            if (t <= 0)
                return p0;
            if (t >= 1)
                return p2;

            return Interpolate.bezier(p0, p1, p2, t);
        }

        private static Vector3 bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return Interpolate.bezier(Interpolate.bezier(p0, p1, t), Interpolate.bezier(p1, p2, t), t);
        }

        /// <summary>
        /// Performs a third order Bezier curve interpolation.
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="t">The amount of interpolation (between 0 and 1).</param>
        /// <returns>The result of the Bezier curve interpolation.</returns>
        public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            if (t <= 0)
                return p0;
            if (t >= 1)
                return p3;

            return Interpolate.bezier(p0, p1, p2, p3, t);
        }

        private static Vector3 bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            return Interpolate.bezier(Interpolate.bezier(p0, p1, p2, t), Interpolate.bezier(p1, p2, p3, t), t);
        }
        #endregion

        #region Smooth
        /// <summary>
        /// Performs a Hermite spline interpolation.
        /// </summary>
        /// <param name="value1">From position.</param>
        /// <param name="tangent1">From tangent.</param>
        /// <param name="value2">To position.</param>
        /// <param name="tangent2">To tangent.</param>
        /// <param name="amount">The amount of interpolation.</param>
        /// <returns>The result of the Hermite spline interpolation.</returns>
        public static float Hermite(float value1, float tangent1, float value2, float tangent2, float amount)
        {
            // All transformed to double not to lose precission.
            // Otherwise, we might get NaN or Infinity result.
            double v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount, result;
            double sCubed = s * s * s;
            double sSquared = s * s;

            if (amount == 0f)
                result = value1;
            else if (amount == 1f)
                result = value2;
            else
                result = (2 * v1 - 2 * v2 + t2 + t1) * sCubed +
                    (3 * v2 - 3 * v1 - 2 * t1 - t2) * sSquared +
                    t1 * s +
                    v1;
            return (float)result;
        }

        /// <summary>
        /// Performs a cubic interpolation between two values.
        /// </summary>
        /// <param name="from">The first value.</param>
        /// <param name="to">The second value.</param>
        /// <param name="t">The amount of interpolation (between 0 and 1).</param>
        /// <returns>The interpolated value.</returns>
        public static float SmoothStep(float from, float to, float t)
        {
            if (t <= 0)
                return from;
            if (t >= 1)
                return to;

            return Interpolate.Hermite(from, 0, to, 0, t);
        }
        #endregion

        #region Linear
        /// <summary>
        /// Performs a linear interpolation between two values.
        /// </summary>
        /// <param name="from">The first value.</param>
        /// <param name="to">The second value.</param>
        /// <param name="t">The amount of interpolation (between 0 and 1).</param>
        /// <returns>The interpolated value.</returns>
        public static float Lerp(float from, float to, float t)
        {
            if (t <= 0)
                return from;
            if (t >= 1)
                return to;

            return from + (to - from) * t;
        }

        /// <summary>
        /// Performs a biliear interpolation between four values.
        /// Note: The parameters are not clamped to the 0-1 range.
        /// </summary>
        /// <param name="value00">The first value.</param>
        /// <param name="value10">The second value.</param>
        /// <param name="value01">The third value.</param>
        /// <param name="value11">The fourth value.</param>
        /// <param name="u">Parameter in first dimension</param>
        /// <param name="v">Parameter in second dimension.</param>
        /// <returns>The interpolated value.</returns>
        public static float BiLerp(float value00, float value10, float value01, float value11, float u, float v)
        {
            var first = value00 + (value10 - value00) * u;
            var second = value01 + (value11 - value01) * u;
            return first + (second - first) * v;
        }

        /// <summary>
        /// Performs a biliear interpolation between four vectors.
        /// Note: The parameters are not clamped to the 0-1 range.
        /// </summary>
        /// <param name="value00">The first value.</param>
        /// <param name="value10">The second value.</param>
        /// <param name="value01">The third value.</param>
        /// <param name="value11">The fourth value.</param>
        /// <param name="u">Parameter in first dimension</param>
        /// <param name="v">Parameter in second dimension.</param>
        /// <returns>The interpolated value.</returns>
        public static Vector2 BiLerp(Vector2 value00, Vector2 value10, Vector2 value01, Vector2 value11, float u, float v)
        {
            var first = value00 + (value10 - value00) * u;
            var second = value01 + (value11 - value01) * u;
            return first + (second - first) * v;
        }
        #endregion
    }
}
