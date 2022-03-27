using AtCoder;
using AtCoder.Internal;

namespace Kzrnm.Competitive
{
    public interface IReversable<T> where T : IGraphEdge
    {
        /// <summary>
        /// <paramref name="from"/> と <see cref="IGraphEdge.To"/> を逆にする。
        /// </summary>
        /// <returns><see cref="IGraphEdge.To"/> が <paramref name="from"/> になった <typeparamref name="T"/></returns>
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
        /// <summary>
        /// 部分木のサイズ
        /// </summary>
        int Size { get; }
    }
    [IsOperator]
    public interface IGraphBuildOperator<TGraph, TNode, TEdge>
    {
        TNode Node(int i, TEdge[] roots, TEdge[] children);
        TGraph Graph(TNode[] nodes, CSR<TEdge> edges);
    }
    [IsOperator]
    public interface ITreeBuildOperator<TTree, TNode, TEdge>
    {
        TNode TreeNode(int i, int size, TNode parent, TEdge parentEdge, TEdge[] children);
        TNode TreeRootNode(int i, int size, TEdge[] children);
        TTree Tree(TNode[] nodes, int root);
    }
    public interface IGraph<TNode, TEdge>
        where TNode : IGraphNode<TEdge>
        where TEdge : IGraphEdge
    {
        CSR<TEdge> Edges { get; }
        TNode[] AsArray();
        TNode this[int index] { get; }
        int Length { get; }
    }
    public interface ITreeGraph<TNode, TEdge>
        where TNode : ITreeNode<TEdge>
        where TEdge : IGraphEdge
    {
        int Root { get; }
        TNode[] AsArray();
        TNode this[int index] { get; }
        int Length { get; }
    }
}
