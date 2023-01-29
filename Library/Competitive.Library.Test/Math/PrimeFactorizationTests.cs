using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kzrnm.Competitive.Testing.MathNS
{
    public class PrimeFactorizationTests
    {
        private struct IsPrimeModId { }
        [Fact(Timeout = 10000)]
        public async Task IsPrime()
        {
            await Task.Yield();
            var p = new PrimeNumber(1 << 20);
            for (int i = 0; i < 2000; i++)
            {
                foreach (var start in new[] {
                    0,
                    (1L << 30) - 500,
                    1795265022 - 500,
                    4759123141 - 500,
                })
                {
                    var x = start + i;
                    PrimeFactorization<IsPrimeModId>.IsPrime(x).Should().Be(p.IsPrime(x));
                }
            }
            PrimeFactorization<IsPrimeModId>.IsPrime(200560490131).Should().Be(true);
            PrimeFactorization<IsPrimeModId>.IsPrime(92709568269121).Should().Be(true);
            PrimeFactorization<IsPrimeModId>.IsPrime(9007199254740997).Should().Be(true);
            PrimeFactorization<IsPrimeModId>.IsPrime(1162193L * 1347377).Should().Be(false);
            PrimeFactorization<IsPrimeModId>.IsPrime(89652331L * 96325939).Should().Be(false);
        }

        private struct DivisorIntModId0 { }
        private struct DivisorIntModId1 { }
        private struct DivisorIntModId2 { }
        private struct DivisorIntModId3 { }
        private struct DivisorIntModId4 { }
        private struct DivisorIntModId5 { }
        public static TheoryData DivisorInt_Data => new TheoryData<int, int[], Func<int, int[]>>
        {
            {
                1,
                new [] { 1 },
                new (PrimeFactorization<DivisorIntModId0>.Divisor)
            },
            {
                1 << 16,
                Enumerable.Range(0, 17).Select(i => 1 << i).ToArray(),
                new (PrimeFactorization<DivisorIntModId1>.Divisor)
            },
            {
                49,
                new [] { 1, 7, 49, },
                new (PrimeFactorization<DivisorIntModId2>.Divisor)
            },
            {
                2 * 3 * 5,
                new [] { 1, 2, 3, 5, 6, 10, 15, 30, },
                new (PrimeFactorization<DivisorIntModId3>.Divisor)
            },
            {
                720,
                new [] {
                    1, 2, 3, 4, 5, 6, 8, 9, 10,
                    12, 15, 16, 18, 20, 24, 30,
                    36, 40, 45, 48, 60, 72, 80,
                    90, 120, 144, 180, 240, 360, 720
                },
                new (PrimeFactorization<DivisorIntModId4>.Divisor)
            },
            {
                2147483647,
                new [] { 1, 2147483647, },
                new (PrimeFactorization<DivisorIntModId5>.Divisor)
            },
        };

        [Theory(Timeout = 10000)]
        [MemberData(nameof(DivisorInt_Data))]
        public async Task DivisorInt(int num, int[] expected, Func<int, int[]> Divisor)
        {
            await Task.Yield();
            Divisor(num).Should().Equal(expected);
        }

        private struct DivisorIntLargeModId { }
        [Fact(Timeout = 10000)]
        public async Task DivisorIntLarge()
        {
            await Task.Yield();
            PrimeFactorization<DivisorIntLargeModId>.Divisor(6480).Should()
                .StartWith(new int[] { 1, 2, 3, 4, 5, 6, 8, 9, 10, 12, 15, 16, 18, 20, 24, 27, 30, 36, 40, 45, 48, 54, 60, 72, 80, 81 })
                .And
                .EndWith(new int[] { 1620, 2160, 3240, 6480 })
                .And
                .HaveCount(50);

            PrimeFactorization<DivisorIntLargeModId>.Divisor(2095133040).Should().HaveCount(1600); //高度合成数
        }

        private struct DivisorLongModId { }
        [Fact(Timeout = 10000)]
        public async Task DivisorLong()
        {
            await Task.Yield();
            PrimeFactorization<DivisorLongModId>.Divisor(1L).Should().Equal(new long[] { 1 });
            PrimeFactorization<DivisorLongModId>.Divisor(128100283921).Should().Equal(new long[] {
                1,
                71,
                5041,
                357911,
                25411681,
                1804229351,
                128100283921 });
            PrimeFactorization<DivisorLongModId>.Divisor(132147483703).Should().Equal(new long[] { 1, 132147483703 });
            PrimeFactorization<DivisorLongModId>.Divisor(963761198400).Should().HaveCount(6720); //高度合成数
            PrimeFactorization<DivisorLongModId>.Divisor(897612484786617600).Should().HaveCount(103680); //高度合成数

            PrimeFactorization<DivisorLongModId>.Divisor(9007199254740997).Should().Equal(new long[] { 1, 9007199254740997 });
            PrimeFactorization<DivisorLongModId>.Divisor(89652331L * 96325939).Should().Equal(new long[] {
                1,
                89652331,
                96325939,
                89652331L * 96325939,
            });
        }

        private struct IntFactoringModId0 { }
        private struct IntFactoringModId1 { }
        private struct IntFactoringModId2 { }
        private struct IntFactoringModId3 { }
        private struct IntFactoringModId4 { }
        private struct IntFactoringModId5 { }
        private struct IntFactoringModId6 { }
        public static TheoryData PrimeFactoringInt_Data => new TheoryData<int, Dictionary<int, int>, Func<int, Dictionary<int, int>>>
        {
            {
                1,
                new Dictionary<int, int> { },
                new (PrimeFactorization<IntFactoringModId0>.PrimeFactoring)
            },
            {
                1 << 16,
                new Dictionary<int, int> {
                    { 2, 16 },
                },
                new (PrimeFactorization<IntFactoringModId1>.PrimeFactoring)
            },
            {
                2 * 3 * 5,
                new Dictionary<int, int> {
                    { 2, 1 },
                    { 3, 1 },
                    { 5, 1 },
                },
                new (PrimeFactorization<IntFactoringModId2>.PrimeFactoring)
            },
            {
                99991,
                new Dictionary<int, int> {
                    { 99991, 1 },
                },
                new (PrimeFactorization<IntFactoringModId3>.PrimeFactoring)
            },
            {
                2147483647,
                new Dictionary<int, int> {
                    { 2147483647, 1 },
                },
                new (PrimeFactorization<IntFactoringModId4>.PrimeFactoring)
            },
            {
                2095133040, //高度合成数
                new Dictionary<int, int> {
                    { 2, 4 },
                    { 3, 4 },
                    { 5, 1 },
                    { 7, 1 },
                    { 11, 1 },
                    { 13, 1 },
                    { 17, 1 },
                    { 19, 1 },
                },
                new (PrimeFactorization<IntFactoringModId5>.PrimeFactoring)
            },
        };

        [Theory(Timeout = 10000)]
        [MemberData(nameof(PrimeFactoringInt_Data))]
        public async Task PrimeFactoringInt(int num, Dictionary<int, int> expected, Func<int, Dictionary<int, int>> PrimeFactoring)
        {
            await Task.Yield();
            PrimeFactoring(num).Should().Equal(expected);
        }

        private struct LongFactoringModId0 { }
        private struct LongFactoringModId1 { }
        private struct LongFactoringModId2 { }
        private struct LongFactoringModId3 { }
        private struct LongFactoringModId4 { }
        private struct LongFactoringModId5 { }
        private struct LongFactoringModId6 { }
        public static TheoryData PrimeFactoringLong_Data => new TheoryData<long, Dictionary<long, int>, Func<long, Dictionary<long, int>>>
        {
            {
                1,
                new Dictionary<long, int> { },
                new (PrimeFactorization<LongFactoringModId0>.PrimeFactoring)
            },
            {
                903906555552,
                new Dictionary<long, int> {
                    { 2, 5 },
                    { 3, 8 },
                    { 7, 1 },
                    { 11, 2 },
                    { 13, 1 },
                    { 17, 1 },
                    { 23, 1 },
                },
                new (PrimeFactorization<LongFactoringModId1>.PrimeFactoring)
            },
            {
                132147483703,
                new Dictionary<long, int> {
                    { 132147483703, 1 },
                },
                new (PrimeFactorization<LongFactoringModId2>.PrimeFactoring)
            },
            {
                963761198400, //高度合成数
                new Dictionary<long, int> {
                    { 2, 6 },
                    { 3, 4 },
                    { 5, 2 },
                    { 7, 1 },
                    { 11, 1 },
                    { 13, 1 },
                    { 17, 1 },
                    { 19, 1 },
                    { 23, 1 },
                },
                new (PrimeFactorization<LongFactoringModId3>.PrimeFactoring)
            },
            {
                897612484786617600, //高度合成数
                new Dictionary<long, int> {
                    { 2, 8 },
                    { 3, 4 },
                    { 5, 2 },
                    { 7, 2 },
                    { 11, 1 },
                    { 13, 1 },
                    { 17, 1 },
                    { 19, 1 },
                    { 23, 1 },
                    { 29, 1 },
                    { 31, 1 },
                    { 37, 1 },
                },
                new (PrimeFactorization<LongFactoringModId4>.PrimeFactoring)
            },
            {
                89652331L * 96325939,
                new Dictionary<long, int> {
                    { 89652331, 1 },
                    { 96325939, 1 },
                },
                new (PrimeFactorization<LongFactoringModId5>.PrimeFactoring)
            },
            {
                9007199254740997,
                new Dictionary<long, int> {
                    { 9007199254740997, 1 },
                },
                new (PrimeFactorization<LongFactoringModId6>.PrimeFactoring)
            },
        };

        [Theory]
        [MemberData(nameof(PrimeFactoringLong_Data))]
        public async Task PrimeFactoringLong(long num, Dictionary<long, int> expected, Func<long, Dictionary<long, int>> PrimeFactoring)
        {
            await Task.Yield();
            PrimeFactoring(num).Should().Equal(expected);
        }

        private struct StressModId0 { }
        private struct StressModId1 { }
        private struct StressModId2 { }
        private struct StressModId3 { }
        private struct StressModId4 { }
        private struct StressModId5 { }
        private struct StressModId6 { }
        private struct StressModId7 { }
        private struct StressModId8 { }
        private struct StressModId9 { }
        public static IEnumerable<object[]> StressDivisor_Data()
        {
            var rnd = new Random(227);
            long n;
            n = rnd.NextLong() % (1L << 45);
            yield return new object[] { n, Naive(n), new Func<long, long[]>(PrimeFactorization<StressModId0>.Divisor) };
            n = rnd.NextLong() % (1L << 45);
            yield return new object[] { n, Naive(n), new Func<long, long[]>(PrimeFactorization<StressModId1>.Divisor) };
            n = rnd.NextLong() % (1L << 45);
            yield return new object[] { n, Naive(n), new Func<long, long[]>(PrimeFactorization<StressModId2>.Divisor) };
            n = rnd.NextLong() % (1L << 45);
            yield return new object[] { n, Naive(n), new Func<long, long[]>(PrimeFactorization<StressModId3>.Divisor) };
            n = rnd.NextLong() % (1L << 45);
            yield return new object[] { n, Naive(n), new Func<long, long[]>(PrimeFactorization<StressModId4>.Divisor) };
            n = rnd.NextLong() % (1L << 45);
            yield return new object[] { n, Naive(n), new Func<long, long[]>(PrimeFactorization<StressModId5>.Divisor) };
            n = rnd.NextLong() % (1L << 45);
            yield return new object[] { n, Naive(n), new Func<long, long[]>(PrimeFactorization<StressModId6>.Divisor) };
            n = rnd.NextLong() % (1L << 45);
            yield return new object[] { n, Naive(n), new Func<long, long[]>(PrimeFactorization<StressModId7>.Divisor) };
            n = rnd.NextLong() % (1L << 45);
            yield return new object[] { n, Naive(n), new Func<long, long[]>(PrimeFactorization<StressModId8>.Divisor) };
            n = rnd.NextLong() % (1L << 45);
            yield return new object[] { n, Naive(n), new Func<long, long[]>(PrimeFactorization<StressModId9>.Divisor) };

            static long[] Naive(long n)
            {
                if (n <= 1) return new long[] { 1 };

                var left = new List<long>();
                var right = new List<long>();
                left.Add(1);
                right.Add(n);

                for (long i = 2, d = Math.DivRem(n, i, out var am);
                    i <= d;
                    i++, d = Math.DivRem(n, i, out am))
                {
                    if (am == 0)
                    {
                        left.Add(i);
                        if (i != d)
                            right.Add(d);
                    }
                }
                right.Reverse();
                var res = new long[left.Count + right.Count];
                left.CopyTo(res, 0);
                right.CopyTo(res, left.Count);
                return res;
            }
        }
        [Theory(Timeout = 10000)]
        [MemberData(nameof(StressDivisor_Data))]
        public async Task StressDivisor(long n, long[] expected, Func<long, long[]> Divisor)
        {
            await Task.Yield();
            Divisor(n).Should().Equal(expected);
        }
    }
}
