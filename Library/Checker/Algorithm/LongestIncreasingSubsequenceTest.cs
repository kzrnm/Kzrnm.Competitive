using Kzrnm.Competitive.IO;
using System.Linq;

namespace Kzrnm.Competitive.Algorithm;

internal class LongestIncreasingSubsequenceTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/longest_increasing_subsequence";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        var lis = LongestIncreasingSubsequence.Lis(cr.Repeat(N).Int());
        cw.WriteLine(lis.Length);
        cw.WriteLineJoin(lis.Select(t => t.Index));
        return null;
    }
}
