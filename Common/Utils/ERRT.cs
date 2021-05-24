using System.Runtime.CompilerServices;
using System.Threading;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;

namespace MRL.SSL.Common.Utils
{
    public class ERRT
    {
        private const float goalProbbality = 0.2f;
        private const float wayPointProbbality = 0.3f;
        private const int numWayPoints = 15;
        private const float extendSize = 0.15f;
        public const int maxNodes = 50;
        public const int maxNodes2Try = 500;
        private const int maxTreeCount = 1000;

        private KdTree tree;
        private ThreadLocal<XorShift> rand;
        private VectorF2D Field;
        private bool useERrrt;

        public ERRT(bool _useErrt)
        {
            tree = new KdTree();
            rand = XorShift.CreateInstance();
            Field = new VectorF2D();
            useERrrt = _useErrt;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected SingleObjectState RandomState()
        {

            return new SingleObjectState(new VectorF2D(Field.X * (1f - 2f * rand.Value.RandFloat()),
                                                        Field.Y * (1f - 2f * rand.Value.RandFloat())),
                                                        VectorF2D.Zero);
        }
        protected SingleObjectState ChoosTarget(SingleObjectState goal, SingleObjectState[] wayPoints)
        {
            double r = rand.Value.RandFloat();
            if (r < goalProbbality)
                return goal;
            else if (r < (wayPointProbbality + goalProbbality) && useERrrt)
            {
                int l = rand.Value.RandInt() % numWayPoints;
                if (wayPoints[l] != null)
                    return wayPoints[l];

            }
            return RandomState();
        }
        public void FindPath(VectorF2D init, VectorF2D goal, Obstacles obs)
        {
            Field.X = FieldConfig.Default.FieldLength / 2f + FieldConfig.Default.BoundaryWidth;
            Field.Y = FieldConfig.Default.FieldWidth / 2f + FieldConfig.Default.BoundaryWidth;

        }
    }
}