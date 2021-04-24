using System;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.Math.Helpers;

namespace MRL.SSL.Common.KdTree
{
    public class KdTree
    {
        KdNode root;
        int leafSize;
        int maxDepth;
        int tests;

        public void Clear()
        {
            if (root == null) return;
            if (root.Child[0] != null) Clear(root.Child[0]);
            if (root.Child[1] != null) Clear(root.Child[1]);

            root.Child[0] = null;
            root.Child[1] = null;

            root.States = null;
            root.NumStates = 0;
            root = null;
        }

        public void SetDim(VectorF2D minv, VectorF2D maxv, int leafSize_n, int maxDepth_n)
        {
            Clear();
            root = new KdNode(minv, maxv);
            leafSize = leafSize_n;
            maxDepth = maxDepth_n;
        }

        public bool Add(SingleObjectState state)
        {
            KdNode p = root;
            int c, level = 0;

            if (p == null || !IsInside(p.Minv, p.Maxv, state)) return false;

            // go down tree to see where new state should go
            while (p.Child[0] != null)
            { // implies p->child[1] also
                c = Convert.ToInt32(!IsInside(p.Child[0].Minv, p.Child[0].Maxv, state));
                p = p.Child[c];
                level++;
            }

            // add it to leaf; and split leaf if too many children
            state.Child = p.States;
            p.States = state;
            p.NumStates++;

            // split leaf if not too deep and too many children for one node
            if (level < maxDepth && p.NumStates > leafSize)
            {
                Split(p, level % 2);
            }
            return true;
        }

        public SingleObjectState Nearest(float dist, VectorF2D x)
        {
            SingleObjectState best;

            dist = 4000;
            tests = 0;
            best = Nearest(root, dist, x);
            // printf("tests=%d dist=%f\n\n",tests,best_dist);

            return best;
        }

        private SingleObjectState Nearest(KdNode t, float best_dist, VectorF2D x, SingleObjectState best = null)
        {
            float d;
            float[] dc = new float[2];
            SingleObjectState p;
            bool c;

            // look at states at current node
            p = t.States;
            while (p != null)
            {
                d = VectorF2D.Distance(p.Location, x);
                if (d < best_dist)
                {
                    best = p;
                    best_dist = d;
                }
                tests++;
                p = p.Child;
            }

            // recurse on children (nearest first to maximize pruning)
            if (t.Child[0] != null)
            { // implies t->child[1]
                dc[0] = BoxDistance(t.Child[0].Minv, t.Child[0].Maxv, x);
                dc[1] = BoxDistance(t.Child[1].Minv, t.Child[1].Maxv, x);
                c = dc[1] < dc[0]; // c indicates nearest lower bound distance child
                int ic = Convert.ToInt32(c), ic_p = Convert.ToInt32(!c);
                if (dc[ic] < best_dist) best = Nearest(t.Child[ic], best_dist, x, best);
                if (dc[ic_p] < best_dist) best = Nearest(t.Child[ic_p], best_dist, x, best);
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

        private bool IsInside(VectorF2D minv, VectorF2D maxv, SingleObjectState state)
        {
            return (state.Location.X > minv.X && state.Location.Y > minv.Y &&
                    state.Location.X < maxv.X && state.Location.Y < maxv.Y);
        }

        private void Split(KdNode t, int split_dim)
        {
            KdNode a = new KdNode(t.Minv, t.Maxv), b = new KdNode(t.Minv, t.Maxv);
            SingleObjectState p, n;
            float split_val;

            if (split_dim == 0)
            {
                split_val = (t.Minv.X + t.Maxv.X) / 2;
                a.Maxv.X = b.Minv.X = split_val;
            }
            else
            {
                split_val = (t.Minv.Y + t.Maxv.Y) / 2;
                a.Maxv.Y = b.Minv.Y = split_val;
            }

            // separate children based on split
            n = t.States;
            while (n != null)
            {
                p = n;
                n = n.Child;

                if (((split_dim == 0) ? p.Location.X : p.Location.Y) < split_val)
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

        private float BoxDistance(VectorF2D minv, VectorF2D maxv, VectorF2D p)
        {
            float dx, dy;

            dx = p.X - MathHelper.BoundF(p.X, minv.X, maxv.X);
            dy = p.Y - MathHelper.BoundF(p.Y, minv.Y, maxv.Y);

            return MathF.Sqrt(dx * dx + dy * dy);
        }
    }
}