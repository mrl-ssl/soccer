using System;
using MRL.SSL.Common.Math.Helpers;

namespace MRL.SSL.Common.Math
{
    public class Quaternion
    {

        float x;
        float y;
        float z;
        float w;

        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        public float Z
        {
            get { return z; }
            set { z = value; }
        }
        public float W
        {
            get { return w; }
            set { w = value; }
        }

        public Quaternion(float _x, float _y, float _z, float _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }
        public Quaternion()
        {
            x = 0F;
            y = 0F;
            z = 0F;
            w = 0F;
        }
        public Quaternion(Quaternion q)
        {
            x = q.X;
            y = q.Y;
            z = q.Z;
            w = q.W;
        }


        public static Quaternion operator *(Quaternion q, Quaternion r)
        {
            return new Quaternion(
            r.X = r.W * q.X + r.X * q.W + r.Y * q.Z - r.Z * q.Y,
            r.Y = r.W * q.Y - r.X * q.Z + r.Y * q.W + r.Z * q.x,
            r.Z = r.W * q.Z + r.X * q.Y - r.Y * q.X + r.Z * q.W,
            r.W = r.W * q.W - r.X * q.X - r.Y * q.Y - r.Z * q.Z
            );
        }

        public static Quaternion operator *(Quaternion q, VectorF3D v)
        {
            return new Quaternion(
                q.W * v.X + q.Y * v.Z - q.Z * v.Y,
                q.W * v.Y - q.X * v.Z + q.Z * v.X,
                q.W * v.Z + q.X * v.Y - q.Y * v.X,
              -q.X * v.X - q.Y * v.Y - q.Z * v.Z
                );
        }

        public static bool operator ==(Quaternion q, Quaternion r)
        {
            if (q == null && r == null) return true;
            if (q == null) return false;
            return q.Equals(r);
        }

        public static bool operator !=(Quaternion q, Quaternion r)
        {
            if (q == null && r == null) return false;
            if (q == null) return true;
            return !q.Equals(r);
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Quaternion q)
                return MathF.Abs(q.x - x) < MathHelper.EpsilonF && MathF.Abs(q.y - y) < MathHelper.EpsilonF && MathF.Abs(q.z - z) < MathHelper.EpsilonF && MathF.Abs(q.w - w) < MathHelper.EpsilonF;
            return false;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode() ^ w.GetHashCode();
        }

        public void Normalize()
        {
            float mag2 = (w * w) + (x * x) + (y * y) + (z * z);
            if (MathF.Abs(mag2 - 1F) > MathHelper.EpsilonF)
            {
                float mag = MathF.Sqrt(mag2);
                if (mag != 0F)
                {
                    w /= mag;
                    x /= mag;
                    y /= mag;
                    z /= mag;
                }
            }
        }

        public void Blend(float d, Quaternion q)
        {
            float norm = x * q.X + y * q.Y + z * q.Z + w * q.W;
            bool bFlip = false;
            if (norm < 0)
            {
                norm = -norm;
                bFlip = true;
            }
            float inv_d;
            if (1F - norm < MathHelper.EpsilonF)
            {
                inv_d = 1F - d;
            }
            else
            {
                float theta = MathF.Acos(norm);
                float s = (1F / MathF.Sin(theta));
                inv_d = MathF.Sin((1F - d) * theta) * s;
                d = MathF.Sin(d * theta) * s;
            }
            if (bFlip)
            {
                d = -d;
            }
            x = inv_d * x + d * q.X;
            y = inv_d * y + d * q.Y;
            z = inv_d * z + d * q.Z;
            w = inv_d * w + d * q.W;
        }

        public void Clear()
        {
            x = 0;
            y = 0;
            z = 0;
            w = 1F;
        }

        public void Conjugate()
        {
            x = -x;
            y = -y;
            z = -z;
        }

        public void Invert()
        {
            Conjugate();
            float norm = x * x + y * y + z * z + w * w;
            if (norm != 0F)
            {
                float inv_norm = 1F / norm;
                x *= inv_norm;
                y *= inv_norm;
                z *= inv_norm;
                w *= inv_norm;
            }
        }

        public void Set(float qx, float qy, float qz, float qw)
        {
            x = qx;
            y = qy;
            z = qz;
            w = qw;
        }

        public void SetAxis(VectorF3D v, float angle)
        {
            if (angle != 0F)
            {

                angle *= 0.5F;
                float sinAngle = MathF.Sin(angle);
                VectorF3D vn = v;
                vn.Normalize();
                x = (vn.X * sinAngle);
                y = (vn.Y * sinAngle);
                z = (vn.Z * sinAngle);
                w = MathF.Cos(angle);
            }
            else
            {
                Clear();
            }
        }

