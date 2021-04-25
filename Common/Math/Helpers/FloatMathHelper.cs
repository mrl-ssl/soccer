namespace MRL.SSL.Common.Math.Helpers
{
    public class FloatMathHelper : IGenericMathHelper<float>
    {
        System.Random random = new System.Random(System.DateTime.Now.Millisecond);

        public float Zero => 0f;
        public float One => 1f;
        public float NegativeOne => -1f;
        public float PI => System.MathF.PI;

        public float Random() => (float)random.NextDouble();
        public float Random(float min, float max) => (float)(random.NextDouble() * (max - min) + min);
        public float Max(float a, float b) => System.MathF.Max(a, b);
        public float Bound(float x, float min, float max) => MathHelper.BoundF(x, min, max);
        public float Abs(float a) => System.MathF.Abs(a);
        public float Pow(float x, float y) => System.MathF.Pow(x, y);
        public float Square(float x) => x * x;
        public float Times(int times, float x) => times * x;
        public float Times(float times, float x) => times * x;
        public float Sqrt(float a) => System.MathF.Sqrt(a);
        public float Cos(float a) => System.MathF.Cos(a);
        public float ACos(float a) => System.MathF.Acos(a);
        public float Sin(float a) => System.MathF.Sin(a);
        public float Atan2(float y, float x) => System.MathF.Atan2(y, x);
        public float Radian2Degree(float radian) => radian * 180f / PI;
        public float Sign(float x) => System.MathF.Sign(x);
        public float Negative(float x) => -x;

        public float Sum(float a, float b) => a + b;
        public float Sub(float a, float b) => a - b;
        public float Divide(float a, float b) => a / b;
        public float Multi(float a, float b) => a * b;

        public bool Equal(float a, float b) => System.MathF.Abs(a - b) <= MathHelper.EpsilonF;
        public bool EqualZero(float a) => System.MathF.Abs(a) <= MathHelper.EpsilonF;
        public bool Greater(float a, float b) => a > b && !Equal(a, b);
        public bool GreaterThanZero(float a) => a > 0f && !EqualZero(a);
        public bool GreaterOrEqual(float a, float b) => a > b || Equal(a, b);
        public bool GreaterOrEqualThanZero(float a) => a > 0f || EqualZero(a);
        public bool Less(float a, float b) => a < b && !Equals(a, b);
        public bool LessThanZero(float a) => a < 0f && !EqualZero(a);
        public bool LessOrEqual(float a, float b) => a < b || Equal(a, b);
        public bool LessOrEqualThanZero(float a) => a < 0f || EqualZero(a);
    }
}