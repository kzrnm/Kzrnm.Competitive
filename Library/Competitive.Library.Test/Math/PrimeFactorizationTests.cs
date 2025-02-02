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
                    PrimeFactorization.IsPrime(x).ShouldBe(p.IsPrime(x), $"Value: {x}");
                }
            }
            PrimeFactorization.IsPrime(200560490131).ShouldBe(true);
            PrimeFactorization.IsPrime(92709568269121).ShouldBe(true);
            PrimeFactorization.IsPrime(9007199254740997).ShouldBe(true);
            PrimeFactorization.IsPrime(1162193L * 1347377).ShouldBe(false);
            PrimeFactorization.IsPrime(89652331L * 96325939).ShouldBe(false);
        }

        public static TheoryData<int, int[]> DivisorInt_Data => new()
    {
        {
            1,
            [1]
        },
        {
            1 << 16,
            Enumerable.Range(0, 17).Select(i => 1 << i).ToArray()
        },
        {
            49,
            [1, 7, 49,]
        },
        {
            2 * 3 * 5,
            [1, 2, 3, 5, 6, 10, 15, 30,]
        },
        {
            720,
            [
                1, 2, 3, 4, 5, 6, 8, 9, 10,
                12, 15, 16, 18, 20, 24, 30,
                36, 40, 45, 48, 60, 72, 80,
                90, 120, 144, 180, 240, 360, 720,
            ]
        },
        {
            2147483647,
            [1, 2147483647,]
        },
    };

        [Theory]
        [MemberData(nameof(DivisorInt_Data))]
        public void DivisorInt(int num, int[] expected)
        {
            PrimeFactorization.Divisor(num).ShouldBe(expected);
        }

        [Fact]
        public void DivisorIntLarge()
        {
            var divisor6480 = PrimeFactorization.Divisor(6480);
            divisor6480.Length.ShouldBe(50);
            divisor6480[..26].ShouldBe([1, 2, 3, 4, 5, 6, 8, 9, 10, 12, 15, 16, 18, 20, 24, 27, 30, 36, 40, 45, 48, 54, 60, 72, 80, 81]);
            divisor6480[^4..].ShouldBe([1620, 2160, 3240, 6480]);

            PrimeFactorization.Divisor(2095133040).Length.ShouldBe(1600); //高度合成数
        }
        [Fact]
        public void DivisorLong()
        {
            PrimeFactorization.Divisor(1L).ShouldBe([1]);
            PrimeFactorization.Divisor(128100283921).ShouldBe([
                1,
            71,
            5041,
            357911,
            25411681,
            1804229351,
            128100283921]);
            PrimeFactorization.Divisor(132147483703).ShouldBe([1, 132147483703]);
            PrimeFactorization.Divisor(963761198400).Length.ShouldBe(6720); //高度合成数
            PrimeFactorization.Divisor(897612484786617600).Length.ShouldBe(103680); //高度合成数

            PrimeFactorization.Divisor(9007199254740997).ShouldBe([1, 9007199254740997]);
            PrimeFactorization.Divisor(89652331L * 96325939).ShouldBe([
                1,
            89652331,
            96325939,
            89652331L * 96325939,
        ]);
        }

        public static TheoryData<int, SerializableTuple<int, int>[]> PrimeFactoringInt_Data => new()
    {
        {
            1,
            []
        },
        {
            1 << 16,
            [
                (2, 16),
            ]
        },
        {
            2 * 3 * 5,
            [
                (2, 1),
                (3, 1),
                (5, 1),
            ]
        },
        {
            99991,
            [
                (99991, 1),
            ]
        },
        {
            2147483647,
            [
                (2147483647, 1),
            ]
        },
        {
            2095133040, //高度合成数
            [
                (2, 4),
                (3, 4),
                (5, 1),
                (7, 1),
                (11, 1),
                (13, 1),
                (17, 1),
                (19, 1),
            ]
        },
    };

        [Theory]
        [MemberData(nameof(PrimeFactoringInt_Data))]
        public void PrimeFactoringInt(int num, SerializableTuple<int, int>[] expected)
        {
            var f = PrimeFactorization.PrimeFactoring(num);
            f.Count.ShouldBe(expected.Length);
            foreach (var (k, count) in expected)
            {
                f.ShouldContainKeyAndValue(k, count);
            }
        }

        public static TheoryData<long, SerializableTuple<long, int>[]> PrimeFactoringLong_Data => new()
    {
        {
            1,
            []
        },
        {
            903906555552,
            [
                (2, 5),
                (3, 8),
                (7, 1),
                (11, 2),
                (13, 1),
                (17, 1),
                (23, 1),
            ]
        },
        {
            132147483703,
            [
                (132147483703, 1),
            ]
        },
        {
            963761198400, //高度合成数
            [
                (2, 6),
                (3, 4),
                (5, 2),
                (7, 1),
                (11, 1),
                (13, 1),
                (17, 1),
                (19, 1),
                (23, 1),
            ]
        },
        {
            897612484786617600, //高度合成数
            [
                (2, 8),
                (3, 4),
                (5, 2),
                (7, 2),
                (11, 1),
                (13, 1),
                (17, 1),
                (19, 1),
                (23, 1),
                (29, 1),
                (31, 1),
                (37, 1),
            ]
        },
        {
            89652331L * 96325939,
            [
                (89652331, 1),
                (96325939, 1),
            ]
        },
        {
            9007199254740997,
            [
                (9007199254740997, 1),
            ]
        },
    };

        [Theory]
        [MemberData(nameof(PrimeFactoringLong_Data))]
        public void PrimeFactoringLong(long num, SerializableTuple<long, int>[] expected)
        {
            var f = PrimeFactorization.PrimeFactoring(num);
            f.Count.ShouldBe(expected.Length);
            foreach (var (k, count) in expected)
            {
                f.ShouldContainKeyAndValue(k, count);
            }
        }

        public static IEnumerable<TheoryDataRow<long>> StressDivisor_Data()
        {
            var rnd = new Random(227);
            for (int q = 0; q < 10; q++)
                yield return rnd.NextLong() % (1L << 45);
        }

        [Theory]
        [MemberData(nameof(StressDivisor_Data))]
        public void StressDivisor(long n)
        {
            PrimeFactorization.Divisor(n).ShouldBe(NaiveDivisor(n));
        }

        [Fact]
        public void SmallDivisor()
        {
            for (int i = 1; i < 257 * 257 + 50; i++)
                PrimeFactorization.Divisor(i)
                    .ShouldBe(NaiveDivisor(i).Select(n => checked((int)n)), $"Value: {i}");
        }


        static long[] NaiveDivisor(long n)
        {
            if (n <= 1) return [1];

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