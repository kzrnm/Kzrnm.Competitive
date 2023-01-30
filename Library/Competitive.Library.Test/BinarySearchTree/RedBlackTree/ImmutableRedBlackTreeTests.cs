using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public class ImmutableRedBlackTreeTests : ImmutableBinarySearchTreeTestsBase<ImmutableRedBlackTree<int>>
    {
        protected override bool UseProd => false;
        protected override ImmutableRedBlackTree<int> Empty
            => ImmutableRedBlackTree<int>.Empty;
        protected override ImmutableRedBlackTree<int> Create(IEnumerable<int> values)
            => new ImmutableRedBlackTree<int>(values);
    }
}
