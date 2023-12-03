using Kzrnm.Competitive.IO;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.DataStructure
{
    internal class RollingHashTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/zalgorithm";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            string S = cr;
            var rh = RollingHash.Create(S);
            var rt = new int[S.Length];
            rt[0] = S.Length;

            for (int i = 1; i < rt.Length; i++)
            {
                rt[i] = new F(rh, i).BinarySearch(0, rt.Length - i + 1);
            }
            cw.WriteLineJoin(rt);
            return null;
        }
        readonly record struct F(RollingHash rh, int start) : IOk<int>
        {
            [MethodImpl(256)]
            public bool Ok(int len) => rh[..len] == rh.Slice(start, len);
        }
    }
}
