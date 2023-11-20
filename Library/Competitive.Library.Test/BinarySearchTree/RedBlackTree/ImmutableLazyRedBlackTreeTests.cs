using Kzrnm.Competitive.Internal;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public class ImmutableLazyRedBlackTreeTests
        : ImmutableLazyBinarySearchTreeTestsBase<ImmutableLazyRedBlackTreeNode<int, int, SumOp>, ImmutableLazyRedBlackTree<int, int, SumOp>>
    {
        protected override ImmutableLazyRedBlackTree<int, int, SumOp> Empty
            => ImmutableLazyRedBlackTree<int, int, SumOp>.Empty;
        protected override ImmutableLazyRedBlackTree<int, int, SumOp> Create(IEnumerable<int> values)
            => new ImmutableLazyRedBlackTree<int, int, SumOp>(values);
    }
}
