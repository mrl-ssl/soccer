using System;

namespace MRL.SSL.Common.Math
{
    public class MathHelper
    {
        public const float EpsilonF = 1e-8F;

        public static bool EqualFloat(float a, float b) => MathF.Abs(a - b) <= EpsilonF;

        public static float BoundF(float x, float min, float max)
        {
            if (x < min || EqualFloat(x, min)) return min;
            if (x > max || EqualFloat(x, max)) return max;
            return x;
        }
    }
}