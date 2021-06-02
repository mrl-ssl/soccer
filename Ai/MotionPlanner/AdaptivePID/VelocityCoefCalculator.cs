
using System;
using System.Linq;
using System.Collections.Generic;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;

namespace MRL.SSL.Ai.MotionPlanner
{
    abstract class VelocityCoefCalculaterBase
    {
        protected static MatrixBuilder<float> matrixBuilder = new MatrixBuilder<float>(new FloatOperator());

        protected int latency, maxFrame2Calc, prediction;
        protected float coef;
        protected float coefResetVal;
        protected float distTresh, velTresh;
        protected float resetCoefTresh;
        protected float minCoefVal;

        protected float frameRate;

        protected Queue<float> posQ = new Queue<float>();
        protected Queue<float> velQ = new Queue<float>();

        public float Coef
        {
            get { return coef; }
        }

        protected VelocityCoefCalculaterBase(float distTresh, float velTresh, float coefResetVal, float resetCoefTresh,
                                             float minCoefVal)
        {
            this.distTresh = distTresh;
            this.velTresh = velTresh;
            this.coef = this.coefResetVal = coefResetVal;
            this.resetCoefTresh = resetCoefTresh;
            this.minCoefVal = minCoefVal;

            latency = ControlConfig.Default.Latency;
            maxFrame2Calc = ControlConfig.Default.MaxFrames;
            prediction = ControlConfig.Default.Prediction;
            frameRate = MergerTrackerConfig.Default.FrameRate;
        }
        public abstract float UpdateVelocityCoefs(float p, float v, float target);
        public virtual void Check4Reset()
        {
            if (coef < resetCoefTresh)
                coef = coefResetVal;
        }

    }
    class VelocityCoefCalculatorPos : VelocityCoefCalculaterBase
    {
        public VelocityCoefCalculatorPos(float distTresh, float velTresh, float velCoefResetVal, float resetCoefTresh,
                                         float minCoefVal)
                                         : base(distTresh, velTresh, velCoefResetVal, resetCoefTresh, minCoefVal)
        {

        }

        public override float UpdateVelocityCoefs(float p, float v, float target)
        {
            if (MathF.Abs(p - target) > distTresh && MathF.Abs(v) > velTresh)
            {
                posQ.Enqueue(p);
                velQ.Enqueue(v);

                if (posQ.Count > maxFrame2Calc + prediction)
                    posQ.Dequeue();
                if (velQ.Count > maxFrame2Calc + latency)
                    velQ.Dequeue();

                if (velQ.Count < latency + maxFrame2Calc || posQ.Count < prediction + maxFrame2Calc)
                    return coef;

                var tmp = posQ.Skip(prediction).ToArray();
                var xn = matrixBuilder.DenseZero(tmp.Length, 1);
                for (int i = 0; i < tmp.Length; i++)
                    xn[i, 0] = tmp[i];


                tmp = posQ.Take(posQ.Count - prediction).ToArray();
                var x = matrixBuilder.DenseZero(tmp.Length, 1);
                for (int i = 0; i < tmp.Length; i++)
                    x[i, 0] = tmp[i];

                tmp = velQ.Take(velQ.Count - latency).ToArray();
                var vn = matrixBuilder.DenseZero(tmp.Length, 1);
                for (int i = 0; i < tmp.Length; i++)
                    vn[i, 0] = tmp[i];

                var d = frameRate * (xn - x);
                var alfaM = ((SquareMatrix<float>)(vn.Transpose() * vn)).Inverse() * vn.Transpose() * d;

                return coef = alfaM[0, 0];
            }
            posQ.Clear();
            velQ.Clear();
            return coef;
        }
    }
    class VelocityCoefCalculatorAngle : VelocityCoefCalculaterBase
    {
        public VelocityCoefCalculatorAngle(float distTresh, float velTresh, float velCoefResetVal, float resetCoefTresh,
                                           float minCoefVal)
                                       : base(distTresh, velTresh, velCoefResetVal, resetCoefTresh, minCoefVal)
        {

        }

        public override float UpdateVelocityCoefs(float p, float v, float target)
        {
            float dif = MathHelper.AngleMod(p - target);

            if (MathF.Abs(dif) > distTresh && MathF.Abs(v) > velTresh)
            {
                posQ.Enqueue(p);
                velQ.Enqueue(v);

                if (posQ.Count > maxFrame2Calc + prediction)
                    posQ.Dequeue();
                if (velQ.Count > maxFrame2Calc + latency)
                    velQ.Dequeue();

                if (velQ.Count < latency + maxFrame2Calc || posQ.Count < prediction + maxFrame2Calc)
                    return coef;

                float[] xn = posQ.Skip(prediction).ToArray();
                float[] x = posQ.Take(posQ.Count - prediction).ToArray();

                var tmp = velQ.Take(velQ.Count - latency).ToArray();

                var vn = matrixBuilder.DenseZero(tmp.Length, 1);
                for (int i = 0; i < tmp.Length; i++)
                    vn[i, 0] = tmp[i];

                var d = matrixBuilder.DenseZero(maxFrame2Calc, 1);

                for (int i = 0; i < maxFrame2Calc; i++)
                    d[i, 0] = frameRate * MathHelper.AngleMod(xn[i] - x[i]);

                var alfaM = ((SquareMatrix<float>)(vn.Transpose() * vn)).Inverse() * vn.Transpose() * d;

                return coef = MathF.Max(alfaM[0, 0], minCoefVal);
            }
            posQ.Clear();
            velQ.Clear();
            return coef;
        }

    }

}