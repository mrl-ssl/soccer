using System.Collections.Generic;
using MRL.SSL.Common.Math;
using System.Linq;
using System;
using MRL.SSL.Common.Utils.Extensions;
using MatrixF = MRL.SSL.Common.Math.Matrix<float>;
using SquareMatrixF = MRL.SSL.Common.Math.SquareMatrix<float>;

namespace MRL.SSL.Ai.MergerTracker
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

        protected KalmanBase(int _stateN, int _obsN, double _stepSize)
        {
            stateNum = _stateN;
            obsNum = _obsN;
            stepSize = _stepSize;

            xs = new();
            xs.Enqueue(matrixBuilder.DenseZero(stateNum, 1));
            Ps = new();
            Ps.Enqueue(matrixBuilder.DenseZero(stateNum));
            Is = new();
            Is.Enqueue(matrixBuilder.DenseZero(1));

            predictionLookahead = 0;
            predictionTime = 0;
            errors = matrixBuilder.DenseZero(stateNum, 1);
        }

        public abstract MatrixF f(bool visionProblem, MatrixF x, ref MatrixF I, bool checkCollision); // noiseless dynamics
        public abstract MatrixF h(MatrixF x); // noiseless observation
        public abstract MatrixF Q(MatrixF x); // Covariance of propagation noise
        public abstract MatrixF R(MatrixF x); // Covariance of observation noise
        public abstract MatrixF A(bool visionProblem, MatrixF x); // Jacobian of f w.r.t. x
        public abstract MatrixF W(MatrixF x); // Jacobian of f w.r.t. noise
        public abstract MatrixF H(MatrixF x); // Jacobian of h w.r.t. x
        public abstract MatrixF V(MatrixF x);// Jacobian of h w.r.t. noise

        public abstract void Observe(double timestamp, bool visionProblem, bool checkCollision);

        protected virtual void Initial(double t, ref MatrixF x, ref MatrixF P)
        {
            xs.Clear();
            xs.Enqueue(x);
            Ps.Clear();
            Ps.Enqueue(P);
            Is.Clear();
            Is.Enqueue(matrixBuilder.DenseZero(1, 1));
            steppedTime = time = t;
        }
        protected virtual void Propagate(bool visionProblem, bool checkCollision)
        {
            MatrixF x = xs.Last();
            MatrixF P = Ps.Last();
            MatrixF __A = A(visionProblem, x);
            MatrixF I = matrixBuilder.DenseZero(1, 1);

            x = f(visionProblem, x, ref I, checkCollision);
            MatrixF __W = W(x);
            MatrixF __Q = Q(x);
            tmpC = __W * __Q * __W.Transpose();
            P = __A * P * __A.Transpose() + tmpC;
            xs.Enqueue(x);
            Ps.Enqueue(P);
            Is.Enqueue(I);
            steppedTime += stepSize;
        }
        protected virtual void Tick(double dt, bool visionProblem, bool checkCollision)
        {
            int nsteps = (int)Math.Round(dt / stepSize);

            while (xs.Count - 1 < nsteps) Propagate(visionProblem, checkCollision);

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
        protected virtual void Update(MatrixF z, bool visionProblem, bool checkCollision)
        {
            var x = xs.First();
            var P = Ps.First();
            var I = Is.First();
            var __H = H(x);

            xs.Clear(); Ps.Clear(); Is.Clear();
            steppedTime = time;
            SquareMatrixF =

            MatrixF K = P * (__H.Transpose()) * ((SquareMatrix)(__H * P * __H.Transpose() + tmpCV)).Inverse();
            MatrixF error = K * (z - h(x));
            x = x + error;
            imCheck[0, 0] = x[0, 0];
            imCheck[1, 0] = x[1, 0];
            imCheck[2, 0] = x[2, 0];
            imCheck[3, 0] = x[3, 0];
            var tmpM = new RectangularMatrix(P.RowCount, P.RowCount);
            tmpM.Fill((i, j) =>
            {
                if (i == j)
                    return 1;
                else
                    return 0;
            });
            P = (tmpM - K * __H) * P;
            xs.Enqueue(x); Ps.Enqueue(P); Is.Enqueue(I);
            if (prediction_lookahead > 0.0)
            {
                if (time - prediction_time >= prediction_lookahead)
                {
                    if (prediction_x != null)
                    {
                        if (prediction_time > 0.0)
                        {
                            error = x - prediction_x;

                            for (int i = 0; i < error.RowCount; i++)
                                errors[i, 0] += Math.Abs(error[i, 0]);
                            errors_n++;
                        }
                    }
                    prediction_x = predict(prediction_lookahead);
                    prediction_time = time;
                }
            }
        }
        protected virtual MatrixF Predict(double dt, bool visionProblem, bool checkCollision)
        {
            int nsteps = (int)Math.Round(dt / stepSize);

            while (xs.Count - 1 < nsteps) Propagate(visionProblem, checkCollision);

            return xs.ElementAt(nsteps);
        }

        protected virtual MatrixF PredictCov(double dt, bool visionProblem, bool checkCollision)
        {
            int nsteps = (int)Math.Round(dt / stepSize);
            while (xs.Count - 1 < nsteps) Propagate(visionProblem, checkCollision);
            return Ps.ElementAt(nsteps);
        }

        protected virtual MatrixF PredictInfo(double dt, bool visionProblem, bool checkCollision)
        {
            int nsteps = (int)Math.Round(dt / stepSize);

            while (xs.Count - 1 < nsteps) Propagate(visionProblem, checkCollision);

            return Is.ElementAt(nsteps);
        }

        protected virtual MatrixF PredictFast(double dt, bool visionProblem, bool checkCollision)
        {
            int nsteps = (int)Math.Round(dt / stepSize);
            double origStepsize = stepSize;

            if (xs.Count - 1 >= nsteps) return xs.ElementAt(nsteps);

            stepSize = dt - (steppedTime - time);
            Propagate(visionProblem, checkCollision);

            var rv = xs.Last();

            steppedTime -= stepSize;
            stepSize = origStepsize;

            Ps.PopBack();
            Is.PopBack();

            return rv;
        }

        public virtual float GetObsLikelihood(double dt, MatrixF z, bool visionProblem, bool checkCollision)
        {
            var x = Predict(dt, visionProblem, checkCollision);
            var P = PredictCov(dt, visionProblem, checkCollision);
            var _hx = h(x);
            var _H = H(x);

            var C = _H * P * (_H.Transpose());

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

        public virtual void Reset() { }
    }

}