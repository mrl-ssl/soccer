using System;
using MRL.SSL.Common.Math.Helpers;
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

        [ProtoMember(1)]
        public override float X { get => x; set => x = value; }

        [ProtoMember(2)]
        public override float Y { get => y; set => y = value; }

        public override Vector2D<float> Add(Vector2D<float> v)
        {
            return new VectorF2D(x + v.X, y + v.Y);
        }

        public override float AngleBetweenInDegrees(Vector2D<float> v)
        {
            return AngleBetweenInRadians(v) * 180F / MathF.PI;
        }

        public override float AngleBetweenInRadians(Vector2D<float> v)
        {
            float a1 = MathF.Atan2(y, x), a2 = MathF.Atan2(v.Y, v.X);
            float d = a1 - a2;
            while (d > MathF.PI)
                d -= 2F * MathF.PI;

            while (d < -MathF.PI)
                d += 2F * MathF.PI;
            return d;
        }

        public override Vector3D<float> Cross(Vector2D<float> p)
        {
            return new VectorF3D(0F, 0F, x * p.Y - y * p.X);
        }

        public override Vector2D<float> Divide(float p)
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

        public override float Length()
        {
            return MathF.Sqrt(x * x + y * y);
        }

        public override Vector2D<float> Norm()
        {
            VectorF2D r = new VectorF2D();
            float Size = Length();
            if (Size < MathHelper.EpsilonF) return new VectorF2D(0F, 0F);
            r.x = x / Size; r.y = y / Size;
            return r;
        }

        public override void Normalize()
        {
            float Size = Length();
            if (Size < MathHelper.EpsilonF) { x = 0F; y = 0F; return; }
            x /= Size; y /= Size;
        }

        public override void NormTo(float newLength)
        {
            float size = Length();
            if (size < MathHelper.EpsilonF)
                x = y = 0F;
            else
            {
                x *= newLength / size;
                y *= newLength / size;
            }
        }

        public override Vector2D<float> Scale(float s)
        {
            return new VectorF2D(x * s, y * s);
        }

        public override float SqLength()
        {
            return x * x + y * y;
        }

        public override Vector2D<float> Sub(Vector2D<float> v)
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

        public override Vector2D<float> GetRotate(float angle)
        {
            return FromAngleSize(angle, Length());
        }

        public override void Rotate(float angle)
        {
            VectorF2D v = FromAngleSize(angle, Length());
            x = v.x; y = v.y;
        }

        public override Vector2D<float> GetPerp()
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
            n.Normalize();
            v = p - this;
            return (n.Dot(v));
        }

        public override Vector2D<float> PrependecularPoint(Vector2D<float> start, Vector2D<float> from)
        {
            Vector2D<float> startFromVec = from - start;
            Vector2D<float> fromStartVec = start - from;
            float teta = AngleBetweenInRadians(startFromVec);
            float s = 1F;
            if (MathF.Abs(teta) > MathF.PI / 2F)
            {
                s = -1F;
                teta = (MathF.PI - MathF.Abs(teta)) * MathF.Sign(teta);
            }
            float d = MathF.Abs(startFromVec.Length() * MathF.Sin(teta));
            float alfa = MathF.PI / 2F - MathF.Abs(teta);
            return from + FromAngleSize(fromStartVec.AngleInRadians() - s * MathF.Sign(teta) * alfa, d);
        }

        public override bool IsBetween(Vector2D<float> other, Vector2D<float> v)
        {
            Vector3D<float> n = other.Cross(this);
            float innerL = n.Dot(other.Cross(v)), innerR = n.Dot(v.Cross(this));
            return innerL >= 0F && innerR >= 0F;
        }

        public override Vector2D<float> Reverse()
        {
            return new VectorF2D(-x, -y);
        }

        public override Vector2D<float> Abs()
        {
            VectorF2D r = new VectorF2D
            {
                x = MathF.Abs(x),
                y = MathF.Abs(y)
            };
            return r;
        }

        public override Vector2D<float> Max(Vector2D<float> v)
        {
            VectorF2D r = new VectorF2D
            {
                x = MathF.Max(x, v.X),
                y = MathF.Max(y, v.Y)
            };
            return r;
        }

        public override Vector2D<float> Bound(float low, float high)
        {
            VectorF2D r = new VectorF2D(x, y);
            if (x < low) r.x = low; else if (x > high) r.x = high;
            if (y < low) r.y = low; else if (y > high) r.y = high;
            return r;
        }

        public override float Cosine(Vector2D<float> v)
        {
            float l = (this.Dot(v));
            float t = Length() * v.Length();
            return l / t;
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

        public override float DistanceToLine(Vector2D<float> lHead, Vector2D<float> lTail)
        {
            Vector2D<float> r;
            float t;
            t = ((x - lHead.X) + (y - lHead.Y)) / (lTail.X + lTail.Y);
            r = lHead + (lTail - lHead) * t;
            return r.Distance(this);
        }

        public override Vector2D<float> Extend(float x, float y)
        {
            return new VectorF2D(this.x + x, this.y + y);
        }

        public override Vector2D<float> Interpolate(Vector2D<float> end, float amount)
        {
            return new VectorF2D(x * (1F - amount) + end.X * amount, y * (1F - amount) + end.Y * amount);
        }

        public override Vector2D<float> SegmentNearLine(Vector2D<float> a1, Vector2D<float> b0, Vector2D<float> b1)
        {
            Vector2D<float> v, n, p;
            float dn, t;

            v = a1 - this;
            n = (b1 - b0).Norm();
            n = n.GetPerp();

            dn = Dot(v, n);
            if (MathF.Abs(dn) < MathHelper.EpsilonF) return this;

            t = -Dot(this - b0, n) / dn;
            if (t < 0) t = 0F;
            if (t > 1) t = 1F;
            p = this + v * t;

            return p;
        }

        public override Vector2D<float> Intersection(Vector2D<float> a2, Vector2D<float> b1, Vector2D<float> b2)
        {
            Vector2D<float> a = a2.Sub(this);

            Vector2D<float> b1r = (b1.Sub(this)).GetRotate(-a.AngleInRadians());
            Vector2D<float> b2r = (b2.Sub(this)).GetRotate(-a.AngleInRadians());
            Vector2D<float> br = (b1r.Sub(b2r));
            Vector2D<float> t = new VectorF2D(b2r.X - b2r.Y * (br.X / br.Y), 0F);

            return t.GetRotate(a.AngleInRadians()).Add(this);
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

        public override Vector2D<float> PointOnSegment(Vector2D<float> x1, Vector2D<float> p)
        {
            Vector2D<float> sx, sp, r;
            float f, l;

            sx = x1 - this;
            sp = p - this;

            f = Dot(sx, sp);
            if (f <= 0.0) return this;         // also handles this=x1 case

            l = sx.SqLength();
            if (f >= l) return x1;

            r = this + sx * (f / l);

            return r;
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