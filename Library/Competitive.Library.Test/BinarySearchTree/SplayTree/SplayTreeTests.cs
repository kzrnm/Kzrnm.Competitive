using Kzrnm.Competitive.Internal.Bbst;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public class SplayTreeTests : BinarySearchTreeTestsBase<SplayTreeNode<int, Starry>>
    {
        protected override SplayTree<int, Starry> Create()
            => new SplayTree<int, Starry>();

        protected override SplayTree<int, Starry> Create(IEnumerable<int> values)
            => new SplayTree<int, Starry>(values);
    }
}
