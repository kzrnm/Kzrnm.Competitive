using Kzrnm.Competitive.Internal.Bbst;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public class LazySplayTreeTests : LazyBinarySearchTreeTestsBase
    {
        protected override ILazyBinarySearchTree<int, int> Create()
            => new LazySplayTree<int, int, SumOp>();

        protected override ILazyBinarySearchTree<int, int> Create(IEnumerable<int> values)
            => new LazySplayTree<int, int, SumOp>(values);
    }
}
