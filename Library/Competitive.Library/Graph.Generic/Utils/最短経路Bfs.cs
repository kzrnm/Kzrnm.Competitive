using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public static class 最短経路Bfs
    {
        /// <summary>
        /// <paramref name="from"/> からの最短経路長をBfsで求める。到達判定にも有用
        /// </summary>
        public static uint[] ShortestPathBfs<TNode, TEdge>(this IGraph<TNode, TEdge> graph, int from)
            where TNode : IGraphNode<TEdge>
            where TEdge : IGraphEdge
        {
            var graphArr = graph.AsArray();
            var res = new uint[graphArr.Length];
            Array.Fill(res, uint.MaxValue);
            var queue = new Queue<int>();
            queue.Enqueue(from);
            res[from] = 0;
            while (queue.TryDequeue(out var cur))
            {
                foreach (var e in graphArr[cur].Children)
                {
                    var child = e.To;
                    if (res[child].UpdateMin(res[cur] + 1))
                        queue.Enqueue(child);
                }
            }
            return res;
        }
        /// <summary>
        /// <paramref name="from"/> から逆方向の最短経路長をBfsで求める。到達判定にも有用
        /// </summary>
        public static uint[] ShortestPathBfsReverse<TNode, TEdge>(this IGraph<TNode, TEdge> graph, int from)
            where TNode : IGraphNode<TEdge>
            where TEdge : IGraphEdge
        {
            var graphArr = graph.AsArray();
            var res = new uint[graphArr.Length];
            Array.Fill(res, uint.MaxValue);
            var queue = new Queue<int>();
            queue.Enqueue(from);
            res[from] = 0;
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                foreach (var e in graphArr[cur].Parents)
                {
                    var child = e.To;
                    if (res[child].UpdateMin(res[cur] + 1))
                        queue.Enqueue(child);
                }
            }
            return res;
        }
    }
}
