namespace Kzrnm.Competitive
{
    public struct MstResult<T, TEdge>
        where TEdge : IWGraphEdge<T>
    {
        public T Cost;
        public (int from, TEdge edge)[] Edges;
    }
}
