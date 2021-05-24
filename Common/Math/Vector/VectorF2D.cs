using System;
using ProtoBuf;

namespace MRL.SSL.Common.Math
{
    [ProtoContract]
    public class VectorF2D : Vector2D<float>
    {
        public VectorF2D() : base() { }
        public VectorF2D(float _x, float _y) : base(_x, _y) { }

        public static VectorF2D Zero
        {
            get { return new VectorF2D(0F, 0F); }
        }

        [ProtoMember(1, IsRequired = true)]
        public override float X { get => x; set => x = value; }

        [ProtoMember(2, IsRequired = true)]
        public override float Y { get => y; set => y = value; }

        public override VectorF2D Add(Vector2D<float> v)
        {
            return new VectorF2D(x + v.X, y + v.Y);
        }
        public override float SmallestAngleBetweenInDegrees(Vector2D<float> v)
        {
            return SmallestAngleBetweenInRadians(v) * 180F / MathF.PI;
        }

        public override float SmallestAngleBetweenInRadians(Vector2D<float> v)
        {
            var l1 = Length();
            var l2 = v.Length();
            if (l1 < MathHelper.EpsilonF) return v.AngleInRadians();
            else if (l2 < MathHelper.EpsilonF) return AngleInRadians();

            return System.MathF.Acos(MathHelper.BoundF(Cosine(v), -1, 1));
        }
        public override float AngleBetweenInDegrees(Vector2D<float> v)
        {
            return AngleBetweenInRadians(v) * 180F / MathF.PI;
        }

        public override float AngleBetweenInRadians(Vector2D<float> v)
        {
            var angle = SmallestAngleBetweenInRadians(v);
            var sgn = new VectorF3D(0, 0, 1).TripleProdcut(this, v);
            if (sgn > 0)
                return -angle;
            return angle;
        }

        public override VectorF3D Cross(Vector2D<float> p)
        {
            return new VectorF3D(0F, 0F, x * p.Y - y * p.X);
        }

        public override VectorF2D Divide(float p)
        {
            return new VectorF2D(x / p, y / p);
        }

        public override float Dot(Vector2D<float> v)
        {
            return x * v.X + y * v.Y;
        }


        public static VectorF2D FromAngleSize(float angle, float size)
        {
            return new VectorF2D(size * MathF.Cos(angle), size * MathF.Sin(angle));
        }
        public override void ToAngleSize(float angle, float size)
        {
            x = size * MathF.Cos(angle);
            y = size * MathF.Sin(angle);
        }


        public override float Length()
        {
            return MathF.Sqrt(x * x + y * y);
        }

        public override VectorF2D GetNorm()
        {
            VectorF2D r = new VectorF2D();
            float Size = Length();
            if (Size < MathHelper.EpsilonF) return r;
            r.x = x / Size; r.y = y / Size;
            return r;
        }

        public override void Norm()
        {
            float Size = Length();
            if (Size < MathHelper.EpsilonF) { x = 0F; y = 0F; return; }
            x /= Size; y /= Size;
        }

        public override void NormTo(float newLength)
        {
            float size = Length();
            if (size < MathHelper.EpsilonF)
            {
                x = y = 0F;
                return;
            }
            x *= newLength / size;
            y *= newLength / size;

        }
        public override VectorF2D GetNormTo(float newLength)
        {
            VectorF2D r = new VectorF2D();
            float size = Length();
            if (size < MathHelper.EpsilonF)
                return r;

            r.x = x * newLength / size;
            r.y = y * newLength / size;

            return r;
        }
        public override VectorF2D Scale(float s)
        {
            return new VectorF2D(x * s, y * s);
        }

        public override float SqLength()
        {
            return x * x + y * y;
        }

        public override VectorF2D Sub(Vector2D<float> v)
        {
            return new VectorF2D(x - v.X, y - v.Y);
        }

        public override float AngleInRadians()
        {
            return MathF.Atan2(y, x);
        }

        public override float AngleInDegrees()
        {
            return AngleInRadians() * 180F / MathF.PI;
        }

        public override VectorF2D GetRotate(float angle)
        {
            var r = new VectorF2D(x, y);
            r.Rotate(angle);
            return r;
        }

        public override void Rotate(float angle)
        {
            float cos = System.MathF.Cos(angle), sin = System.MathF.Sin(angle);
            var px = x * cos - y * sin;
            var py = x * sin + y * cos;
            x = px;
            y = py;
        }

