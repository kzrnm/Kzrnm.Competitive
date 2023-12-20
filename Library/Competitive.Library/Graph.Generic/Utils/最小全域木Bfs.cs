using System.Collections.Generic;

namespace Kzrnm.Competitive
{
    public static class 最小全域木Bfs
    {
        /// <summary>
        /// 最小全域木をBfsで求める。長さを持たないグラフ用
        /// </summary>
        public static MstResult<int, TEdge> MinimumSpanningTreeBfs<TEdge>(this IGraph<TEdge> graph)
            where TEdge : IGraphEdge
        {
            var sumi = new bool[graph.Length];
            var res = new List<(int from, TEdge edge)>(graph.Length);
            var queue = new Queue<int>(graph.Length);
            queue.Enqueue(0);
            sumi[0] = true;
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                sumi[cur] = true;
                foreach (var e in graph[cur].Children)
                {
                    var child = e.To;
                    if (!sumi[child])
                    {
                        sumi[child] = true;
                        res.Add((cur, e));
                        queue.Enqueue(child);
                    }
                }
            }
            return new MstResult<int, TEdge>(res.ToArray(), res.Count);
        }
    }
}
