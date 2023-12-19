using System.Collections.Generic;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public record struct MstResult<T, TEdge>((int from, TEdge edge)[] Edges, T Cost);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "いらん")]
    internal struct MstBuilder<T, TEdge>
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        where TEdge : IWGraphEdge<T>
    {
        List<(int from, TEdge edge)> Edges;
        T Cost;
        public MstBuilder(int size)
        {
            Edges = new(size);
            Cost = T.AdditiveIdentity;
        }

        [凾(256)]
        public void Add((int from, TEdge edge) et)
        {
            Edges.Add(et);
            Cost += et.edge.Value;
        }
        [凾(256)] public MstResult<T, TEdge> Build() => new(Edges.ToArray(), Cost);
    }
}
