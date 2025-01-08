using AtCoder.Internal;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class ___GraphWeighted
    {
        public static IGraph<TEdge> AsUnweighted<T, TEdge>(this IWGraph<T, TEdge> g)
            where TEdge : IWGraphEdge<T>
            => g;

        public static ITreeGraph<TNode, TEdge> AsUnweighted<T, TNode, TEdge>(this IWTreeGraph<T, TNode, TEdge> g)
            where TNode : ITreeNode<TEdge>
            where TEdge : IWGraphEdge<T>
            => g;

        [DebuggerDisplay("{" + nameof(To) + "}")]
        public readonly record struct OneEdge(int To) : IGraphEdge<OneEdge>, IWGraphEdge<uint>
        {
            public static OneEdge None => new(-1);
            [凾(256)] public static implicit operator int(OneEdge e) => e.To;
            [凾(256)] public OneEdge Reversed(int from) => new(from);
            uint IWGraphEdge<uint>.Value => 1;
        }
        private class Weighted : SimpleGraph<GraphNode<OneEdge>, OneEdge>, IWGraph<uint, OneEdge>
        {
            public Weighted(GraphNode<OneEdge>[] array, Csr<OneEdge> edges) : base(array, edges)
            {
            }
        }
        public static IWGraph<uint, OneEdge> ToWeighted(this IGraph<GraphEdge> graph)
            => new Weighted(Unsafe.As<GraphNode<OneEdge>[]>(graph.AsArray()), Unsafe.As<Csr<OneEdge>>(graph.Edges));
    }
}
