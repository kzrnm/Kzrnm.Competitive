namespace Kzrnm.Competitive
{
    public static class ___GraphToWeighted
    {
        public static WGraph<int, WGraphNode<int, WEdge<int>>, WEdge<int>> ToWeighted(this SimpleGraph<GraphNode, GraphEdge> graph)
        {
            var grrArr = graph.AsArray();
            var isDirected = graph.Nodes[0].IsDirected;
            var gb = new WGraphBuilder<int>(graph.Length, isDirected);
            for (int i = 0; i < grrArr.Length; i++)
                foreach (int ch in grrArr[i].Children)
                    if (isDirected || i <= ch)
                        gb.Add(i, ch, 1);
            return gb.ToGraph();
        }
    }
}
