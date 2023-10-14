namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE 最小全域木の結果
    public struct MstResult<T, TEdge>
        where TEdge : IWGraphEdge<T>
    {
        public T Cost;
        public (int from, TEdge edge)[] Edges;
    }
}
