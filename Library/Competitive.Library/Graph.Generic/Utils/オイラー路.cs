using AtCoder.Internal;
using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public static class オイラー路
    {
        /// <summary>
        /// <para>オイラー路(一筆書きのこと)を求める。</para>
        /// <para>無向オイラーグラフ: 次数(出ていく辺の数)が全て偶数</para>
        /// <para>無向準オイラーグラフ: 次数(出ていく辺の数)が奇数なのが2個</para>
        /// <para>有向オイラーグラフ: ある頂点で入次数が出次数より1多く、別の頂点で出次数が入次数より1多く、ほかの全ての頂点で入次数と出次数が等しい</para>
        /// <para>有向準オイラーグラフ: 全ての頂点で入次数と出次数が等しい</para>
        /// </summary>
        /// <returns>スタート地点とオイラー路を返す。見つからなかったら (-1, null) を返す。</returns>
        public static (int from, TEdge[] trail) EulerianTrail<TEdge>(this IGraph<TEdge> graph)
            where TEdge : struct, IGraphEdge<TEdge>
        {
            if (graph[0].IsDirected)
                return EulerianTrailDirected(graph);
            return EulerianTrailUndirected(graph);
        }

        /// <summary>
        /// <para>無向グラフのオイラー路を求める</para>
        /// <para>無向オイラーグラフ: 次数(出ていく辺の数)が全て偶数</para>
        /// <para>無向準オイラーグラフ: 次数(出ていく辺の数)が奇数なのが2個</para>
        /// </summary>
        private static (int from, TEdge[] trail) EulerianTrailUndirected<TEdge>(this IGraph<TEdge> graph)
            where TEdge : struct, IGraphEdge<TEdge>
        {
            Contract.Assert(!graph[0].IsDirected);
            var start = 0;
            var oddCount = 0;

            for (int i = 0; i < graph.Length; i++)
            {
                var node = graph[i];
                if ((node.Children.Length & 1) == 1)
                {
                    if (++oddCount > 2)
                        return (-1, null);
                    start = i;
                }
            }

            if (oddCount == 0 || oddCount == 2)
                return (start, EulerianTrail(graph, start));
            return (-1, null);
        }

        /// <summary>
        /// <para>有向グラフのオイラー路を求める</para>
        /// <para>有向オイラーグラフ: ある頂点で出次数が入次数より1多く、別の頂点で入次数が出次数より1多く、ほかの全ての頂点で入次数と出次数が等しい</para>
        /// <para>有向準オイラーグラフ: 全ての頂点で入次数と出次数が等しい</para>
        /// </summary>
        private static (int from, TEdge[] trail) EulerianTrailDirected<TEdge>(this IGraph<TEdge> graph)
            where TEdge : struct, IGraphEdge<TEdge>
        {
            Contract.Assert(graph[0].IsDirected);
            var start = -1;
            var goal = -1;

            for (int i = 0; i < graph.Length; i++)
            {
                var node = graph[i];
                var inCnt = node.Parents.Length;
                var outCnt = node.Children.Length;
                if (inCnt == outCnt)
                    continue;
                else if (inCnt + 1 == outCnt)
                {
                    if (start >= 0)
                        return (-1, null);
                    start = i;
                }
                else if (inCnt == outCnt + 1)
                {
                    if (goal >= 0)
                        return (-1, null);
                    goal = i;
                }
                else
                    return (-1, null);
            }

            if (start == -1 || goal == -1)
                return (0, EulerianTrail(graph, 0));
            if (start >= 0 || goal >= 0)
                return (start, EulerianTrail(graph, start));
            return (-1, null);
        }

        /// <summary>
        /// <para>オイラー路を求める。</para>
        /// <para><paramref name="from"/> からの一筆書きのこと</para>
        /// </summary>
        public static TEdge[] EulerianTrail<TEdge>(this IGraph<TEdge> graph, int from)
            where TEdge : struct, IGraphEdge<TEdge>
        {
            var isDirected = graph[from].IsDirected;
            var graphQueue = new Queue<EdgeInternal<TEdge>>[graph.Length];
            for (int i = 0; i < graph.Length; i++)
                graphQueue[i] = new Queue<EdgeInternal<TEdge>>(graph[i].Children.Length);
            for (int i = 0; i < graph.Length; i++)
            {
                foreach (var e in graph[i].Children)
                {
                    var child = e.To;
                    var ein = new EdgeInternal<TEdge>(i, e);
                    if (isDirected || i == child)
                    {
                        graphQueue[i].Enqueue(ein);
                    }
                    else if (i < child)
                    {
                        graphQueue[i].Enqueue(ein);
                        graphQueue[child].Enqueue(ein);
                    }
                }
            }
            return EulerianTrail(graphQueue, from);
        }

        private static TEdge[] EulerianTrail<TEdge>(Queue<EdgeInternal<TEdge>>[] graph, int from)
            where TEdge : struct, IGraphEdge<TEdge>
        {
            var res = new List<TEdge>();
            var idx = new Stack<TEdge>();
            while (graph[from].Count > 0)
            {
                do
                {
                    var ein = graph[from].Dequeue();
                    if (ein.Used) continue;
                    ein.Used = true;
                    idx.Push(ein.Edge(from));
                    break;
                } while (graph[from].Count > 0);
            Dfs:
                while (idx.Count > 0)
                {
                    var cur = idx.Peek().To;
                    while (graph[cur].Count > 0)
                    {
                        var ein = graph[cur].Dequeue();
                        if (ein.Used) continue;
                        ein.Used = true;
                        idx.Push(ein.Edge(cur));
                        goto Dfs;
                    }
                    res.Add(idx.Pop());
                }
            }

            var resArr = res.AsSpan().ToArray();
            Array.Reverse(resArr);
            return resArr;
        }
        private class EdgeInternal<TEdge> where TEdge : IGraphEdge<TEdge>
        {
            public readonly int From;
            public readonly TEdge ToEdge;
            public bool Used = false;
            public EdgeInternal(int from, TEdge edge)
            {
                From = from;
                ToEdge = edge;
            }

            public TEdge Edge(int from)
            {
                if (from == From)
                    return ToEdge;
                return ToEdge.Reversed(From);
            }
        }
    }
}