        public VectorF3D GetZvector()
        {
            VectorF3D v = new VectorF3D(0F, 0F, 1F);
            return RotateVectorByQuaternion(v);
        }
        public VectorF3D RotateVectorByQuaternion(VectorF3D v)
        {
            float x2 = x * x;
            float y2 = y * y;
            float z2 = z * z;
            float w2 = w * w;
            float xy = x * y;
            float xz = x * z;
            float yz = y * z;
            float wx = w * x;
            float wy = w * y;
            float wz = w * z;
            VectorF3D res = new VectorF3D();
            res.X = (1F - 2F * (y2 + z2)) * v.X + (2F * (xy - wz)) * v.Y + (2F * (xz + wy)) * v.Z;
            res.Y = (2F * (xy + wz)) * v.X + (1F - 2F * (x2 + z2)) * v.Y + (2F * (yz - wx)) * v.Z;
            res.Z = (2F * (xz - wy)) * v.X + (2F * (yz + wx)) * v.Y + (1 - 2F * (x2 + y2)) * v.Z;

            return res;
        }

        public SquareMatrix<float> GetMatrix()
        {
            float x2 = x * x;
            float y2 = y * y;
            float z2 = z * z;
            float xy = x * y;
            float xz = x * z;
            float yz = y * z;
            float wx = w * x;
            float wy = w * y;
            float wz = w * z;
            SquareMatrix<float> m = new SquareMatrix<float>(3);
            m[0, 0] = 1F - 2F * (y2 + z2); m[0, 1] = (2F * (xy - wz)); m[0, 2] = 2F * (xz - wy);
            m[1, 0] = 2F * (xy + wz); m[1, 1] = 1F - 2F * (x2 + z2); m[1, 2] = 2F * (yz - wx);
            m[2, 0] = 2F * (xz - wy); m[2, 1] = 2F * (yz + wx); m[2, 2] = 1F - 2F * (x2 + y2);
            return m;
        }

        public void GetAxisAngle(out VectorF3D axis, out float angle)
        {
            float scale = MathF.Sqrt(x * x + y * y + z * z);
            axis = new VectorF3D(x / scale, y / scale, z / scale);
            angle = MathF.Acos(w) * 2F;
        }

        public float GetAngle()
        {
            return MathF.Acos(w) * 2F;
        }

        public void SetEuler(float pitch, float yaw, float roll)
        {
            float c1 = MathF.Cos(yaw / 2F);
            float s1 = MathF.Cos(yaw / 2F);
            float c2 = MathF.Cos(roll / 2F);
            float s2 = MathF.Cos(roll / 2F);
            float c3 = MathF.Cos(pitch / 2F);
            float s3 = MathF.Cos(pitch / 2F);
            w = c1 * c2 * c3 - s1 * s2 * s3;
            x = c1 * c2 * s3 + s1 * s2 * c3;
            y = s1 * c2 * c3 + c1 * s2 * s3;
            z = c1 * s2 * c3 - s1 * c2 * s3;
            Normalize();
        }

        public float GetYaw()
        {
            if (MathF.Abs((x * y + z * w) - 0.5F) < MathHelper.EpsilonF)
            {
                return 2F * MathF.Atan2(x, w);
            }
            if (MathF.Abs((x * y + z * w) + 0.5F) < MathHelper.EpsilonF)
            {
                return -2F * MathF.Atan2(x, w);
            }
            return MathF.Atan2(2F * y * w - 2F * x * z, 1F - 2F * (y * y) - 2F * (z * z));
        }
        public float GetPitch()
        {
            if (MathF.Abs((x * y + z * w) - 0.5F) < MathHelper.EpsilonF)
            {
                return 0F;
            }
            if (MathF.Abs((x * y + z * w) + 0.5F) < MathHelper.EpsilonF)
            {
                return 0F;
            }
            return MathF.Atan2(2F * x * w - 2F * y * z, 1F - 2F * (x * x) - 2F * (z * z));
        }
        public float GetRoll()
        {
            return MathF.Asin(2 * x * y + 2 * z * w);
        }
        public void GetEuler(out float pitch, out float yaw, out float roll)
        {
            pitch = GetPitch();
            yaw = GetYaw();
            roll = GetRoll();
        }

        public static Quaternion ShortestArc(VectorF3D from, VectorF3D to)
        {
            Vector3D<float> cross = from * to;
            float dot = from.Dot(to);
            dot = MathF.Sqrt(2F * (dot + 1F));
            cross /= dot;
            return new Quaternion(cross.X, cross.Y, cross.Z, -dot / 2F);
        }
    }
}