using static System.Math;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class SegtreeBeatsTests
    {
        [Fact]
        public void Native()
        {
            LongSums sums;
            var seg = new SegtreeBeats<S, long, Op>(new S[10].Fill(new S(0)));
            seg.Apply(0, 8, 1);
            sums = new LongSums(new long[] { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 });
            for (int l = 0; l < 10; l++)
                for (int r = l; r <= 10; r++)
                    seg[l..r].sum.Should().Be(sums[l..r]);

            seg.Apply(0, 3, 5);
            sums = new LongSums(new long[] { 5, 5, 5, 1, 1, 1, 1, 1, 0, 0 });
            for (int l = 0; l < 10; l++)
                for (int r = l; r <= 10; r++)
                    seg[l..r].sum.Should().Be(sums[l..r]);

            seg.Apply(2, 5, 2);
            sums = new LongSums(new long[] { 5, 5, 5, 2, 2, 1, 1, 1, 0, 0 });
            for (int l = 0; l < 10; l++)
                for (int r = l; r <= 10; r++)
                    seg[l..r].sum.Should().Be(sums[l..r]);
        }
        struct S
        {
            public long max;
            public long max2;
            public long sum;
            public int cnt;
            public int maxCnt;
            public S(long num, int cnt = 1)
            {
                max = num;
                max2 = long.MinValue >> 2;
                sum = num * cnt;
                this.cnt = cnt;
                maxCnt = cnt;
            }
            public override readonly string ToString() => $"max: {max}, max2: {max2}, sum: {sum}, cnt: {cnt}, maxCnt: {maxCnt}";
        }

        struct Op : ISegtreeBeatsOperator<S, long>
        {
            public S Identity => new()
            {
                max = long.MinValue >> 2,
                max2 = long.MinValue >> 2,
            };

            public readonly long FIdentity => long.MinValue >> 2;

            public S Operate(S x, S y) => new()
            {
                max = Max(x.max, y.max),
                max2 = Max2(x.max, x.max2, y.max, y.max2),
                sum = x.sum + y.sum,
                cnt = x.cnt + y.cnt,
                maxCnt = x.max == y.max ? x.maxCnt + y.maxCnt : x.max > y.max ? x.maxCnt : y.maxCnt,
            };

            readonly long Max2(long v1, long v2, long v3, long v4)
            {
                if (v1 == v3) return Max(v2, v4);
                if (v1 > v3) return Max(v2, v3);
                else return Max(v1, v4);
            }
            public bool Mapping(long f, S x, out S res)
            {
                if (x.cnt == 0)
                {
                    res = Identity;
                    return true;
                }
                if (f >= x.max)
                {
                    res = new S(f, x.cnt);
                    return true;
                }
                if (x.cnt == x.maxCnt)
                {
                    res = x;
                    return true;
                }
                if (f > x.max2)
                {
                    res = x;
                    res.max2 = f;
                    res.sum = x.max * x.maxCnt + (x.cnt - x.maxCnt) * f;
                    return true;
                }
                res = x;
                return false;
            }
            public readonly long Composition(long nf, long cf) => Max(nf, cf);
        }
    }
}
