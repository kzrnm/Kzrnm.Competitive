using AtCoder.Internal;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class ___GraphToWeighted
    {
        public readonly struct OneEdge : IGraphEdge<OneEdge>, IEquatable<OneEdge>, IWGraphEdge<uint>
        {
            public static OneEdge None { get; } = new OneEdge(-1);
            public OneEdge(int to)
            {
                To = to;
            }
            public int To { get; }
            uint IWGraphEdge<uint>.Value => 1;

            [凾(256)] public static implicit operator int(OneEdge e) => e.To;
            public override string ToString() => To.ToString();
            [凾(256)] public OneEdge Reversed(int from) => new OneEdge(from);

            public override int GetHashCode() => To;
            public override bool Equals(object obj) => obj is OneEdge edge && Equals(edge);
            public bool Equals(OneEdge other) => To == other.To;
            public static bool operator ==(OneEdge left, OneEdge right) => left.Equals(right);
            public static bool operator !=(OneEdge left, OneEdge right) => !left.Equals(right);
        }
        private class InnerWeighted : SimpleGraph<OneEdge>, IWGraph<uint, OneEdge>
        {
            public InnerWeighted(GraphNode<OneEdge>[] array, Csr<OneEdge> edges) : base(array, edges)
            {
            }
        }
        public static IWGraph<uint, OneEdge> ToWeighted(this SimpleGraph<GraphEdge> graph)
         => new InnerWeighted(Unsafe.As<GraphNode<OneEdge>[]>(graph.Nodes), Unsafe.As<Csr<OneEdge>>(graph.Edges));
        //var grrArr = graph.AsArray();
        //var isDirected = graph.Nodes[0].IsDirected;
        //var gb = new WGraphBuilder<int>(graph.Length, isDirected);
        //for (int i = 0; i < grrArr.Length; i++)
        //    foreach (int ch in grrArr[i].Children)
        //        if (isDirected || i <= ch)
        //            gb.Add(i, ch, 1);
        //return gb.ToGraph();
    }
}
