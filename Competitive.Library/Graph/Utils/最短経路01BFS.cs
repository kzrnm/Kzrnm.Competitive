using AtCoder;
using AtCoder.Stl;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public static class 最短経路01BFS
    {
        /// <summary>
        /// <para><paramref name="from"/> からの最短経路長を01-BSFで求める。</para>
        /// <para>計算量: O(|E| + |V|)</para>
        /// </summary>
        public static T[] ShortestPath01BFS<T, TOp, TNode, TEdge>(this IWGraph<T, TOp, TNode, TEdge> graph, int from)
            where T : struct
            where TOp : struct, INumOperator<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            TOp op = default;
            var graphArr = graph.AsArray();
            var INF = op.MaxValue;
            var res = Global.NewArray(graphArr.Length, INF);
            res[from] = default;

            var used = new bool[graphArr.Length];
            int count = 0;
            var remains = new Deque<int>(graphArr.Length) { from };

            while (remains.Count > 0)
            {
                var ix = remains.PopFirst();
                if (used[ix]) continue;
                used[ix] = true;
                if (++count >= graphArr.Length) break;
                var len = res[ix];
                foreach (var e in graphArr[ix].Children)
                {
                    var nextLength = len;
                    if (EqualityComparer<T>.Default.Equals(e.Value, default))
                    {
                        if (op.GreaterThan(res[e.To], nextLength))
                        {
                            res[e.To] = nextLength;
                            remains.AddFirst(e.To);
                        }
                    }
                    else
                    {
                        nextLength = op.Increment(nextLength);
                        if (op.GreaterThan(res[e.To], nextLength))
                        {
                            res[e.To] = nextLength;
                            remains.AddLast(e.To);
                        }
                    }
                }
            }
            return res;
        }
    }
}
