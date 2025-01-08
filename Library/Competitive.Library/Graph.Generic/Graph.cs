using AtCoder.Internal;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// グラフを表す
    /// </summary>
    /// <typeparam name="Tn">ノードの型</typeparam>
    /// <typeparam name="Te">辺の型</typeparam>
    /// <param name="Nodes">ノード</param>
    /// <param name="Edges">辺</param>
    [DebuggerDisplay(nameof(Length) + " = {" + nameof(Length) + ",nq}")]
    public record SimpleGraph<Tn, Te>(
        [property: DebuggerBrowsable(DebuggerBrowsableState.RootHidden)] Tn[] Nodes,
        Csr<Te> Edges) : IGraph<SimpleGraph<Tn, Te>, Tn, Te>
        where Tn : GraphNode<Te>
    {
        [凾(256)]
        public Tn[] AsArray() => Nodes;
        [凾(256)]
        static SimpleGraph<Tn, Te> IGraph<SimpleGraph<Tn, Te>, Tn, Te>.Graph(Tn[] nodes, Csr<Te> edges)
            => new(nodes, edges);

        public Tn this[int index] { [凾(256)] get => Nodes[index]; }
        public int Length => Nodes.Length;
    }
    /// <summary>
    /// 木を表す
    /// </summary>
    /// <typeparam name="Tt">ノードの型</typeparam>
    /// <typeparam name="Te">辺の型</typeparam>
    /// <param name="Nodes">木のノード</param>
    /// <param name="Root">根となるノードのインデックス</param>
    /// <param name="HlDecomposition">HL分解</param>
    [DebuggerDisplay(nameof(Length) + " = {" + nameof(Length) + ",nq}")]
    public record TreeGraph<Tt, Te>(
        [property: DebuggerBrowsable(DebuggerBrowsableState.RootHidden)] Tt[] Nodes,
        int Root,
        HeavyLightDecomposition<Tt, Te> HlDecomposition) : ITreeGraph<TreeGraph<Tt, Te>, Tt, Te>
        where Tt : ITreeNode<Te>
        where Te : IGraphEdge
    {
        [凾(256)]
        public Tt[] AsArray() => Nodes;
        public Tt this[int index] { [凾(256)] get => Nodes[index]; }
        public int Length => Nodes.Length;

        [凾(256)]
        static TreeGraph<Tt, Te> ITreeGraph<TreeGraph<Tt, Te>, Tt, Te>.Tree(Tt[] nodes, int root, HeavyLightDecomposition<Tt, Te> hl)
            => new(nodes, root, hl);
    }
}
