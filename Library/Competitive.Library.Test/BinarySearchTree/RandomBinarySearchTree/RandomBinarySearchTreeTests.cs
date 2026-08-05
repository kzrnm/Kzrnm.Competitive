using Kzrnm.Competitive.Internal.Bbst;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using Node = RandomBinarySearchTreeNode<int, Starry>;

[InheritsTests]
public class RandomBinarySearchTreeTests : BinarySearchTreeTestsBase<Node, Node.Op>
{
    public RandomBinarySearchTreeTests()
    {
        Unsafe.AsRef(RandomBinarySearchTreeNodeBase.rnd) = new Xoshiro256(227);
    }
    protected override RandomBinarySearchTree<int, Starry> Create()
        => new RandomBinarySearchTree<int, Starry>();

    protected override RandomBinarySearchTree<int, Starry> Create(IEnumerable<int> values)
        => new RandomBinarySearchTree<int, Starry>(values);
}
