using System.Collections.Generic;
using MRL.SSL.Common.Math;
using System.Linq;
using System;
using MRL.SSL.Common.Utils.Extensions;
using MatrixF = MRL.SSL.Common.Math.Matrix<float>;
using SMath = System.Math;

namespace MRL.SSL.Common
{
    public abstract class KalmanBase
    {
        protected static MatrixBuilder<float> matrixBuilder = new MatrixBuilder<float>(new FloatOperator());
        protected int stateNum, obsNum; // Number of state and observation variables
        protected double stepSize;
        protected Queue<MatrixF> xs; // State vector. [0] is current state.
        protected Queue<MatrixF> Ps; // Covariance matrix.  [0] is current covariance.
        protected Queue<MatrixF> Is; // Information matrix. [0] is current information.
        protected double steppedTime; // Time of the last state in the future queue.
        protected double time; // Time of the first state in the future queue.

        // Kalman Error
        protected MatrixF predictionX;
        protected double predictionTime;
        protected double predictionLookahead;
        protected MatrixF errors;
        protected int errorsNum;
        protected MatrixF tmpC;
        protected MatrixF tmpCV;
        protected MatrixF Identity;
        protected bool _reset;
        protected MatrixF _z;
        protected MatrixF _A, _H, _W, _Q, _V, _h, _R;
        protected KalmanBase(int _stateN, int _obsN, int propNum, double _stepSize)
        {
            stateNum = _stateN;
            obsNum = _obsN;
            stepSize = _stepSize;
            _reset = true;
            predictionLookahead = 0;
            predictionTime = 0;

            xs = new();
            Ps = new();
            Is = new();
            xs.Enqueue(matrixBuilder.DenseZero(stateNum, 1));
            Ps.Enqueue(matrixBuilder.DenseZero(stateNum));
            Is.Enqueue(matrixBuilder.DenseZero(1));

            Identity = matrixBuilder.Identity(stateNum);
            errors = matrixBuilder.DenseZero(stateNum, 1);

            _z = matrixBuilder.DenseZero(obsNum, 1);
            _A = matrixBuilder.Identity(stateNum);
            _h = matrixBuilder.DenseZero(obsNum, 1);
            _R = matrixBuilder.DenseZero(obsNum, obsNum);
            _H = matrixBuilder.Identity(obsNum, stateNum);
            _V = matrixBuilder.Identity(obsNum);
            _Q = matrixBuilder.DenseZero(propNum, propNum);
            _R = matrixBuilder.Identity(obsNum);
            _W = matrixBuilder.DenseZero(SMath.Max(0, stateNum - propNum), propNum).Stack(matrixBuilder.Identity(propNum));

            var __V = V(null);
            var __R = R(null);
            tmpCV = __V * __R * __V.Transpose();

            var __W = W(null);
            var __Q = Q(null);
            tmpC = __W * __Q * __W.Transpose();

        }

        public abstract MatrixF f(in MatrixF x, ref MatrixF I); // noiseless dynamics
        public abstract MatrixF A(MatrixF x); // Jacobian of f w.r.t. x
        public abstract MatrixF Q(MatrixF x); // Covariance of propagation noise
        public abstract MatrixF R(MatrixF x); // Covariance of observation noise

