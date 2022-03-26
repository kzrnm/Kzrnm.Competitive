namespace Kzrnm.Competitive
{
    public interface IReversable<T> where T : IGraphEdge
    {
        T Reversed(int from);
    }
    public interface IGraphEdge
    {
        /// <summary>
        /// 向き先
        /// </summary>
        int To { get; }
    }
    public interface IWGraphEdge<T> : IGraphEdge
    {
        /// <summary>
        /// 重み
        /// </summary>
        T Value { get; }
    }
    public interface IGraphData<T> : IGraphEdge
    {
        /// <summary>
        /// 自由なデータ
        /// </summary>
        T Data { get; }
    }
    public interface IGraphNode<out TEdge> where TEdge : IGraphEdge
    {
        /// <summary>
        /// ノードのインデックス
        /// </summary>
        int Index { get; }
        /// <summary>
        /// 入ってくる辺の向いてる先
        /// </summary>
        TEdge[] Roots { get; }
        /// <summary>
        /// 出ている辺の向いてる先
        /// </summary>
        TEdge[] Children { get; }
        /// <summary>
        /// 有向グラフかどうか
        /// </summary>
        bool IsDirected { get; }
    }
    public interface ITreeNode<TEdge> where TEdge : IGraphEdge
    {
        /// <summary>
        /// ノードのインデックス
        /// </summary>
        int Index { get; }
        /// <summary>
        /// 親ノード
        /// </summary>
        TEdge Root { get; }
        /// <summary>
        /// 子ノード
        /// </summary>
        TEdge[] Children { get; }
        /// <summary>
        /// 何個遡ったら根になるか
        /// </summary>
        int Depth { get; }
    }
}
