using System.Collections.Generic;
using AtCoder.Internal;

namespace AtCoder
{
    public static class 強連結成分分解
    {
        /// <summary>
        /// 強連結成分分解する
        /// </summary>
        public static List<List<int>> Scc<TNode, TEdge>(this IGraph<TNode, TEdge> graph)
            where TNode : INode<TEdge>
            where TEdge : IEdge
        {
            DebugUtil.Assert(graph[0].IsDirected);
            var sccGraph = new SCCGraph(graph.Length);
            for (int i = 0; i < graph.Length; i++)
                foreach (var e in graph[i].Children)
                    sccGraph.AddEdge(i, e.To);
            return sccGraph.SCC();
        }
    }
}
