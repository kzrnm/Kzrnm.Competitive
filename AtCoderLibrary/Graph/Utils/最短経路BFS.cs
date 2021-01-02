using System.Collections.Generic;

namespace AtCoder
{
    public static class 最短経路BFS
    {
        /// <summary>
        /// <paramref name="from"/> からの最短経路長をBFSで求める。辺が長さを持たないグラフ用
        /// </summary>
        public static int[] ShortestPathBFS<TNode, TEdge>(this IGraph<TNode, TEdge> graph, int from)
            where TNode : INode<TEdge>
            where TEdge : IEdge
        {
            var res = Global.NewArray(graph.Length, int.MaxValue);
            var queue = new Queue<int>();
            queue.Enqueue(from);
            res[from] = 0;
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                foreach (var e in graph[cur].Children)
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
