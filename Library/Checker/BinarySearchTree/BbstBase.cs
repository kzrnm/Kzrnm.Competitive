using Kzrnm.Competitive.Internal;
using Kzrnm.Competitive.IO;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.BinarySearchTree;

// competitive-verifier: DISPLAY no-index
internal abstract class BbstBase : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/dynamic_sequence_range_affine_range_sum";
    public override double? Tle => 10 * 1.2;
    internal readonly struct Op : IReversibleBinarySearchTreeOperator<ModInt, Mod998244353AffineTransformation>
    {
        public ModInt Identity => ModInt.Zero;

        public Mod998244353AffineTransformation FIdentity => new(ModInt.One, ModInt.Zero);

        [凾(256)]
        public ModInt Operate(ModInt x, ModInt y) => x + y;
        [凾(256)]
        public ModInt Mapping(Mod998244353AffineTransformation f, ModInt x, int size) => f.Apply(x) + f.b * (size - 1);
        [凾(256)]
        public Mod998244353AffineTransformation Composition(Mod998244353AffineTransformation nf, Mod998244353AffineTransformation cf) => nf.Apply(cf);
        [凾(256)] public ModInt Inverse(ModInt v) => v;
    }
}
internal abstract class BbstBase<Node> : BbstBase
    where Node : class, ILazyBbstNode<ModInt, Mod998244353AffineTransformation, Node>
{
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        SolveImpl(cr, cw, CreateTree());
        return null;
    }
    protected abstract LazyBinarySearchTreeBase<ModInt, Mod998244353AffineTransformation, Node> CreateTree();
    static void SolveImpl(ConsoleReader cr, Utf8ConsoleWriter cw, LazyBinarySearchTreeBase<ModInt, Mod998244353AffineTransformation, Node> tree)
    {
        int N = cr;
        int Q = cr;
        var a = cr.Repeat(N).Select(cr => ModInt.Raw(cr.Int()));
        tree.AddRange(a);
        while (--Q >= 0)
        {
            int t = cr;
            if (t <= 1)
            {
                int i = cr;
                if (t == 0)
                    tree.Insert(i, ModInt.Raw(cr.Int()));
                else
                    tree.RemoveAt(i);
            }
            else
            {
                int l = cr;
                int r = cr;
                if (t == 3)
                {
                    var b = ModInt.Raw(cr.Int());
                    var c = ModInt.Raw(cr.Int());
                    tree.Apply(l, r, new(b, c));
                }
                else if (t == 2)
                    tree.Reverse(l, r);
                else
                    cw.WriteLine(tree[l..r].Value);
            }
        }
    }
}
