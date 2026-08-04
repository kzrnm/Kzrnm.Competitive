using Kzrnm.Competitive.Internal;

namespace Kzrnm.Competitive.Testing.Collection.BinarySearchTree;

using Node = RedBlackTreeNode<int, Starry>;

[InheritsTests]
public class RedBlackTreeTests : BinarySearchTreeTestsBase<Node, Node.Op>
{
    protected override RedBlackTree<int, Starry> Create()
        => new RedBlackTree<int, Starry>();

    protected override RedBlackTree<int, Starry> Create(IEnumerable<int> values)
        => new RedBlackTree<int, Starry>(values);
}
