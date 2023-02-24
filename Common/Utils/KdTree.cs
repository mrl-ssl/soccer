using System;
using System.Runtime.CompilerServices;
using MRL.SSL.Common.Math;

namespace MRL.SSL.Common.Utils
{
    public class KdTree
    {
        KdNode root;
        int leafSize;
        int maxDepth;
        public int Size { get; set; }

        public void Clear()
        {
            if (root == null) return;
            if (root.Child[0] != null) Clear(root.Child[0]);
            if (root.Child[1] != null) Clear(root.Child[1]);

            root.Child[0] = null;
            root.Child[1] = null;

            root.States = null;
            root.NumStates = 0;
            Size = 0;
        }


        public bool Add(SingleObjectState state)
        {
            KdNode p = root;
            int c, level = 0;

            if (p == null || !IsInside(p.Minv, p.Maxv, state)) return false;

            // go down tree to see where new state should go
            while (p.Child[0] != null)
            { // implies p->child[1] also
                c = IsInside(p.Child[0].Minv, p.Child[0].Maxv, state) ? 0 : 1;
                p = p.Child[c];
                level++;
            }

            // add it to leaf; and split leaf if too many children
            state.Child = p.States;
            p.States = state;
            p.NumStates++;

            Size++;

            // split leaf if not too deep and too many children for one node
            if (level < maxDepth && p.NumStates > leafSize)
            {
                Split(p, level % 2);
            }

            return true;
        }

        public SingleObjectState Nearest(out float dist, VectorF2D x)
        {
            SingleObjectState best = null;

            dist = float.MaxValue;
            best = Nearest(root, ref dist, best, x);

            return best;
        }
        public void SetDim(VectorF2D minv, VectorF2D maxv, int leafSizeN, int maxDepthN)
        {
            Clear();
            if (root == null)
                root = new KdNode(maxv);
            root.Maxv = maxv;
            root.Minv = minv;
            leafSize = leafSizeN;
            maxDepth = maxDepthN;
        }
        private SingleObjectState Nearest(KdNode t, ref float bestDist, SingleObjectState best, VectorF2D x)
        {
            float d;
            float[] dc = new float[2];

            // look at states at current node
            SingleObjectState p = t.States;
            while (p != null)
            {
                d = VectorF2D.Distance(p.Location, x);
                if (d < bestDist)
                {
                    best = p;
                    bestDist = d;
                }
                p = p.Child;

            }

            // recurse on children (nearest first to maximize pruning)
            if (t.Child[0] != null)
            { // implies t->child[1]
                dc[0] = BoxDistance(t.Child[0].Minv, t.Child[0].Maxv, x);
                dc[1] = BoxDistance(t.Child[1].Minv, t.Child[1].Maxv, x);
                int c = dc[1] < dc[0] ? 1 : 0;
                int ic = 1 - c;
                if (dc[c] < bestDist) best = Nearest(t.Child[c], ref bestDist, best, x);
                if (dc[ic] < bestDist) best = Nearest(t.Child[ic], ref bestDist, best, x);
            }

            return best;
        }

        private void Clear(KdNode node)
        {
            if (node == null) return;
            if (node.Child[0] != null) Clear(node.Child[0]);
            if (node.Child[1] != null) Clear(node.Child[1]);

            node.Child[0] = null;
            node.Child[1] = null;
            node.States = null;
            node.NumStates = 0;
            node = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsInside(VectorF2D minv, VectorF2D maxv, SingleObjectState state)
        {
            return (state.Location.X > minv.X && state.Location.Y > minv.Y &&
                    state.Location.X < maxv.X && state.Location.Y < maxv.Y);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float BoxDistance(VectorF2D minv, VectorF2D maxv, VectorF2D p)
        {
            var dx = p.X - MathHelper.BoundF(p.X, minv.X, maxv.X);
            var dy = p.Y - MathHelper.BoundF(p.Y, minv.Y, maxv.Y);

            return MathF.Sqrt(dx * dx + dy * dy);
        }
        private void Split(KdNode t, int splitDim)
        {
            var a = new KdNode(new VectorF2D(t.Minv.X, t.Minv.Y), new VectorF2D(t.Maxv.X, t.Maxv.Y));
            var b = new KdNode(new VectorF2D(t.Minv.X, t.Minv.Y), new VectorF2D(t.Maxv.X, t.Maxv.Y));
            SingleObjectState p, n;
            float splitVal;

            if (splitDim == 0)
            {
                splitVal = (t.Minv.X + t.Maxv.X) / 2f;
                a.Maxv.X = b.Minv.X = splitVal;
            }
            else
            {
                splitVal = (t.Minv.Y + t.Maxv.Y) / 2f;
                a.Maxv.Y = b.Minv.Y = splitVal;
            }

            // separate children based on split
            n = t.States;
            while (n != null)
            {
                p = n;
                n = n.Child;

                if (((splitDim == 0) ? p.Location.X : p.Location.Y) < splitVal)
                {
                    p.Child = a.States;
                    a.States = p;
                    a.NumStates++;
                }
                else
                {
                    p.Child = b.States;
                    b.States = p;
                    b.NumStates++;
                }
            }

            // insert into tree
            t.States = null;
            t.Child[0] = a;
            t.Child[1] = b;
        }


    }
}