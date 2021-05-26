using ProtoBuf;

namespace MRL.SSL.Common.Math
{
    /// <summary>
    /// Represent line as Ax + By + C = 0
    /// </summary>
    [ProtoContract]
    public class Line
    {

        [ProtoMember(1, IsRequired = true)]
        public VectorF2D Head { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public VectorF2D Tail { get; set; }

        public float A { get; set; }

        public float B { get; set; }

        public float C { get; set; }


        public float Angle => System.MathF.Atan2(B, A);

        public Line(float a, float b, float c) { A = a; B = b; C = c; }
        public Line(VectorF2D p1, VectorF2D p2)
        {
            A = p2.Y - p1.Y;
            B = p1.X - p2.X;
            C = -(A * p1.X + B * p1.Y);
            Head = p1; Tail = p2;
        }
        public Line PerpenducilarLineToPoint(VectorF2D From)
        {
            return new Line(this.B, -this.A, -(this.B * From.X + -this.A * From.Y));
        }
        public VectorF2D IntersectWithLine(Line l2, out bool HasValue)
        {
            VectorF2D a = new VectorF2D(0, 0);
            float det = this.A * l2.B - l2.A * this.B;
            if ((float)System.MathF.Abs(det) > 0.0001f)
            {
                a.X = ((l2.C * this.B - this.C * l2.B) / det);
                a.Y = ((this.C * l2.A - l2.C * this.A) / det);
                HasValue = true;
            }
            else
                HasValue = false;
            return a;
        }
    }
}