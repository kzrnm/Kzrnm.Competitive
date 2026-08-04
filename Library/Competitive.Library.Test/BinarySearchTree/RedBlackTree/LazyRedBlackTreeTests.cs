using Kzrnm.Competitive.Internal;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using Node = LazyRedBlackTreeNode<int, int, SumOp>;

[InheritsTests]
public class LazyRedBlackTreeTests : LazyBinarySearchTreeTestsBase<Node, Node.Op>
{
    protected override LazyRedBlackTree<int, int, SumOp> Create()
        => new LazyRedBlackTree<int, int, SumOp>();

    protected override LazyRedBlackTree<int, int, SumOp> Create(IEnumerable<int> values)
        => new LazyRedBlackTree<int, int, SumOp>(values);
}
