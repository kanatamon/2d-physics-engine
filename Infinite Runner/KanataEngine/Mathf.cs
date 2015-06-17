using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace Infinite_Runner.KanataEngine
{
    static class Mathf
    {
        const float k_biasRelative = 0.95f;
        const float k_biasAbolute = 0.01f;

        public const float EPSILON = 0.0001f;
        public const float PI = 3.141592741f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Cross(float a, Vector2 v)
        {
            return new Vector2(-a * v.Y, a * v.X);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Cross(Vector2 v, float a)
        {
            return new Vector2(a * v.Y, -a * v.X);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cross(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sqr(float a)
        {
            return a * a;
        }
 
        // Comparison with tolerance of EPSILON
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equal(float a, float b)
        {
            // <= instead of < for NaN comparison safety
            return Math.Abs(a - b) <= EPSILON;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BiasGreaterThan(float a, float b)
        {
            return a >= b * k_biasRelative + a * k_biasAbolute;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistSqr(Vector2 a, Vector2 b)
        {
            Vector2 c = a - b;
            return Vector2.Dot(c, c);
        }

    }
}
