using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    public class オイラーツアー<TEdge> where TEdge : IGraphEdge, IReversable<TEdge>
    {
        public readonly struct Event
        {
            public readonly bool isStart;
            public readonly int root;
            public readonly TEdge edge;
            public Event(int root, TEdge edge, bool isStart)
            {
                this.root = root;
                this.edge = edge;
                this.isStart = isStart;
            }
            public Event Reverse() => new Event(root, edge, !isStart);
            public override string ToString() => $"{root}{(isStart ? '→' : '←')}{edge}";
        }
        /// <summary>
        /// <para>根から各ノードを深さ優先探索するとき、ノードに入る/出るをイベント化したときのインデックスを返す。</para>
        /// </summary>
        private readonly (int left, int right)[] nodes;
        /// <summary>
        /// <para>根から各ノードを深さ優先探索するとき、ノードに入る/出るをイベント化したときのインデックスを返す。</para>
        /// </summary>
        public (int left, int right) this[int index] => nodes[index];

        public Event[] Events;

        public オイラーツアー((int left, int right)[] nodes, Event[] events)
        {
            this.nodes = nodes;
            this.Events = events;
        }

        /// <summary>
        /// <para>オイラーツアーを求める。</para>
        /// <para>根から各ノードを深さ優先探索するとき、ノードに入る/出るをイベント化したときのインデックスを返す。</para>
        /// </summary>
        public static オイラーツアー<TEdge> Create<TNode>(ITreeGraph<TNode, TEdge> tree)
            where TNode : ITreeNode<TEdge>
        {
            var treeArr = tree.AsArray();
            var root = tree.Root;

            var cnt = 0;
            var nodes = new (int l, int r)[treeArr.Length];
            var events = new Event[2 * treeArr.Length];

            var nodeEvents = new Event[treeArr.Length];
            nodeEvents[root] = new Event(-1, default(TEdge).Reversed(root), true);

            var idx = new Stack<(int index, int ci)>(treeArr.Length);
            idx.Push((root, 0));
            while (idx.Count > 0)
            {
                var (index, ci) = idx.Pop();
                var children = treeArr[index].Children;

                if (ci == 0)
                    events[nodes[index].l = cnt++] = nodeEvents[index];
                if (ci < children.Length)
                {
                    var to = children[ci].To;
                    nodeEvents[to] = new Event(index, children[ci], true);
                    idx.Push((index, ci + 1));
                    idx.Push((to, 0));
                }
                else
                    events[nodes[index].r = cnt++] = nodeEvents[index].Reverse();
            }

            /* 再帰版
            void Dfs(int index)
            {
                res[index].l = cnt++;
                foreach (var ch in tree[index].children)
                    Dfs(ch);
                res[index].r = cnt++;
            }
            Dfs(root);
            */
            return new オイラーツアー<TEdge>(nodes, events);
        }

        /// <summary>
        /// <paramref name="u"/>と<paramref name="v"/>の最小共通祖先を返します。
        /// </summary>
        public int LowestCommonAncestor(int u, int v)
        {
            if (u == v) return u;
            var f = nodes[u].left;
            var t = nodes[v].left;
            if (t < f) (f, t) = (t, f);
            return LowestCommonAncestorTable[f..t].Node;
        }

        private SparseTable<(int Node, int Depth), NodeMinOp> _LowestCommonAncestorTable;
        private SparseTable<(int Node, int Depth), NodeMinOp> LowestCommonAncestorTable
            => _LowestCommonAncestorTable ??= BuildLowestCommonAncestorTable();
        private SparseTable<(int Node, int Depth), NodeMinOp> BuildLowestCommonAncestorTable()
        {
            var arr = new (int Node, int Depth)[Events.Length];
            int dep = -1;
            for (int i = 0; i < arr.Length; i++)
            {
                if (Events[i].isStart)
                    arr[i] = (Events[i].edge.To, ++dep);
                else
                    arr[i] = (Events[i].root, --dep);
            }
            return new SparseTable<(int Node, int Depth), NodeMinOp>(arr);
        }
        private struct NodeMinOp : ISparseTableOperator<(int Node, int Depth)>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public (int Node, int Depth) Operate((int Node, int Depth) x, (int Node, int Depth) y) => x.Depth <= y.Depth ? x : y;
        }
    }
    public static class オイラーツアーExt
    {
        /// <summary>
        /// <para>オイラーツアーを求める。</para>
        /// <para>根から各ノードを深さ優先探索するとき、ノードに入る/出るをイベント化したときのインデックスを返す。</para>
        /// </summary>
        public static オイラーツアー<TEdge> EulerianTour<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
            where TNode : ITreeNode<TEdge>
            where TEdge : IGraphEdge, IReversable<TEdge> => オイラーツアー<TEdge>.Create(tree);
    }
}
