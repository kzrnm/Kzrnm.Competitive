using AtCoder;
using Kzrnm.Competitive.IO;
using System.Runtime.CompilerServices;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace Kzrnm.Competitive.DataStructure
{
    public class SwagTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/queue_operate_all_composite
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int Q = cr;
            var swag = new Swag<F, Op>();
            for (int q = 0; q < Q; q++)
            {
                int t = cr;
                if (t == 0)
                {
                    int a = cr;
                    int b = cr;
                    swag.Push(new F(ModInt.Raw(a), ModInt.Raw(b)));
                }
                else if (t == 1)
                    swag.Pop();
                else
                    cw.WriteLine(swag.AllProd.Apply(cr).Value);
            }
            return null;
        }
        readonly struct F
        {
            public static readonly F Identity = new F(ModInt.Raw(1), default);
            public readonly ModInt a;
            public readonly ModInt b;
            public F(ModInt a, ModInt b)
            {
                this.a = a;
                this.b = b;
            }
            public ModInt Apply(int x) => a * ModInt.Raw(x) + b;
        }
        struct Op : ISegtreeOperator<F>
        {
            [MethodImpl(256)]
            public F Operate(F x, F y)
            {
                var a = x.a;
                var b = x.b;
                var c = y.a;
                var d = y.b;
                return new F(c * a, c * b + d);
            }
            public F Identity => F.Identity;
        }
    }
}
