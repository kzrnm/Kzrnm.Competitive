using Kzrnm.Competitive.Internal.Bbst;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using Node = LazyRandomBinarySearchTreeNode<int, int, SumOp>;

[InheritsTests]
public class LazyRandomBinarySearchTreeTests : LazyBinarySearchTreeTestsBase<Node, Node.Op>
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
