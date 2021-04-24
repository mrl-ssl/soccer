using System;

namespace MRL.SSL.Common.Math.Helpers
{
    public class MathHelper
    {
        public const float EpsilonF = 1e-8F;

        private static readonly IGenericMathHelper<int> intMathHelper = new IntMathHelper();
        private static readonly IGenericMathHelper<float> floatMathHelper = new FloatMathHelper();
        private static readonly IGenericMathHelper<double> doubleMathHelper = new DoubleMathHelper();

        public static IGenericMathHelper<T> GetGenericMathHelper<T>()
        {
            var type = typeof(T);
            if (type == typeof(int))
                return (IGenericMathHelper<T>)intMathHelper;
            if (type == typeof(float))
                return (IGenericMathHelper<T>)floatMathHelper;
            if (type == typeof(double))
                return (IGenericMathHelper<T>)doubleMathHelper;
            throw new Exception($"There is no implement for IGenericMathHelper by given type {type}");
        }

        public static bool EqualFloat(float a, float b) => MathF.Abs(a - b) <= EpsilonF;

        public static float BoundF(float x, float min, float max)
        {
            if (x < min || EqualFloat(x, min)) return min;
            if (x > max || EqualFloat(x, max)) return max;
            return x;
        }
    }
}