using Kzrnm.Competitive.Internal;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using Node = ImmutableRedBlackTreeNode<int, SingleBbstOp<int>>;

[InheritsTests]
public class ImmutableRedBlackTreeTests
    : ImmutableBinarySearchTreeTestsBase<Node, ImmutableRedBlackTree<int>.Mk, Node.Op, ImmutableRedBlackTree<int>>
{
    protected override bool UseProd => false;
    protected override ImmutableRedBlackTree<int> Empty
        => ImmutableRedBlackTree<int>.Empty;
    protected override ImmutableRedBlackTree<int> Create(IEnumerable<int> values)
        => ImmutableRedBlackTree<int>.Create(values.ToArray());
}
