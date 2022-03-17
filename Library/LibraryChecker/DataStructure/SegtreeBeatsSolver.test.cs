using Kzrnm.Competitive.IO;
using System.Linq;
using static System.Math;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class SegtreeBeatsSolver
    {
        static void Main() => new SegtreeBeatsSolver().Solve(new ConsoleReader(), new ConsoleWriter()).Flush();
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/range_chmin_chmax_add_range_sum
        public double TimeoutSecond => 10;
        public ConsoleWriter Solve(ConsoleReader cr, ConsoleWriter cw)
        {
            int N = cr;
            int Q = cr;
            long[] a = cr.Repeat(N);
            var seg = new SegtreeBeats<S, F, Op>(a.Select(n => new S(n)).ToArray());
            for (int q = 0; q < Q; q++)
            {
                int t = cr;
                int l = cr;
                int r = cr;
                if (t == 3)
                    cw.WriteLine(seg[l..r].sum);
                else
                {
                    long b = cr;
                    if (t == 0)
                        seg.Apply(l, r, F.Min(b));
                    else if (t == 1)
                        seg.Apply(l, r, F.Max(b));
                    else
                        seg.Apply(l, r, F.Add(b));
                }
            }
            return cw;
        }
    }
    struct S
    {
        public long min;
        public long max;
        public long max2;
        public long min2;
        public long sum;
        public int cnt;
        public int minCnt;
        public int maxCnt;
        public S(long num, int cnt = 1)
        {
            min = num;
            max = num;
            min2 = long.MaxValue >> 2;
            max2 = long.MinValue >> 2;
            sum = num * cnt;
            this.cnt = cnt;
            minCnt = cnt;
            maxCnt = cnt;
        }
    }
    struct F
    {
        public long min;
        public long max;
        public long sum;

        public static F Min(long num) => new F { min = num, max = long.MinValue >> 2 };
        public static F Max(long num) => new F { min = long.MaxValue >> 2, max = num };
        public static F Add(long num) => new F { min = long.MaxValue >> 2, max = long.MinValue >> 2, sum = num };
    }

    struct Op : ISegtreeBeatsOperator<S, F>
    {
        public S Identity => new S
        {
            min = long.MaxValue >> 2,
            max = long.MinValue >> 2,
            min2 = long.MaxValue >> 2,
            max2 = long.MinValue >> 2,
        };

        public F FIdentity => new F
        {
            min = long.MaxValue >> 2,
            max = long.MinValue >> 2,
        };

        public S Operate(S x, S y) => new S
        {
            min = Min(x.min, y.min),
            max = Max(x.max, y.max),
            min2 = Min2(x.min, x.min2, y.min, y.min2),
            max2 = Max2(x.max, x.max2, y.max, y.max2),
            sum = x.sum + y.sum,
            cnt = x.cnt + y.cnt,
            minCnt = x.min == y.min ? x.minCnt + y.minCnt : x.min < y.min ? x.minCnt : y.minCnt,
            maxCnt = x.max == y.max ? x.maxCnt + y.maxCnt : x.max > y.max ? x.maxCnt : y.maxCnt,
        };

        long Min2(long v1, long v2, long v3, long v4)
        {
            if (v1 == v3) return Min(v2, v4);
            if (v1 < v3) return Min(v2, v3);
            else return Min(v1, v4);
        }

        long Max2(long v1, long v2, long v3, long v4)
        {
            if (v1 == v3) return Max(v2, v4);
            if (v1 > v3) return Max(v2, v3);
            else return Max(v1, v4);
        }
        public bool Mapping(F f, S x, out S res)
        {
            if (x.cnt == 0)
            {
                res = Identity;
                return true;
            }
            if (x.min == x.max || f.max == f.min || f.max >= x.max || f.min < x.min)
            {
                res = new S(Min(f.min, Max(x.min, f.max)) + f.sum, x.cnt);
                return true;
            }
            if (x.min2 == x.max)
            {
                x.min = x.max2 = Max(x.min, f.max) + f.sum;
                x.max = x.min2 = Min(x.max, f.min) + f.sum;
                x.sum = x.min * x.minCnt + x.max * x.maxCnt;
                res = x;
                return true;
            }
            if (f.max < x.min2 && f.min > x.max2)
            {
                var nxt_lo = Max(x.min, f.max);
                var nxt_hi = Min(x.max, f.min);
                x.sum += (nxt_lo - x.min) * x.minCnt - (x.max - nxt_hi) * x.maxCnt + f.sum * x.cnt;
                x.min = nxt_lo + f.sum;
                x.max = nxt_hi + f.sum;
                x.min2 += f.sum;
                x.max2 += f.sum;
                res = x;
                return true;
            }
            res = x;
            return false;
        }
        public F Composition(F nf, F cf) => new F
        {
            max = Max(nf.max, Min(cf.max + cf.sum, nf.min)) - cf.sum,
            min = Min(nf.min, Max(cf.min + cf.sum, nf.max)) - cf.sum,
            sum = cf.sum + nf.sum,
        };
    }
}