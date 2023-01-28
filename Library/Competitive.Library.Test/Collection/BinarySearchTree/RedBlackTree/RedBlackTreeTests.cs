using Kzrnm.Competitive.Internal.Bbst;
using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public class RedBlackTreeTests : BinarySearchTreeTestsBase
    {
        protected override IBinarySearchTree<int> Create()
            => new RedBlackTree<int, Starry>();

        protected override IBinarySearchTree<int> Create(IEnumerable<int> values)
            => new RedBlackTree<int, Starry>(values);
    }
}
