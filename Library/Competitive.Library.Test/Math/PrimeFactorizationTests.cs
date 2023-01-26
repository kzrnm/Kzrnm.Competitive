using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.MathNS
{
    public class PrimeFactorizationTests
    {
        [Fact]
        public void IsPrime()
        {
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
                    PrimeFactorization.IsPrime(x).Should().Be(p.IsPrime(x));
                }
            }
            PrimeFactorization.IsPrime(200560490131).Should().Be(true);
            PrimeFactorization.IsPrime(92709568269121).Should().Be(true);
            PrimeFactorization.IsPrime(9007199254740997).Should().Be(true);
            PrimeFactorization.IsPrime(1162193L * 1347377).Should().Be(false);
            PrimeFactorization.IsPrime(89652331L * 96325939).Should().Be(false);
        }

        public static TheoryData DivisorInt_Data => new TheoryData<int, int[]>
        {
            {
                1,
                new [] { 1 }
            },
            {
                1 << 16,
                Enumerable.Range(0, 17).Select(i => 1 << i).ToArray()
            },
            {
                49,
                new [] { 1, 7, 49, }
            },
            {
                2 * 3 * 5,
                new [] { 1, 2, 3, 5, 6, 10, 15, 30, }
            },
            {
                720,
                new [] {
                    1, 2, 3, 4, 5, 6, 8, 9, 10,
                    12, 15, 16, 18, 20, 24, 30,
                    36, 40, 45, 48, 60, 72, 80,
                    90, 120, 144, 180, 240, 360, 720
                }
            },
            {
                2147483647,
                new [] { 1, 2147483647, }
            },
        };

        [Theory]
        [MemberData(nameof(DivisorInt_Data))]
        public void DivisorInt(int num, int[] expected)
        {
            PrimeFactorization.Divisor(num).Should().Equal(expected);
        }

        [Fact]
        public void DivisorIntLarge()
        {
            PrimeFactorization.Divisor(6480).Should()
                .StartWith(new int[] { 1, 2, 3, 4, 5, 6, 8, 9, 10, 12, 15, 16, 18, 20, 24, 27, 30, 36, 40, 45, 48, 54, 60, 72, 80, 81 })
                .And
                .EndWith(new int[] { 1620, 2160, 3240, 6480 })
                .And
                .HaveCount(50);

            PrimeFactorization.Divisor(2095133040).Should().HaveCount(1600); //高度合成数
        }
        [Fact]
        public void DivisorLong()
        {
            PrimeFactorization.Divisor(1L).Should().Equal(new long[] { 1 });
            PrimeFactorization.Divisor(128100283921).Should().Equal(new long[] {
                1,
                71,
                5041,
                357911,
                25411681,
                1804229351,
                128100283921 });
            PrimeFactorization.Divisor(132147483703).Should().Equal(new long[] { 1, 132147483703 });
            PrimeFactorization.Divisor(963761198400).Should().HaveCount(6720); //高度合成数
            PrimeFactorization.Divisor(897612484786617600).Should().HaveCount(103680); //高度合成数

            PrimeFactorization.Divisor(9007199254740997).Should().Equal(new long[] { 1, 9007199254740997 });
            PrimeFactorization.Divisor(89652331L * 96325939).Should().Equal(new long[] {
                1,
                89652331,
                96325939,
                89652331L * 96325939,
            });
        }

        public static TheoryData PrimeFactoringInt_Data => new TheoryData<int, Dictionary<int, int>>
        {
            {
                1,
                new Dictionary<int, int> { }
            },
            {
                1 << 16,
                new Dictionary<int, int> {
                    { 2, 16 },
                }
            },
            {
                2 * 3 * 5,
                new Dictionary<int, int> {
                    { 2, 1 },
                    { 3, 1 },
                    { 5, 1 },
                }
            },
            {
                99991,
                new Dictionary<int, int> {
                    { 99991, 1 },
                }
            },
            {
                2147483647,
                new Dictionary<int, int> {
                    { 2147483647, 1 },
                }
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
                }
            },
        };

        [Theory]
        [MemberData(nameof(PrimeFactoringInt_Data))]
        public void PrimeFactoringInt(int num, Dictionary<int, int> expected)
        {
            PrimeFactorization.PrimeFactoring(num).Should().Equal(expected);
        }

        public static TheoryData PrimeFactoringLong_Data => new TheoryData<long, Dictionary<long, int>>
        {
            {
                1,
                new Dictionary<long, int> { }
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
                }
            },
            {
                132147483703,
                new Dictionary<long, int> {
                    { 132147483703, 1 },
                }
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
                }
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
                }
            },
            {
                89652331L * 96325939,
                new Dictionary<long, int> {
                    { 89652331, 1 },
                    { 96325939, 1 },
                }
            },
            {
                9007199254740997,
                new Dictionary<long, int> {
                    { 9007199254740997, 1 },
                }
            },
        };

        [Theory]
        [MemberData(nameof(PrimeFactoringLong_Data))]
        public void PrimeFactoringLong(long num, Dictionary<long, int> expected)
        {
            PrimeFactorization.PrimeFactoring(num).Should().Equal(expected);
        }

        public static IEnumerable<object[]> StressDivisor_Data()
        {
            var rnd = new Random(227);
            for (int q = 0; q < 10; q++)
            {
                yield return new object[] { rnd.NextLong() % (1L << 45) };
            }
        }

        [Theory]
        [MemberData(nameof(StressDivisor_Data))]
        public void StressDivisor(long n)
        {
            PrimeFactorization.Divisor(n).Should().Equal(Naive(n));

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
    }
}
