using Kzrnm.Competitive.Internal.Bbst;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public class SplayTreeTests : BinarySearchTreeTestsBase
    {
        protected override IBinarySearchTree<int> Create()
            => new SplayTree<int, Starry>();

        protected override IBinarySearchTree<int> Create(IEnumerable<int> values)
            => new SplayTree<int, Starry>(values);
    }
}
