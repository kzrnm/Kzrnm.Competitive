using Kzrnm.Competitive.Internal;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public class ImmutableRedBlackTreeTests
        : ImmutableBinarySearchTreeTestsBase<ImmutableRedBlackTreeNode<int, SingleBbstOp<int>>, ImmutableRedBlackTree<int>>
    {
        protected override bool UseProd => false;
        protected override ImmutableRedBlackTree<int> Empty
            => ImmutableRedBlackTree<int>.Empty;
        protected override ImmutableRedBlackTree<int> Create(IEnumerable<int> values)
            => ImmutableRedBlackTree<int>.Create(values.ToArray());
    }
}
