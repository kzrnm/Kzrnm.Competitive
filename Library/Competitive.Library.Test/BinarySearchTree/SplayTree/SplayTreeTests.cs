using Kzrnm.Competitive.Internal.Bbst;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using Node = SplayTreeNode<int, Starry>;

[InheritsTests]
public class SplayTreeTests : BinarySearchTreeTestsBase<Node, Node.Op>
{
    protected override SplayTree<int, Starry> Create()
        => new SplayTree<int, Starry>();

    protected override SplayTree<int, Starry> Create(IEnumerable<int> values)
        => new SplayTree<int, Starry>(values);
}
