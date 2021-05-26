using System.Collections.Generic;
using ProtoBuf;
namespace MRL.SSL.Common.Math
{
    [ProtoContract]
    public class Circle
    {
        [ProtoMember(1, IsRequired = true)]
        public VectorF2D Position { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public float Radius { get; set; }

        public Circle(VectorF2D position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public int GetTangent(VectorF2D P, out List<Line> TangentLines, out List<VectorF2D> TangentPoints)
        {
            Vector2D<float> vect = Position - P;
            float dist = vect.Length();
            TangentLines = new List<Line>();
            TangentPoints = new List<VectorF2D>();
            if (dist >= Radius)
            {
                Line l = new Line(P, Position);
                if (dist == Radius)
                {
                    TangentPoints.Add(P);
                    TangentLines.Add(l.PerpenducilarLineToPoint(Position));
                    return 1;
                }
                else
                {
                    float lineAngle = vect.AngleInRadians();
                    float openingAngle = System.MathF.Asin(Radius / dist);
                    float tangentDist = System.MathF.Sqrt(dist * dist - Radius * Radius);
                    VectorF2D v1 = VectorF2D.FromAngleSize(lineAngle + openingAngle, tangentDist);
                    TangentLines.Add(new Line(P, (VectorF2D)(P + v1)));
                    TangentPoints.Add((VectorF2D)(P + v1));

                    v1 = VectorF2D.FromAngleSize(lineAngle - openingAngle, tangentDist);
                    TangentLines.Add(new Line(P, (VectorF2D)(P + v1)));
                    TangentPoints.Add((VectorF2D)(P + v1));
                    return 2;
                }
            }
            else
                return 0;
        }
    }
}