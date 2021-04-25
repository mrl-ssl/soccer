using System;

namespace MRL.SSL.Common.Math.Helpers
{
    public class MathHelper
    {
        public const float EpsilonF = 1e-8F;
        public const double Epsilon = 1e-8;

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
        public static bool EqualDouble(double a, double b) => System.Math.Abs(a - b) <= Epsilon;

        public static double Bound(double x, double min, double max)
        {
            if (x < min || EqualDouble(x, min)) return min;
            if (x > max || EqualDouble(x, max)) return max;
            return x;
        }
        public static float BoundF(float x, float min, float max)
        {
            if (x < min || EqualFloat(x, min)) return min;
            if (x > max || EqualFloat(x, max)) return max;
            return x;
        }
        public static int Bound(int x, int min, int max)
        {
            if (x < min) return min;
            if (x > max) return max;
            return x;
        }
    }
}