using Kzrnm.Competitive.Internal.Bbst;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

public class RandomBinarySearchTreeTests : BinarySearchTreeTestsBase<RandomBinarySearchTreeNode<int, Starry>>
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
