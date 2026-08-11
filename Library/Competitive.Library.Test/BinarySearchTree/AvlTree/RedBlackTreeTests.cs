using Kzrnm.Competitive.Internal.Bbst;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using Node = AvlTreeNode<int, Starry>;

[InheritsTests]
public class AvlTreeTests : BinarySearchTreeTestsBase<Node, Node.Op>
{
    protected override AvlTree<int, Starry> Create()
        => new AvlTree<int, Starry>();

    protected override AvlTree<int, Starry> Create(IEnumerable<int> values)
        => new AvlTree<int, Starry>(values);
}
