using Kzrnm.Competitive.Internal.Bbst;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree
{
    public class LazyRandomBinarySearchTreeTests : LazyBinarySearchTreeTestsBase
    {
        public LazyRandomBinarySearchTreeTests()
        {
            Unsafe.AsRef(LazyRandomBinarySearchTreeNodeOperator<int, int, SumOp>.rnd) = new Xoshiro256(227);
        }
        protected override ILazyBinarySearchTree<int, int> Create()
            => new LazyRandomBinarySearchTree<int, int, SumOp>();

        protected override ILazyBinarySearchTree<int, int> Create(IEnumerable<int> values)
            => new LazyRandomBinarySearchTree<int, int, SumOp>(values);
    }
}
