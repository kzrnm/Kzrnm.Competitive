using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Kzrnm.Competitive.Collection;

internal class AssociativeArrayTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/associative_array";
    public override double? Tle => 5;
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int Q = cr;
        var dic = new Dictionary<long, long>(Q, new EqualityComparer());
        for (int i = 0; i < Q; i++)
        {
            int t = cr;
            long k = cr;
            if (t == 0)
            {
                long v = cr;
                dic[k] = v;
            }
            else
                cw.WriteLine(dic.GetValueOrDefault(k));
        }
        return null;
    }
    private class EqualityComparer : EqualityComparer<long>
    {
        public override bool Equals(long x, long y) => x == y;
        public override int GetHashCode([DisallowNull] long obj) => HashCode.Combine((int)obj, (int)(obj >> 11), (int)(obj >> 22), (int)(obj >> 33));
    }
}
