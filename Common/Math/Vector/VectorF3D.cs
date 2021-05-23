using System;
using ProtoBuf;

namespace MRL.SSL.Common.Math
{
    [ProtoContract]
    public class VectorF3D : Vector3D<float>
    {
        public VectorF3D() { }
        public VectorF3D(float X, float Y, float Z) : base(X, Y, Z) { x = X; y = Y; z = Z; }
        public static VectorF3D Zero
        {
            get { return new VectorF3D(0F, 0F, 0F); }
        }
        public override VectorF3D Abs()
        {
            VectorF3D r = new VectorF3D
            {
                x = MathF.Abs(x),
                y = MathF.Abs(y),
                z = MathF.Abs(z)
            };
            return r;
        }

        public override VectorF3D Add(Vector3D<float> v)
        {
            VectorF3D r = new VectorF3D
            {
                x = x + v.X,
                y = y + v.Y,
                z = z + v.Z
            };
            return r;
        }

        public override VectorF3D Bound(float low, float high)
        {
            VectorF3D r = new VectorF3D(x, y, z);
            if (x < low) r.x = low; else if (x > high) r.x = high;
            if (y < low) r.y = low; else if (y > high) r.y = high;
            if (z < low) r.z = low; else if (z > high) r.z = high;
            return r;
        }

        public override VectorF3D Cross(Vector3D<float> p)
        {
            VectorF3D r = new VectorF3D
            {
                x = y * p.Z - z * p.Y,
                y = z * p.X - x * p.Z,
                z = x * p.Y - y * p.X
            };
            return r;
        }

