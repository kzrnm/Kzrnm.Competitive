using Kzrnm.Competitive.Internal.Bbst;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public class LazyRedBlackTreeTests : LazyBinarySearchTreeTestsBase
    {
        protected override ILazyBinarySearchTree<int, int> Create()
            => new LazyRedBlackTree<int, int, SumOp>();

        protected override ILazyBinarySearchTree<int, int> Create(IEnumerable<int> values)
            => new LazyRedBlackTree<int, int, SumOp>(values);
    }
}
