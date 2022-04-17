using Kzrnm.Competitive.IO;
using System.Linq;

namespace Kzrnm.Competitive.Collection
{
    public class SetTest
    {
        static void Main() { using var cw = ConsoleOutput.cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/predecessor_problem
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            string T = cr;
            var set = new Set<int>(T.Select((c, i) => (c, i)).Where(t => t.c == '1').Select(t => t.i));
            for (int i = 0; i < Q; i++)
            {
                int t = cr;
                int k = cr;
                switch (t)
                {
                    case 0:
                        set.Add(k); break;
                    case 1:
                        set.Remove(k); break;
                    case 2:
                        cw.WriteLine(set.FindNode(k) is null ? 0 : 1); break;
                    case 3:
                        cw.WriteLine(set.FindNodeLowerBound(k)?.Value ?? -1); break;
                    default:
                        cw.WriteLine(set.FindNodeReverseLowerBound(k)?.Value ?? -1); break;
                }
            }
            return null;
        }
    }
}
