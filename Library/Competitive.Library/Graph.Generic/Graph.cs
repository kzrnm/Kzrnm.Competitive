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
    public class SimpleGraph<Tn, Te> : IGraph<SimpleGraph<Tn, Te>, Tn, Te>
        where Tn : GraphNode<Te>
    {
        public Csr<Te> Edges { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal Tn[] Nodes { get; }
        [凾(256)]
        public Tn[] AsArray() => Nodes;
        [凾(256)]
        static SimpleGraph<Tn, Te> IGraph<SimpleGraph<Tn, Te>, Tn, Te>.Graph(Tn[] nodes, Csr<Te> edges)
            => new(nodes, edges);

        public Tn this[int index] { [凾(256)] get => Nodes[index]; }
        public int Length => Nodes.Length;
        public SimpleGraph(Tn[] n, Csr<Te> e)
        {
            Nodes = n;
            Edges = e;
        }
    }
    /// <summary>
    /// 木を表す
    /// </summary>
    /// <typeparam name="Tt">ノードの型</typeparam>
    /// <typeparam name="Te">辺の型</typeparam>
    public class TreeGraph<Tt, Te> : ITreeGraph<TreeGraph<Tt, Te>, Tt, Te>
        where Tt : ITreeNode<Te>
        where Te : IGraphEdge
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal Tt[] Nodes { get; }
        [凾(256)]
        public Tt[] AsArray() => Nodes;
        public Tt this[int index] { [凾(256)] get => Nodes[index]; }
        public int Length => Nodes.Length;
        public int Root { get; }
        public HeavyLightDecomposition<Tt, Te> HlDecomposition { get; }
        public TreeGraph(Tt[] array, int root, HeavyLightDecomposition<Tt, Te> hl)
        {
            Root = root;
            Nodes = array;
            HlDecomposition = hl;
        }

        [凾(256)]
        static TreeGraph<Tt, Te> ITreeGraph<TreeGraph<Tt, Te>, Tt, Te>.Tree(Tt[] nodes, int root, HeavyLightDecomposition<Tt, Te> hl)
            => new(nodes, root, hl);
    }
}
