// using MRL.SSL.Common.Math;

namespace MRL.SSL.Common.KdTree
{
    class KdNode
    {
        public int SelfIndex { get; set; }
        public int ParentIndex { get; set; }
        public int LeftChildIndex { get; set; }
        public int RightChildIndex { get; set; }
        
        public KdNode(int selfIndex)
        {
            SelfIndex = selfIndex;
            ParentIndex = LeftChildIndex = RightChildIndex = -1;
        }

        public KdNode(int selfIndex,int parentIndex)
        {
            SelfIndex = selfIndex;
            ParentIndex = parentIndex;
            LeftChildIndex = RightChildIndex = -1;
        }
    }
    /*public class KdNode
    {
        public static KdNode NullNode { get => new KdNode(); }

        VectorF2D minv, maxv; // bounding box of subtree
        SingleObjectState states;      // list of states stored at this node
        int numStates;     // number of states of this subtree
        KdNode[] child;

        /// <summary>
        /// Min bound of subtree
        /// </summary>
        public VectorF2D Minv { get => minv; set => minv = value; }
        /// <summary>
        /// Max bound of subtree
        /// </summary>
        public VectorF2D Maxv { get => maxv; set => maxv = value; }
        public SingleObjectState States { get => states; set => states = value; }
        public int NumStates { get => numStates; set => numStates = value; }
        public KdNode[] Child { get => child; set => child = value; }
        /// <summary>
        /// First Child (child[0])
        /// </summary>
        public KdNode Next { get => child[0]; set => child[0] = value; }

        public KdNode(VectorF2D minv=null, VectorF2D maxv=null,SingleObjectState state=null)
        {
            this.minv = minv;
            this.maxv = maxv;
            this.states = state;
            child = new KdNode[2];
        }
    }*/
}