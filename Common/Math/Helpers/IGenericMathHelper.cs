namespace MRL.SSL.Common.Math.Helpers
{
    public interface IGenericMathHelper<T>
    {
        T Zero { get; }
        T One { get; }
        T NegativeOne { get; }
        T PI { get; }

        T Random();
        T Random(T min, T max);
        T Max(T a, T b);
        T Bound(T x, T min, T max);
        T Abs(T a);
        /// <returns>x power y (x^y)</returns>
        T Pow(T x, T y);
        /// <returns>x * x (x^2)</returns>
        T Square(T x);
        T Sqrt(T a);
        T Cos(T a);
        T Sin(T a);
        T Atan2(T y,T x);
        T Radian2Degree(T radian);
        /// <returns>-x</returns>
        T Negative(T x);

        /// <returns>a + b</returns>
        T Sum(T a, T b);
        /// <returns>a - b</returns>
        T Sub(T a, T b);
        /// <returns>a / b</returns>
        T Divide(T a, T b);
        /// <returns>a * b</returns>
        T Multi(T a, T b);

        /// <returns>a == b</returns>
        bool Equal(T a, T b);
        /// <returns>a == 0</returns>
        bool EqualZero(T a);
        /// <returns>a > b</returns>
        bool Greater(T a, T b);
        /// <returns>a > 0</returns>
        bool GreaterThanZero(T a);
        /// <returns>a >= b</returns>
        bool GreaterOrEqual(T a, T b);
        /// <returns>a >= 0</returns>
        bool GreaterOrEqualThanZero(T a);
        /// <returns>a < b</returns>
        bool Less(T a, T b);
        /// <returns>a < 0</returns>
        bool LessThanZero(T a);
        /// <returns>a <= b</returns>
        bool LessOrEqual(T a, T b);
        /// <returns>a <= 0</returns>
        bool LessOrEqualThanZero(T a);
    }
}