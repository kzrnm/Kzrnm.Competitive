using Kzrnm.Competitive.Internal.Bbst;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using Node = LazySplayTreeNode<int, int, SumOp>;

[InheritsTests]
public class LazySplayTreeTests : LazyBinarySearchTreeTestsBase<Node, Node.Op>
{
    protected override LazySplayTree<int, int, SumOp> Create()
        => new LazySplayTree<int, int, SumOp>();

    protected override LazySplayTree<int, int, SumOp> Create(IEnumerable<int> values)
        => new LazySplayTree<int, int, SumOp>(values);
}
