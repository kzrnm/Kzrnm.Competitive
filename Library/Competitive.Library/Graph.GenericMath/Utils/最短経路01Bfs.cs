using AtCoder;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 最短経路(01-BFS)
    public static class 最短経路01Bfs
    {
        /// <summary>
        /// <para><paramref name="from"/> からの最短経路長を01-BSFで求める。</para>
        /// <para>計算量: O(|E| + |V|)</para>
        /// </summary>
        public static T[] ShortestPath01Bfs<T, TNode, TEdge>(this IWGraph<T, TNode, TEdge> graph, int from)
            where T : IMinMaxValue<T>, IIncrementOperators<T>, IComparable<T>
            where TNode : IGraphNode<TEdge>
            where TEdge : IWGraphEdge<T>
        {
            var graphArr = graph.AsArray();
            var INF = T.MaxValue;
            var res = new T[graphArr.Length];
            System.Array.Fill(res, INF);
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
                    var to = e.To;
                    var nextLength = len;
                    if (EqualityComparer<T>.Default.Equals(e.Value, default))
                    {
                        if (res[to].CompareTo(nextLength) > 0)
                        {
                            res[to] = nextLength;
                            remains.AddFirst(to);
                        }
                    }
                    else
                    {
                        if (res[to].CompareTo(++nextLength) > 0)
                        {
                            res[to] = nextLength;
                            remains.AddLast(to);
                        }
                    }
                }
            }
            return res;
        }
    }
}
