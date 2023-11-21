using Kzrnm.Competitive.Internal;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.BinarySearchTree
{
    internal class LazySplayTreeTest : BbstBase<LazySplayTreeNode<ModInt, Mod998244353AffineTransformation, BbstBase.Op>>
    {
        protected override LazySplayTree<ModInt, Mod998244353AffineTransformation, Op> CreateTree()
            => new LazySplayTree<ModInt, Mod998244353AffineTransformation, Op>();
    }
}