        public override float Distance(Vector3D<float> v)
        {
            float dx, dy, dz;
            dx = x - v.X;
            dy = y - v.Y;
            dz = z - v.Z;
            return MathF.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public override float DistanceToLine(Vector3D<float> lHead, Vector3D<float> lTail)
        {
            //To do...
            /*Vector3D<float> r = new VectorF3D();
            float t;
            t = ((x - lHead.X) + (y - lHead.Y) + (z - lHead.Z)) / (lTail.X + lTail.Y + lTail.Z);
            r = lHead + (lTail - lHead) * t;
            return r.Distance(this);*/
            return 0f;
        }
        public override float TripleProdcut(Vector2D<float> v, Vector2D<float> y)
        {
            return Dot(v.Cross(y));
        }

        public override float Dot(Vector3D<float> v)
        {
            return v.X * x + v.Y * y + v.Z * z;
        }


        public override float Length()
        {
            return MathF.Sqrt(x * x + y * y + z * z);
        }

        public override VectorF3D Max(Vector3D<float> v)
        {
            VectorF3D r = new VectorF3D
            {
                x = MathF.Max(x, v.X),
                y = MathF.Max(y, v.Y),
                z = MathF.Max(z, v.Z)
            };
            return r;
        }

        public override VectorF3D GetNorm()
        {
            float l = Length();
            if (l < MathHelper.EpsilonF) { return new VectorF3D(); }
            else
            {
                return new VectorF3D
                {
                    x = x / l,
                    y = y / l,
                    z = z / l
                }; ;
            }
        }
        public override VectorF3D GetNormTo(float newLength)
        {
            float l = Length();
            if (l < MathHelper.EpsilonF) { return new VectorF3D(); }
            else
            {
                return new VectorF3D
                {
                    x = x * newLength / l,
                    y = y * newLength / l,
                    z = z * newLength / l
                }; ;
            }
        }
        public override void Norm()
        {
            float l = Length();
            if (l < MathHelper.EpsilonF) { x = 0F; y = 0F; z = 0F; }
            else
                x /= l; y /= l; z /= l;
        }
        public override void NormTo(float newLength)
        {
            float l = Length();
            if (l < MathHelper.EpsilonF) { x = 0F; y = 0F; z = 0F; }
            else
            {
                x *= newLength / l;
                y *= newLength / l;
                z *= newLength / l;
            }
        }

        public override VectorF3D GetRotateX(float angle)
        {
            VectorF3D q = new VectorF3D();
            float s, c;
            s = MathF.Sin(angle); c = MathF.Cos(angle);
            q.x = x;
            q.y = c * y + -s * z;
            q.z = s * y + c * z;
            return q;
        }

        public override VectorF3D GetRotateY(float angle)
        {
            VectorF3D q = new VectorF3D();
            float s, c;
            s = MathF.Sin(angle); c = MathF.Cos(angle);
            q.x = c * x + s * z;
            q.y = y;
            q.z = -s * x + c * z;
            return q;
        }

        public override VectorF3D GetRotateZ(float angle)
        {
            VectorF3D q = new VectorF3D();
            float s, c;
            s = MathF.Sin(angle); c = MathF.Cos(angle);
            q.x = c * x + -s * y;
            q.y = s * x + c * y;
            q.z = z;
            return q;
        }

        public override VectorF3D Scale(float s)
        {
            VectorF3D r = new VectorF3D
            {
                x = x * s,
                y = y * s,
                z = z * s
            };
            return r;
        }

        public override float SqDistance(Vector3D<float> v)
        {
            float dx, dy, dz;
            dx = x - v.X;
            dy = y - v.Y;
            dz = z - v.Z;
            return dx * dx + dy * dy + dz * dz;
        }

        public override float SqLength()
        {
            return x * x + y * y + z * z;
        }

        public override VectorF3D Sub(Vector3D<float> v)
        {
            VectorF3D r = new VectorF3D
            {
                x = x - v.X,
                y = y - v.Y,
                z = z - v.Z
            };
            return r;
        }



        public static VectorF3D FromAngleSize(float alpha, float beta, float Size)
        {
            return new VectorF3D(Size * MathF.Cos(alpha) * MathF.Cos(beta), Size * MathF.Sin(beta), Size * MathF.Sin(alpha) * MathF.Cos(beta));
        }



        public override VectorF3D Reverse()
        {
            return new VectorF3D(-x, -y, -z);
        }

        public override VectorF3D Divide(float s)
        {
            return new VectorF3D(x / s, y / s, z / s);
        }


        public override VectorF3D Extend(float X, float Y, float Z)
        {
            return new VectorF3D(x + X, y + Y, z + Z);
        }

        public override VectorF3D Interpolate(Vector3D<float> end, float amount)
        {
            return new VectorF3D(x * (1F - amount) + end.X * amount, y * (1F - amount) + end.Y * amount, z * (1F - amount) + end.Z * amount);
        }

        public override float ClosestPointTime(Vector3D<float> v1, Vector3D<float> x2, Vector3D<float> v2)
        {
            Vector3D<float> v = v1 - v2;
            float sl = v.SqLength();
            float t;

            if (sl < MathHelper.EpsilonF) return 0.0F; // parallel tracks, any time is ok.

            t = -v.Dot(this - x2) / sl;
            if (t < 0.0F) return 0.0F; // nearest time was in the past, now
                                       // is closest point from now on.

            return t;
        }

        public override float DistanceSegToSeg(Vector3D<float> s1b, Vector3D<float> s2a, Vector3D<float> s2b)
        {
            Vector3D<float> dp;
            Vector3D<float> u = s1b - this;
            Vector3D<float> v = s2b - s2a;
            Vector3D<float> w = this - s2a;
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

        public override VectorF3D PointOnSegment(Vector3D<float> x1, Vector3D<float> p)
        {
            Vector3D<float> sx, sp, r;
            float f, l;

            sx = x1 - this;
            sp = p - this;

            f = Dot(sx, sp);
            if (f <= 0.0) return this;         // also handles this=x1 case

            l = sx.SqLength();
            if (f >= l) return (VectorF3D)x1;

            r = this + sx * (f / l);

            return (VectorF3D)r;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj != null && (obj is VectorF3D v))
                return MathF.Abs(x - v.x) < MathHelper.EpsilonF && MathF.Abs(y - v.y) < MathHelper.EpsilonF && MathF.Abs(z - v.z) < MathHelper.EpsilonF;
            return false;
        }
    }
}