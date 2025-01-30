using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Algorithm;

internal class LongestIncreasingSubsequenceTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/longest_increasing_subsequence";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        var idx = LongestIncreasingSubsequence.Lis(cr.Repeat(N).Int()).Indexes;
        cw.WriteLine(idx.Length);
        cw.WriteLineJoin(idx);
        return null;
    }
}
