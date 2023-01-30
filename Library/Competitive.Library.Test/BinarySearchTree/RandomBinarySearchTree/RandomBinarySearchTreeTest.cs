using Kzrnm.Competitive.Internal.Bbst;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public class RandomBinarySearchTreeTest : BinarySearchTreeTestsBase
    {
        public RandomBinarySearchTreeTest()
        {
            Unsafe.AsRef(RandomBinarySearchTreeNodeOperator<int, Starry>.rnd) = new Xoshiro256(227);
        }
        protected override IBinarySearchTree<int> Create()
            => new RandomBinarySearchTree<int, Starry>();

        protected override IBinarySearchTree<int> Create(IEnumerable<int> values)
            => new RandomBinarySearchTree<int, Starry>(values);
    }
}
