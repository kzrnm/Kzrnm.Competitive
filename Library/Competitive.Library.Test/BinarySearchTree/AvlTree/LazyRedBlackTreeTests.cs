using Kzrnm.Competitive.Internal.Bbst;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using Node = LazyAvlTreeNode<int, int, SumOp>;

[InheritsTests]
public class LazyAvlTreeTests : LazyBinarySearchTreeTestsBase<Node, Node.Op>
{
    protected override LazyAvlTree<int, int, SumOp> Create()
        => new LazyAvlTree<int, int, SumOp>();

    protected override LazyAvlTree<int, int, SumOp> Create(IEnumerable<int> values)
        => new LazyAvlTree<int, int, SumOp>(values);
}
