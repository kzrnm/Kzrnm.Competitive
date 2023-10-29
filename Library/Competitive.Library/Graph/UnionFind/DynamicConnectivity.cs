#define DynamicConnectivity_TUPLEDIC
// https://qiita.com/hotman78/items/78cd3aa50b05a57738d4
using System.Collections.Generic;
using System.Linq;

using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{

    /// <summary>
    /// <para>削除可能 UnionFind を O(log^2 N) で操作できるデータ構造です。</para>
    /// </summary>
    public class DynamicConnectivity
    {
        internal class Node
        {
            public Node ch0;
            public Node ch1;
            public Node parent;
            public int l, r, size;
            internal byte flag;
            public bool Exact
            {
                [凾(256)]
                set
                {
                    if (value) flag |= 0b0001;
                    else flag &= unchecked((byte)~0b0001);
                }
                [凾(256)]
                get => (flag & 0b0001) > 0;
            }
            public bool ChildExact
            {
                [凾(256)]
                set
                {
                    if (value) flag |= 0b0010;
                    else flag &= unchecked((byte)~0b0010);
                }
                [凾(256)]
                get => (flag & 0b0010) > 0;
            }
            public bool EdgeConnected
            {
                [凾(256)]
                set
                {
                    if (value) flag |= 0b0100;
                    else flag &= unchecked((byte)~0b0100);
                }
                [凾(256)]
                get => (flag & 0b0100) > 0;
            }
            public bool ChildEdgeConnected
            {
                [凾(256)]
                set
                {
                    if (value) flag |= 0b1000;
                    else flag &= unchecked((byte)~0b1000);
                }
                [凾(256)]
                get => (flag & 0b1000) > 0;
            }
            public Node(int l, int r)
            {
                this.l = l;
                this.r = r;
                size = l == r ? 1 : 0;

                // Exact = ChildExact = l < r;
                if (l < r)
                    flag = 0b0011;
            }
            public bool IsRoot => parent == null;
        }
        class EulerianTourTree
        {
#if DynamicConnectivity_TUPLEDIC
            private Dictionary<(int, int), Node> ptr;
#else
            private Dictionary<int, Node>[] ptr;
#endif
            [凾(256)]
            private Node GetNode(int l, int r)
            {
#if DynamicConnectivity_TUPLEDIC
                if (ptr.TryGetValue((l, r), out var node))
                    return node;
                return ptr[(l, r)] = new Node(l, r);
#else
                var p = ptr[l];
                if (p.TryGetValue(r, out var node))
                    return node;
                return p[r] = new Node(l, r);
#endif
            }
            private Node Root(Node t)
            {
                if (t == null) return null;
                while (t.parent != null) t = t.parent;
                return t;
            }

            [凾(256)]
            bool Same(Node s, Node t)
            {
                if (s != null) Splay(s);
                if (t != null) Splay(t);
                return Root(s) == Root(t);
            }
            [凾(256)]
            Node ReRoot(Node t)
            {
                var (s1, s2) = Split(t);
                return Merge(s2, s1);
            }
            [凾(256)]
            (Node, Node) Split(Node s)
            {
                Splay(s);
                Node t = s.ch0;
                if (t != null) t.parent = null;
                s.ch0 = null;
                return (t, Update(s));
            }
            [凾(256)]
            (Node, Node) Split2(Node s)
            {
                Splay(s);
                Node t = s.ch0;
                Node u = s.ch1;
                if (t != null) t.parent = null;
                s.ch0 = null;
                if (u != null) u.parent = null;
                s.ch1 = null;
                return (t, u);
            }
            [凾(256)]
            (Node, Node, Node) Split(Node s, Node t)
            {
                var (u1, u2) = Split2(s);
                var same = Same(u1, t);
                var (r1, r2) = Split2(t);
                if (same)
                    return (r1, r2, u2);
                else
                    return (u1, r1, r2);
            }
            private Node Merge(Node s, Node t)
            {
                if (s == null) return t;
                if (t == null) return s;
                while (s.ch1 != null) s = s.ch1;
                Splay(s);
                s.ch1 = t;
                if (t != null) t.parent = s;
                return Update(s);
            }

            [凾(256)]
            private int Size(Node t) => t != null ? t.size : 0;
            [凾(256)]
            private Node Update(Node t)
            {
                t.size = Size(t.ch0) + Size(t.ch1) + (t.l == t.r ? 1 : 0);
                t.ChildEdgeConnected = (t.ch0 != null && t.ch0.ChildEdgeConnected) || (t.EdgeConnected) || (t.ch1 != null && t.ch1.ChildEdgeConnected);
                t.ChildExact = (t.ch0 != null && t.ch0.ChildExact) || (t.Exact) || (t.ch1 != null && t.ch1.ChildExact);
                return t;
            }
            [System.Diagnostics.Conditional("DEBUG")]
            private void Push(Node _)
            {
                //遅延評価予定
            }

            private void Rotate(Node t, bool b)
            {
                Node x = t.parent, y = x.parent;
                if (b)
                {
                    if ((x.ch0 = t.ch1) != null)
                        t.ch1.parent = x;
                    t.ch1 = x;
                }
                else
                {
                    if ((x.ch1 = t.ch0) != null)
                        t.ch0.parent = x;
                    t.ch0 = x;
                }
                x.parent = t;

                Update(x);
                Update(t);
                if ((t.parent = y) != null)
                {
                    if (y.ch0 == x) y.ch0 = t;
                    if (y.ch1 == x) y.ch1 = t;
                    Update(y);
                }
            }
            private void Splay(Node t)
            {
                Push(t);
                while (!t.IsRoot)
                {
                    Node q = t.parent;
                    if (q.IsRoot)
                    {
                        Push(q);
                        Push(t);
                        Rotate(t, q.ch0 == t);
                    }
                    else
                    {
                        Node r = q.parent;
                        Push(r);
                        Push(q);
                        Push(t);
                        bool b = r.ch0 == q;
                        if ((b ? q.ch0 : q.ch1) == t)
                        {
                            Rotate(q, b);
                            Rotate(t, b);
                        }
                        else
                        {
                            Rotate(t, !b);
                            Rotate(t, b);
                        }
                    }
                }
            }

            public List<(int f, int t)> Debug(Node n) => Debug(n, new List<(int f, int t)>());
            private List<(int f, int t)> Debug(Node n, List<(int f, int t)> list)
            {
                Debug(n.ch0, list);
                list.Add((n.l, n.r));
                Debug(n.ch1, list);
                return list;
            }

            public EulerianTourTree(int size)
            {
#if DynamicConnectivity_TUPLEDIC
                ptr = new Dictionary<(int, int), Node>();
                for (int i = 0; i < size; i++)
                    ptr[(i, i)] = new Node(i, i);
#else
                ptr = new Dictionary<int, Node>[size];
                for (int i = 0; i < size; i++)
                    ptr[i] = new Dictionary<int, Node> { [i] = new Node(i, i) };
#endif
            }

            [凾(256)]
            public int Size(int s)
            {
                Node t = GetNode(s, s);
                Splay(t);
                return t.size;
            }
            [凾(256)]
            public bool Same(int s, int t)
            {
                return Same(GetNode(s, s), GetNode(t, t));
            }
            [凾(256)]
            public void UpdateEdge(int s, UpdateEdgeStatus st)
            {
                void Dfs(Node t, UpdateEdgeStatus st)
                {
                    while (true)
                    {
                        if (t.l < t.r && t.Exact)
                        {
                            Splay(t);
                            t.Exact = false;
                            Update(t);
                            st.Link(t.l, t.r);
                            return;
                        }
                        if (t.ch0 != null && t.ch0.ChildExact)
                            t = t.ch0;
                        else
                            t = t.ch1;
                    }
                }

                Node t = GetNode(s, s);
                Splay(t);
                while (t != null && t.ChildExact)
                {
                    Dfs(t, st);
                    Splay(t);
                }
            }
            [凾(256)]
            public bool TryReconnect(int s, ReconnectStatus r)
            {
                bool Dfs(Node t, ReconnectStatus r)
                {
                    while (true)
                    {
                        if (t.EdgeConnected)
                        {
                            Splay(t);
                            return r.F(t.l);
                        }
                        if (t.ch0 != null && t.ch0.ChildEdgeConnected)
                            t = t.ch0;
                        else
                            t = t.ch1;
                    }
                }
                Node t = GetNode(s, s);
                Splay(t);
                while (t.ChildEdgeConnected)
                {
                    if (Dfs(t, r)) return true;
                    Splay(t);
                }
                return false;
            }
            [凾(256)]
            public void UpdateEdgeConnected(int s, bool b)
            {
                Node t = GetNode(s, s);
                Splay(t);
                t.EdgeConnected = b;
                Update(t);
            }
            [凾(256)]
            public bool Link(int l, int r)
            {
                if (Same(l, r)) return false;
                Merge(
                    ReRoot(GetNode(l, l)),
                    Merge(GetNode(l, r),
                        Merge(ReRoot(GetNode(r, r)), GetNode(r, l))));
                return true;
            }
            [凾(256)]
            public bool Cut(int l, int r)
            {
#if DynamicConnectivity_TUPLEDIC
                if (!ptr.ContainsKey((l, r))) return false;
                var (s, _, u) = Split(GetNode(l, r), GetNode(r, l));
                Merge(s, u);
                ptr.Remove((l, r));
                ptr.Remove((r, l));
#else
                if (!ptr[l].ContainsKey(r)) return false;
                var (s, _, u) = Split(GetNode(l, r), GetNode(r, l));
                Merge(s, u);
                ptr[l].Remove(r);
                ptr[r].Remove(l);
#endif
                return true;
            }
        }

        private int _size;
        private List<EulerianTourTree> ett;
        private List<HashSet<int>[]> edges;

        public DynamicConnectivity(int size)
        {
            _size = size;
            ett = new List<EulerianTourTree> { new EulerianTourTree(size) };
            edges = new List<HashSet<int>[]> { new HashSet<int>[size] };
        }

        /// <summary>
        /// 頂点 <paramref name="s"/> と頂点 <paramref name="t"/> を結ぶ辺を追加します。既に同じ木に追加済みならば false、新たに追加されたならば true を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="s"/>, <paramref name="t"/>&lt;n</para>
        /// <para>計算量: O(log^2 n)</para>
        /// </remarks>
        [凾(256)]
        public bool Link(int s, int t)
        {
            if (s == t) return false;
            var ett0 = ett[0];
            if (ett0.Link(s, t)) return true;

            var edges0 = edges[0];
            if (edges0[s] == null) edges0[s] = new HashSet<int>();
            if (edges0[t] == null) edges0[t] = new HashSet<int>();
            edges0[s].Add(t);
            edges0[t].Add(s);
            if (edges0[s].Count == 1) ett0.UpdateEdgeConnected(s, true);
            if (edges0[t].Count == 1) ett0.UpdateEdgeConnected(t, true);
            return false;
        }

        /// <summary>
        /// 頂点 <paramref name="s"/>, <paramref name="t"/> が連結かどうかを返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="s"/>, <paramref name="t"/>&lt;n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [凾(256)]
        public bool Same(int s, int t) => ett[0].Same(s, t);
        /// <summary>
        /// <paramref name="s"/> と連結している頂点の数を返します。
        /// </summary>
        [凾(256)]
        public int Size(int s) => ett[0].Size(s);

        //private int[] GetVertex(int s)
        //{
        //    //return ett[0].vertex_list(s);
        //}

        /// <summary>
        /// 頂点 <paramref name="s"/> と頂点 <paramref name="t"/> を結ぶ辺を削除します。新たに分離されたならば true を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="s"/>, <paramref name="t"/>&lt;n</para>
        /// <para>計算量: O(log^2 n)</para>
        /// </remarks>
        [凾(256)]
        public bool Cut(int s, int t)
        {
            if (s == t) return false;
            for (int i = 0; i < edges.Count; i++)
            {
                var edge = edges[i];
                var edgeS = edge[s];
                var edgeT = edge[t];
                if (edgeS == null) edge[s] = new HashSet<int>();
                else
                {
                    edgeS.Remove(t);
                    if (edge[s].Count == 0) ett[i].UpdateEdgeConnected(s, false);
                }
                if (edgeT == null) edge[t] = new HashSet<int>();
                else
                {
                    edgeT.Remove(s);
                    if (edge[t].Count == 0) ett[i].UpdateEdgeConnected(t, false);
                }
            }
            for (int i = edges.Count - 1; i >= 0; i--)
            {
                if (ett[i].Cut(s, t))
                {
                    if (edges.Count - 1 == i)
                    {
                        ett.Add(new EulerianTourTree(_size));
                        edges.Add(new HashSet<int>[_size]);
                    }
                    return !TryReconnect(s, t, i);
                }
            }
            return false;
        }
        struct UpdateEdgeStatus
        {
            public EulerianTourTree etti1;
            public readonly bool Link(int s, int t) => etti1.Link(s, t);
        }
        struct ReconnectStatus
        {
            public List<EulerianTourTree> ett;
            public List<HashSet<int>[]> edges;
            public int index;
            public readonly bool F(int x)
            {
                var etti = ett[index];
                var etti1 = ett[index + 1];
                var edge = edges[index];
                var edge1 = edges[index + 1];

                var edgeX = edge[x];
                if (edgeX == null)
                    edge[x] = edgeX = new HashSet<int>();
                var used = new List<int>();
                var ys = edgeX.ToArray();
                foreach (var y in ys)
                {
                    used.Add(y);
                    var edgeY = edge[y];
                    if (edgeY == null) edgeY = new HashSet<int>();
                    edgeX.Remove(y);
                    edgeY.Remove(x);
                    if (edgeX.Count == 0) etti.UpdateEdgeConnected(x, false);
                    if (edgeY.Count == 0) etti.UpdateEdgeConnected(y, false);
                    if (etti.Same(x, y))
                    {
                        if (edge1[x] == null)
                            edge1[x] = new HashSet<int>();
                        edge1[x].Add(y);
                        if (edge1[y] == null)
                            edge1[y] = new HashSet<int>();
                        edge1[y].Add(x);
                        if (edge1[x].Count == 1) etti1.UpdateEdgeConnected(x, true);
                        if (edge1[y].Count == 1) etti1.UpdateEdgeConnected(y, true);
                    }
                    else
                    {
                        for (int j = 0; j <= index; j++)
                            ett[j].Link(x, y);
                        return true;
                    }
                }
                edgeX.Clear();
                return false;
            }
        }
        private bool TryReconnect(int s, int t, int k)
        {
            for (int i = 0; i < k; i++)
                ett[i].Cut(s, t);
            for (int i = k; i >= 0; i--)
            {
                var etti = ett[i];
                var etti1 = ett[i + 1];
                if (etti.Size(s) > etti.Size(t))
                    (s, t) = (t, s);
                etti.UpdateEdge(s, new UpdateEdgeStatus { etti1 = etti1 });
                if (etti.TryReconnect(s, new ReconnectStatus
                {
                    index = i,
                    ett = ett,
                    edges = edges,
                })) return true;
            }
            return false;
        }
    }
}
