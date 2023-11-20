using Kzrnm.Competitive.Internal;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public class RedBlackTreeTests : BinarySearchTreeTestsBase<RedBlackTreeNode<int, Starry>>
    {
        protected override RedBlackTree<int, Starry> Create()
            => new RedBlackTree<int, Starry>();

        protected override RedBlackTree<int, Starry> Create(IEnumerable<int> values)
            => new RedBlackTree<int, Starry>(values);
    }
}
