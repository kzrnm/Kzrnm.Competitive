using Kzrnm.Competitive.IO;
using System.Collections.Generic;

namespace Kzrnm.Competitive.DataStructure
{
    internal class StringRunTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/runenumerate";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            Asciis S = cr;
            var ls = new List<(int, int, int)>();
            var runs = StringLibEx.RunEnumerate(S);
            for (int i = 0; i < runs.Length; i++)
            {
                runs[i].Sort();
                foreach (var (l, r) in runs[i])
                {
                    ls.Add((i, l, r));
                }
            }
            cw.WriteLine(ls.Count);
            return cw.WriteGrid(ls);
        }
    }
}
