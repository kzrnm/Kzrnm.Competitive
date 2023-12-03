using Kzrnm.Competitive.IO;
using System;

namespace Kzrnm.Competitive.DataStructure
{
    internal class ManacherTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/enumerate_palindromes";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            string S = cr;
            var T = new char[2 * S.Length + 1];
            for (int i = 0; i < S.Length; i++)
            {
                T[2 * i] = '\0';
                T[2 * i + 1] = S[i];
            }
            T[^1] = '\0';
            var rt = StringLibEx.Manacher(T);
            for (int i = 0; i < rt.Length; i++)
            {
                --rt[i];
            }
            return cw.WriteLineJoin(rt.AsSpan()[1..^1]);
        }
    }
}