        public virtual MatrixF h(MatrixF x)// noiseless observation
        {
            for (int i = 0; i < obsNum; i++)
            {
                _h[i, 0] = x[i, 0];
            }
            return _h;
        }
        public virtual MatrixF W(MatrixF x) { return _W; } // Jacobian of f w.r.t. noise
        public virtual MatrixF H(MatrixF x) { return _H; } // Jacobian of h w.r.t. x
        public virtual MatrixF V(MatrixF x) { return _V; }// Jacobian of h w.r.t. noise
        public abstract void Observe(Observation obs);
        protected virtual void Initial(double t, MatrixF x, MatrixF P)
        {
            xs.Clear();
            xs.Enqueue(x);
            Ps.Clear();
            Ps.Enqueue(P);
            Is.Clear();
            Is.Enqueue(matrixBuilder.DenseZero(1, 1));
            steppedTime = time = t;
        }
        protected virtual void Propagate()
        {
            MatrixF x = new MatrixF(xs.Last());
            MatrixF P = Ps.Last();
            MatrixF __A = A(x);
            MatrixF I = matrixBuilder.DenseZero(1, 1);

            x = f(x, ref I);
            P = __A * P * __A.Transpose() + tmpC;
            xs.Enqueue(x);
            Ps.Enqueue(P);
            Is.Enqueue(I);
            steppedTime += stepSize;
        }
        protected virtual void Tick(double dt)
        {
            int nsteps = (int)SMath.Round(dt / stepSize);

            while (xs.Count - 1 < nsteps) Propagate();

            int i = 0;
            while (i < nsteps)
            {
                if (xs.Count > 0) xs.Dequeue();
                if (Ps.Count > 0) Ps.Dequeue();
                if (Is.Count > 0) Is.Dequeue();
                i++;
            }
            time += dt;
        }
        protected virtual void Update(MatrixF z)
        {
            var x = xs.First();
            var P = Ps.First();
            var I = Is.First();
            var __H = H(x);

            xs.Clear(); Ps.Clear(); Is.Clear();
            steppedTime = time;
            // SquareMatrixF =

            MatrixF K = P * __H.Transpose() * (__H * P * __H.Transpose() + tmpCV).ToSquareMatrix().Inverse();
            MatrixF error = K * (z - h(x));
            x = x + error;

            P = (Identity - K * __H) * P;
            xs.Enqueue(x); Ps.Enqueue(P); Is.Enqueue(I);
            if (predictionLookahead > 0.0f)
            {
                if (time - predictionTime >= predictionLookahead)
                {
                    if (predictionX != null)
                    {
                        if (predictionTime > 0.0)
                        {
                            error = x - predictionX;

                            for (int i = 0; i < error.Rows; i++)
                                errors[i, 0] += SMath.Abs(error[i, 0]);
                            errorsNum++;
                        }
                    }
                    predictionX = Predict(predictionLookahead);
                    predictionTime = time;
                }
            }
        }
        protected virtual MatrixF Predict(double dt)
        {
            int nsteps = (int)SMath.Round(dt / stepSize);

            while (xs.Count - 1 < nsteps) Propagate();

            return xs.ElementAt(nsteps);
        }

        protected virtual MatrixF PredictCov(double dt)
        {
            int nsteps = (int)SMath.Round(dt / stepSize);
            while (xs.Count - 1 < nsteps) Propagate();
            return Ps.ElementAt(nsteps);
        }

        protected virtual MatrixF PredictInfo(double dt)
        {
            int nsteps = (int)SMath.Round(dt / stepSize);

            while (xs.Count - 1 < nsteps) Propagate();

            return Is.ElementAt(nsteps);
        }

        protected virtual MatrixF PredictFast(double dt)
        {
            int nsteps = (int)SMath.Round(dt / stepSize);
            double origStepsize = stepSize;

            if (xs.Count - 1 >= nsteps) return xs.ElementAt(nsteps);

            stepSize = dt - (steppedTime - time);
            Propagate();

            var rv = xs.Last();

            steppedTime -= stepSize;
            stepSize = origStepsize;

            Ps.PopBack();
            Is.PopBack();

            return rv;
        }

        public virtual float GetObsLikelihood(double dt, MatrixF z)
        {
            var x = Predict(dt);
            var P = PredictCov(dt);
            var _hx = h(x);
            var _H = H(x);

            var C = _H * P * _H.Transpose();

            var D = z - _hx;

            float likelihood = 1.0f;

            for (int i = 0; i < D.Rows; i++)
                likelihood *= MathF.Exp(-(D[i, 0] * D[i, 0]) / (2f * C[i, i]));

            return likelihood;
        }

        public virtual MatrixF GetErrorMean()
        {
            return (1.0f / (float)errorsNum) * errors;
        }

        public virtual void ResetError()
        {
            errors = 0 * errors;
            errorsNum = 0;
        }

        public virtual double GetTimeElapsedError()
        {
            return errorsNum * predictionLookahead;
        }

        public virtual void Reset() { _reset = true; }
    }

}