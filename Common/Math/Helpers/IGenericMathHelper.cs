namespace MRL.SSL.Common.Math.Helpers
{
    public interface IGenericMathHelper<T>
    {
        T Zero { get; }
        T One { get; }
        T NegativeOne { get; }
        
        T Random();
        T Random(T min, T max);
        T Abs(T a);

        /// <returns>a + b</returns>
        T Sum(T a, T b);
        /// <returns>a - b</returns>
        T Sub(T a, T b);
        /// <returns>a / b</returns>
        T Dvide(T a, T b);
        /// <returns>a * b</returns>
        T Multi(T a, T b);
        
        /// <returns>a == b</returns>
        bool Equal(T a, T b);
        /// <returns>a > b</returns>
        bool Greater(T a, T b);
        /// <returns>a >= b</returns>
        bool GreaterOrEqual(T a, T b);
        /// <returns>a < b</returns>
        bool Less(T a, T b);
        /// <returns>a <= b</returns>
        bool LessOrEqual(T a, T b);
    }
}