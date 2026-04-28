using Kzrnm.Competitive.Internal.Bbst;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

[InheritsTests]
public class LazySplayTreeTests : LazyBinarySearchTreeTestsBase<LazySplayTreeNode<int, int, SumOp>>
{
    protected override LazySplayTree<int, int, SumOp> Create()
        => new LazySplayTree<int, int, SumOp>();

    protected override LazySplayTree<int, int, SumOp> Create(IEnumerable<int> values)
        => new LazySplayTree<int, int, SumOp>(values);
}
