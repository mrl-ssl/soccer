using System;
using System.Runtime.CompilerServices;

namespace MRL.SSL.Common.Math
{
    public class MathHelper
    {
        public const float EpsilonF = 1e-7F;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static bool EqualFloat(float a, float b) => MathF.Abs(a - b) <= EpsilonF;
        public static bool GreaterThan(float a, float b) => a - b > -EpsilonF;
        public static bool LessThan(float a, float b) => a - b < EpsilonF;
        public static bool LessThanEqual(float a, float b) => a - b <= EpsilonF;
        public static bool GreaterThanEqual(float a, float b) => a - b >= -EpsilonF;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float BoundF(float x, float min, float max)
        {
            if (x < min || EqualFloat(x, min)) return min;
            if (x > max || EqualFloat(x, max)) return max;
            return x;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AngleMod(float a)
        {
            var pi2 = MathF.PI * 2f;
            a -= pi2 * (int)MathF.Round(a / pi2);
            return (a);
        }
    }
}