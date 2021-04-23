namespace MRL.SSL.Common.Math.Helpers
{
    public class FloatMathHelper : IGenericMathHelper<float>
    {
        System.Random random = new System.Random(System.DateTime.Now.Millisecond);

        public float Zero => 0f;
        public float One => 1f;
        public float NegativeOne => -1f;

        public float Random() => random.Next();
        public float Random(float min, float max) => (float)(random.NextDouble() * (max - min) + min);
        public float Abs(float a) => System.MathF.Abs(a);

        public float Sum(float a, float b) => a + b;
        public float Sub(float a, float b) => a - b;
        public float Dvide(float a, float b) => a / b;
        public float Multi(float a, float b) => a * b;

        public bool Equal(float a, float b) => System.MathF.Abs(a - b) <= MathHelper.EpsilonF;
        public bool Greater(float a, float b) => a > b;
        public bool GreaterOrEqual(float a, float b) => a >= b;
        public bool Less(float a, float b) => a < b;
        public bool LessOrEqual(float a, float b) => a <= b;
    }
}