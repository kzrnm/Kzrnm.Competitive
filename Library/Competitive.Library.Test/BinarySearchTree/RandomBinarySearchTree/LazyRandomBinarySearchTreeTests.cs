using Kzrnm.Competitive.Internal.Bbst;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public class LazyRandomBinarySearchTreeTests : LazyBinarySearchTreeTestsBase<LazyRandomBinarySearchTreeNode<int, int, SumOp>>
    {
        public LazyRandomBinarySearchTreeTests()
        {
            Unsafe.AsRef(RandomBinarySearchTreeNodeBase.rnd) = new Xoshiro256(227);
        }
        protected override LazyRandomBinarySearchTree<int, int, SumOp> Create()
            => new LazyRandomBinarySearchTree<int, int, SumOp>();

        protected override LazyRandomBinarySearchTree<int, int, SumOp> Create(IEnumerable<int> values)
            => new LazyRandomBinarySearchTree<int, int, SumOp>(values);
    }
}
