using AtCoder.Internal;
using System.Collections.Generic;

namespace AtCoder
{
    public class EdgeContainer<TEdge> where TEdge : IEdge
    {
        public int Length { get; }
        public bool IsDirected { get; }
        public readonly List<(int from, TEdge edge)> edges = new List<(int from, TEdge edge)>();
        public readonly int[] sizes;
        public readonly int[] rootSizes;
        public EdgeContainer(int size, bool isDirected)
        {
            Length = size;
            IsDirected = isDirected;
            sizes = new int[size];
            rootSizes = isDirected ? new int[size] : sizes;
        }
        public void Add(int from, TEdge edge)
        {
            ++sizes[from];
            ++rootSizes[edge.To];
            edges.Add((from, edge));
        }

        public CSR<TEdge> ToCSR() => new CSR<TEdge>(Length, edges);
    }
}