        public override VectorF2D GetPerp()
        {
            return new VectorF2D(-y, x);
        }


        public override float OffsetToLine(Vector2D<float> x1, Vector2D<float> p)
        {
            float xt = x1.X - x;
            float yt = x1.Y - y;
            float size = MathF.Sqrt(xt * xt + yt * yt);
            if (size < MathHelper.EpsilonF) { return 0F; }
            return MathF.Abs((-y * (p.X - x) + x * (p.Y - y)) / size);
        }

        public override float OffsetAlongLine(Vector2D<float> x1, Vector2D<float> p)
        {
            Vector2D<float> n, v;
            // get normal to line
            n = x1 - this;
            n.Norm();
            v = p - this;
            return (n.Dot(v));
        }

        public override VectorF2D PrependecularPoint(Vector2D<float> lHead, Vector2D<float> lTail)
        {
            var c = new VectorF2D(y - lHead.Y, lHead.X - x);
            var d1 = (lTail - lHead);
            var d2 = d1.GetPerp();
            var t = -(c.Dot(d2) / d1.Dot(d1));
            return (VectorF2D)(lHead + d1 * t);
        }

        public override bool IsBetween(Vector2D<float> other, Vector2D<float> v)
        {
            Vector3D<float> n = other.Cross(this);
            float innerL = n.Dot(other.Cross(v)), innerR = n.Dot(v.Cross(this));
            return innerL >= 0F && innerR >= 0F;
        }

        public override VectorF2D Reverse()
        {
            return new VectorF2D(-x, -y);
        }

        public override VectorF2D Abs()
        {
            VectorF2D r = new VectorF2D
            {
                x = MathF.Abs(x),
                y = MathF.Abs(y)
            };
            return r;
        }

        public override VectorF2D Max(Vector2D<float> v)
        {
            VectorF2D r = new VectorF2D
            {
                x = MathF.Max(x, v.X),
                y = MathF.Max(y, v.Y)
            };
            return r;
        }

        public override VectorF2D Bound(float low, float high)
        {
            VectorF2D r = new VectorF2D(x, y);
            if (x < low) r.x = low; else if (x > high) r.x = high;
            if (y < low) r.y = low; else if (y > high) r.y = high;
            return r;
        }

        public override float Cosine(Vector2D<float> v)
        {
            return this.Dot(v) / (Length() * v.Length());
        }

        public override float Distance(Vector2D<float> v)
        {
            float dx, dy;
            dx = x - v.X;
            dy = y - v.Y;
            return MathF.Sqrt(dx * dx + dy * dy);
        }

        public override float SqDistance(Vector2D<float> v)
        {
            float dx, dy;
            dx = x - v.X;
            dy = y - v.Y;
            return dx * dx + dy * dy;
        }

        public override float SqDistanceToLine(Vector2D<float> lHead, Vector2D<float> lTail)
        {
            return SqDistance(PrependecularPoint(lHead, lTail));
        }
        public override float DistanceToLine(Vector2D<float> lHead, Vector2D<float> lTail)
        {
            return Distance(PrependecularPoint(lHead, lTail));
        }

        public override VectorF2D Extend(float x, float y)
        {
            return new VectorF2D(this.x + x, this.y + y);
        }

        public override VectorF2D Interpolate(Vector2D<float> end, float amount)
        {
            return new VectorF2D(x * (1F - amount) + end.X * amount, y * (1F - amount) + end.Y * amount);
        }

        public override VectorF2D SegmentNearLine(Vector2D<float> a1, Vector2D<float> b0, Vector2D<float> b1)
        {
            Vector2D<float> v, n, p;
            float dn, t;

            v = a1 - this;
            n = (b1 - b0).GetNorm();
            n = n.GetPerp();

            dn = Dot(v, n);
            if (MathF.Abs(dn) < MathHelper.EpsilonF) return this;

            t = -Dot(this - b0, n) / dn;
            if (t < 0) t = 0F;
            if (t > 1) t = 1F;
            p = this + v * t;

            return (VectorF2D)p;
        }

        public override VectorF2D Intersection(Vector2D<float> a2, Vector2D<float> b1, Vector2D<float> b2)
        {
            Vector2D<float> d1 = this - a2, d2 = b1 - b2;
            float t = (this - b1).GetPerp().Dot(d2) / d1.GetPerp().Dot(d2);
            return (VectorF2D)(this - d1 * t);
        }

