using System.Collections.Generic;

namespace AtCoder
{
    public static class 最小全域木BFS
    {
        /// <summary>
        /// 最小全域木をBFSで求める。長さを持たないグラフ用
        /// </summary>
        public static (int from, TEdge edge)[] MinimumSpanningTreeBFS<TNode, TEdge>(this IGraph<TNode, TEdge> graph)
            where TNode : INode<TEdge>
            where TEdge : IEdge
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
            return res.ToArray();
        }
    }
}
