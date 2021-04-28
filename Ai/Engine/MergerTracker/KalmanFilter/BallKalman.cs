using System;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using MatrixF = MRL.SSL.Common.Math.Matrix<float>;

namespace MRL.SSL.Ai.MergerTracker
{
    public enum OccludeType { Visible, MaybeOccluded, Occluded };
    public class BallKalman : KalmanBase
    {
        public BallKalman() : base(4, 2, MergerTrackerConfig.Default.FramePeriod)
        {

        }
        // public bool IsImmobile()
        // {
        //     VectorF2D tmp = velocity(MergerTrackerConfig.Default.FramePeriod);
        //     if (tmp.SqLength() < MergerTrackerConfig.Default.SqImmobileThreshold)
        //         return true;
        //     return false;
        // }
        public override MatrixF A(bool visionProblem, MatrixF x)
        {
            throw new System.NotImplementedException();
        }

        public override MatrixF f(bool visionProblem, MatrixF x, ref MatrixF I, bool checkCollision)
        {
            throw new System.NotImplementedException();
        }

        public override MatrixF h(MatrixF x)
        {
            throw new System.NotImplementedException();
        }

        public override MatrixF H(MatrixF x)
        {
            throw new System.NotImplementedException();
        }

        public override MatrixF Q(MatrixF x)
        {
            throw new System.NotImplementedException();
        }

        public override MatrixF R(MatrixF x)
        {
            throw new System.NotImplementedException();
        }

        public override MatrixF V(MatrixF x)
        {
            throw new System.NotImplementedException();
        }

        public override MatrixF W(MatrixF x)
        {
            throw new System.NotImplementedException();
        }

        public override void Observe(double timestamp, bool visionProblem, bool checkCollision)
        {
            throw new System.NotImplementedException();
        }
        
    }
}