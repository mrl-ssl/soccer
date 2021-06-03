using System;
using System.Runtime.CompilerServices;

namespace MRL.SSL.Common.Math
{
    public class MathHelper
    {
        public const float EpsilonF = 1e-7F;
        public const float PI2 = MathF.PI * 2f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualFloat(float a, float b) => MathF.Abs(a - b) <= EpsilonF;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThan(float a, float b) => a - b > -EpsilonF;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThan(float a, float b) => a - b < EpsilonF;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanEqual(float a, float b) => a - b <= EpsilonF;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanEqual(float a, float b) => a - b >= -EpsilonF;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float BoundF(float x, float min, float max)
        {
            if (x < min) return min;
            if (x > max) return max;
            return x;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float BoundAbsF(float x, float bound)
        {
            if (x < -bound) return -bound;
            if (x > bound) return bound;
            return x;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AngleMod(float a)
        {
            return a - PI2 * (int)MathF.Round(a / PI2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float CopySign(float a, float b)
        {
            return MathF.Abs(a) * MathF.Sign(b);
        }


    }
}