using AtCoder.Internal;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class ___GraphToWeighted
    {

        [DebuggerDisplay("{" + nameof(To) + "}")]
        public readonly record struct OneEdge(int To) : IGraphEdge<OneEdge>, IWGraphEdge<uint>
        {
            public static OneEdge None => new(-1);
            [凾(256)] public static implicit operator int(OneEdge e) => e.To;
            [凾(256)] public OneEdge Reversed(int from) => new(from);
            uint IWGraphEdge<uint>.Value => 1;
        }
        private class InnerWeighted : SimpleGraph<OneEdge>, IWGraph<uint, OneEdge>
        {
            public InnerWeighted(GraphNode<OneEdge>[] array, Csr<OneEdge> edges) : base(array, edges)
            {
            }
        }
        public static IWGraph<uint, OneEdge> ToWeighted(this SimpleGraph<GraphEdge> graph)
         => new InnerWeighted(Unsafe.As<GraphNode<OneEdge>[]>(graph.Nodes), Unsafe.As<Csr<OneEdge>>(graph.Edges));
    }
}
