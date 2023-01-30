using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    using Starry = ImmutableLazyBinarySearchTreeTestsBase.Starry;
    public class ImmutableLazyRedBlackTreeTests : ImmutableLazyBinarySearchTreeTestsBase<ImmutableLazyRedBlackTree<int, int, Starry>>
    {
        protected override ImmutableLazyRedBlackTree<int, int, Starry> Empty
            => ImmutableLazyRedBlackTree<int, int, Starry>.Empty;
        protected override ImmutableLazyRedBlackTree<int, int, Starry> Create(IEnumerable<int> values)
            => new ImmutableLazyRedBlackTree<int, int, Starry>(values);
    }
}
