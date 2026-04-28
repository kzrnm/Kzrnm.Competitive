using Kzrnm.Competitive.Internal.Bbst;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

[InheritsTests]
public class SplayTreeTests : BinarySearchTreeTestsBase<SplayTreeNode<int, Starry>>
{
    protected override SplayTree<int, Starry> Create()
        => new SplayTree<int, Starry>();

    protected override SplayTree<int, Starry> Create(IEnumerable<int> values)
        => new SplayTree<int, Starry>(values);
}
