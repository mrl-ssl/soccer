namespace MRL.SSL.Common.Math.Helpers
{
    public class IntMathHelper : IGenericMathHelper<int>
    {
        System.Random random = new System.Random(System.DateTime.Now.Millisecond);

        public int Zero => 0;
        public int One => 1;
        public int NegativeOne => -1;

        public int Random() => random.Next();
        public int Random(int min, int max) => random.Next(min, max);
        public int Abs(int a) => System.Math.Abs(a);

        public int Sum(int a, int b) => a + b;
        public int Sub(int a, int b) => a - b;
        public int Dvide(int a, int b) => a / b;
        public int Multi(int a, int b) => a * b;

        public bool Equal(int a, int b) => a == b;
        public bool Greater(int a, int b) => a > b;
        public bool GreaterOrEqual(int a, int b) => a >= b;
        public bool Less(int a, int b) => a < b;
        public bool LessOrEqual(int a, int b) => a <= b;
    }
}