        public override float AngleModInRadians(float angle)
        {
            if (angle >= 0)
            {
                if (angle <= MathF.PI) return angle;
                else return angle - MathF.PI;
            }
            else
            {
                if (angle >= -MathF.PI) return angle + MathF.PI;
                else return -angle;
            }
        }
        public override float AngleModInDegrees(float angle)
        {
            return AngleModInRadians(angle) * 180F / MathF.PI;
        }

        public override VectorF2D PointOnSegment(Vector2D<float> x1, Vector2D<float> p)
        {
            Vector2D<float> sx, sp, r;
            float f, l;

            sx = x1 - this;
            sp = p - this;

            f = Dot(sx, sp);
            if (f <= 0.0) return this;         // also handles this=x1 case

            l = sx.SqLength();
            if (f >= l) return (VectorF2D)x1;

            r = this + sx * (f / l);

            return (VectorF2D)r;
        }

        public override float ClosestPointTime(Vector2D<float> v1, Vector2D<float> x2, Vector2D<float> v2)
        {
            Vector2D<float> v = v1 - v2;
            float sl = v.SqLength();
            float t;

            if (sl < MathHelper.EpsilonF) return 0.0F; // parallel tracks, any time is ok.

            t = -v.Dot(this - x2) / sl;
            if (t < 0.0F) return 0.0F; // nearest time was in the past, now
                                       // is closest point from now on.

            return t;
        }

        public override float DistanceSegToSeg(Vector2D<float> s1b, Vector2D<float> s2a, Vector2D<float> s2b)
        {
            Vector2D<float> dp;
            Vector2D<float> u = s1b - this;
            Vector2D<float> v = s2b - s2a;
            Vector2D<float> w = this - s2a;
            float a = Dot(u, u);        // always >= 0
            float b = Dot(u, v);
            float c = Dot(v, v);        // always >= 0
            float d = Dot(u, w);
            float e = Dot(v, w);
            float D = a * c - b * b;       // always >= 0
            float sc, sN, sD = D;      // sc = sN / sD, default sD = D >= 0
            float tc, tN, tD = D;      // tc = tN / tD, default tD = D >= 0

            // compute the line parameters of the two closest points
            if (D < MathHelper.EpsilonF)
            {    // the lines are almost parallel
                sN = 0.0F;
                tN = e;
                tD = c;
            }
            else
            {                // get the closest points on the infinite lines
                sN = (b * e - c * d);
                tN = (a * e - b * d);
                if (sN < 0F)
                {         // sc < 0 => the s=0 edge is visible
                    sN = 0.0F;
                    tN = e;
                    tD = c;
                }
                else if (sN > sD)
                {  // sc > 1 => the s=1 edge is visible
                    sN = sD;
                    tN = e + b;
                    tD = c;
                }
            }

            if (tN < 0F)
            {           // tc < 0 => the t=0 edge is visible
                tN = 0.0F;
                // recompute sc for this edge
                if (-d < 0F)
                {
                    sN = 0.0F;
                }
                else if (-d > a)
                {
                    sN = sD;
                }
                else
                {
                    sN = -d;
                    sD = a;
                }
            }
            else if (tN > tD)
            {      // tc > 1 => the t=1 edge is visible
                tN = tD;
                // recompute sc for this edge
                if ((-d + b) < 0F)
                {
                    sN = 0F;
                }
                else if ((-d + b) > a)
                {
                    sN = sD;
                }
                else
                {
                    sN = (-d + b);
                    sD = a;
                }
            }
            // finally do the division to get sc and tc
            sc = sN / sD;
            tc = tN / tD;

            // get the difference of the two closest points
            dp = w + u * sc - v * tc; // = S1(sc) - S2(tc)

            return dp.Length(); // return the closest distance
        }

        public override VectorF2D ToAiCoordinate(bool isReverse)
        {
            return Scale(isReverse ? -0.001f : 0.001f);
        }

        public override VectorF2D ToVisionCoordinate(bool isReverse)
        {
            return Scale(isReverse ? -1000f : 1000f);
        }
        public override float VertexAngle(Vector2D<float> b, Vector2D<float> c)
        {
            return AngleModInRadians((this - b).AngleInRadians() - (c - b).AngleInRadians());
        }
        public override bool Equals(object obj)
        {
            if (obj != null && (obj is VectorF2D v))
                return MathF.Abs(x - v.x) < MathHelper.EpsilonF && MathF.Abs(y - v.y) < MathHelper.EpsilonF;
            return false;
        }


        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}