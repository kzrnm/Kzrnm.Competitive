using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public class WTreeNode<T, TEdge> : TreeNode<TEdge>, ITreeNode<WTreeNode<T, TEdge>, TEdge>, IWTreeNode<T>
        where T : IAdditionOperators<T, T, T>
        where TEdge : IWGraphEdge<T>, IGraphEdge<TEdge>
    {
        public WTreeNode(int i, int size, TEdge parent, int depth, T depthLength, TEdge[] children)
            : base(i, size, parent, depth, children)
        {
            DepthLength = depthLength;
        }
        public T DepthLength { get; }

        [凾(256)]
        static WTreeNode<T, TEdge> ITreeNode<WTreeNode<T, TEdge>, TEdge>.Node(int i, int size, WTreeNode<T, TEdge> parent, TEdge parentEdge, TEdge[] children)
            => new(i, size, parentEdge.Reversed(parent.Index), parent.Depth + 1, parent.DepthLength + parentEdge.Value, children);

        [凾(256)]
        static WTreeNode<T, TEdge> ITreeNode<WTreeNode<T, TEdge>, TEdge>.RootNode(int i, int size, TEdge[] children)
            => new(i, size, TEdge.None, 0, default, children);
    }
}
