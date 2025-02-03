using Kzrnm.Competitive.Internal;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.BinarySearchTree;

internal class LazyRedBlackTreeTest : BbstBase<LazyRedBlackTreeNode<ModInt, Mod998244353AffineTransformation, BbstBase.BbstOp>>
{
    protected override LazyRedBlackTree<ModInt, Mod998244353AffineTransformation, BbstOp> CreateTree()
        => new LazyRedBlackTree<ModInt, Mod998244353AffineTransformation, BbstOp>();
}