using AtCoder;
using Kzrnm.Competitive.IO;
using System.Runtime.CompilerServices;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.DataStructure;

internal class SwagTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/queue_operate_all_composite";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int Q = cr;
        var swag = new Swag<Mod998244353AffineTransformation, Op>();
        while (--Q >= 0)
        {
            int t = cr;
            if (t == 0)
            {
                int a = cr;
                int b = cr;
                swag.Push(new Mod998244353AffineTransformation(ModInt.Raw(a), ModInt.Raw(b)));
            }
            else if (t == 1)
                swag.Pop();
            else
                cw.WriteLine(swag.AllProd.Apply(cr.Int()).Value);
        }
        return null;
    }
    readonly struct Op : ISegtreeOperator<Mod998244353AffineTransformation>
    {
        [MethodImpl(256)]
        public Mod998244353AffineTransformation Operate(Mod998244353AffineTransformation x, Mod998244353AffineTransformation y) => y.Apply(x);

        public Mod998244353AffineTransformation Identity => new Mod998244353AffineTransformation(ModInt.One, ModInt.Zero);
    }
}
