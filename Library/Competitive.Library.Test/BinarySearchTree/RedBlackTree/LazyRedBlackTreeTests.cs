using Kzrnm.Competitive.Internal;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public class LazyRedBlackTreeTests : LazyBinarySearchTreeTestsBase<LazyRedBlackTreeNode<int, int, SumOp>>
    {
        protected override LazyRedBlackTree<int, int, SumOp> Create()
            => new LazyRedBlackTree<int, int, SumOp>();

        protected override LazyRedBlackTree<int, int, SumOp> Create(IEnumerable<int> values)
            => new LazyRedBlackTree<int, int, SumOp>(values);
    }
}
