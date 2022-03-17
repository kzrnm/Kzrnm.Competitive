using AtCoder.Internal;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class EdgeContainer<TEdge> where TEdge : IGraphEdge
    {
        public int Length { get; }
        public bool IsDirected { get; }
        public readonly SimpleList<(int from, TEdge edge)> edges;
        public readonly int[] sizes;
        public readonly int[] rootSizes;
        public EdgeContainer(int size, bool isDirected)
        {
            Length = size;
            IsDirected = isDirected;
            sizes = new int[size];
            rootSizes = isDirected ? new int[size] : sizes;
            edges = new SimpleList<(int from, TEdge edge)>(size);
        }
        [凾(256)]
        public void Add(int from, TEdge edge)
        {
            ++sizes[from];
            ++rootSizes[edge.To];
            edges.Add((from, edge));
        }

        [凾(256)]
        public CSR<TEdge> ToCSR() => new CSR<TEdge>(Length, edges);
    }
}
