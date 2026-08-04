using Kzrnm.Competitive.Internal;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using Node = ImmutableLazyRedBlackTreeNode<int, int, SumOp>;

[InheritsTests]
public class ImmutableLazyRedBlackTreeTests
    : ImmutableLazyBinarySearchTreeTestsBase<Node, ImmutableLazyRedBlackTree<int, int, SumOp>.Mk, Node.Op, ImmutableLazyRedBlackTree<int, int, SumOp>>
{
    protected override ImmutableLazyRedBlackTree<int, int, SumOp> Empty
        => ImmutableLazyRedBlackTree<int, int, SumOp>.Empty;
    protected override ImmutableLazyRedBlackTree<int, int, SumOp> Create(IEnumerable<int> values)
        => ImmutableLazyRedBlackTree<int, int, SumOp>.Create(values.ToArray());
}
