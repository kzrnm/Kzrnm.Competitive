using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class 帰りもあるオイラーツアー<TEdge> where TEdge : IGraphEdge, IReversable<TEdge>
    {
        public readonly struct Event
        {
            public readonly bool isStart;
            public readonly int parent;
            public readonly TEdge edge;
            public Event(int parent, TEdge edge, bool isStart)
            {
                this.parent = parent;
                this.edge = edge;
                this.isStart = isStart;
            }
            [凾(256)]
            public Event Reverse() => new Event(parent, edge, !isStart);
            [凾(256)]
            public (int From, int To) Route() => isStart ? (parent, edge.To) : (edge.To, parent);
            public override string ToString() => $"{parent}{(isStart ? '→' : '←')}{edge}";
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

        public 帰りもあるオイラーツアー((int left, int right)[] nodes, Event[] events)
        {
            this.nodes = nodes;
            Events = events;
        }

        /// <summary>
        /// <para>オイラーツアーを求める。</para>
        /// <para>根から各ノードを深さ優先探索するとき、ノードに入る/出るをイベント化したときのインデックスを返す。</para>
        /// </summary>
        public static 帰りもあるオイラーツアー<TEdge> Create<TNode>(ITreeGraph<TNode, TEdge> tree)
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
            return new 帰りもあるオイラーツアー<TEdge>(nodes, events);
        }
    }
    public static class オイラーツアーExt
    {
        /// <summary>
        /// <para>オイラーツアーを求める。</para>
        /// <para>根から各ノードを深さ優先探索するとき、ノードに入る/出るをイベント化したときのインデックスを返す。</para>
        /// </summary>
        public static 帰りもあるオイラーツアー<TEdge> EulerianTour<TNode, TEdge>(this ITreeGraph<TNode, TEdge> tree)
            where TNode : ITreeNode<TEdge>
            where TEdge : IGraphEdge, IReversable<TEdge> => 帰りもあるオイラーツアー<TEdge>.Create(tree);
    }
}
