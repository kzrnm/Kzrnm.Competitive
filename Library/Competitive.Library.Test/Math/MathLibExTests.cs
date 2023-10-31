using AtCoder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.MathNS
{
    public class MathLibExTests
    {

        public static TheoryData GcdInt_Data => new TheoryData<int, int, int>
        {
            { 1, 2, 1 },
            { 2, 845106, 2 },
            { 152325, 344250, 225 },
            { 344250, 152325, 225 },
            { 43234242, 643643, 1 },
        };
        [Theory]
        [Trait("Category", "Gcd")]
        [MemberData(nameof(GcdInt_Data))]
        public void GcdIntTest(int num1, int num2, int expected)
        {
            MathLibEx.Gcd(num1, num2).Should().Be(expected);
        }
        public static TheoryData GcdIntParams_Data => new TheoryData<int[], int>
        {
            { new int[]{ 344250, 152325, 450 }, 225 },
            { new int[]{ 344250, 152325, 450, 75 }, 75 },
            { new int[]{ 344250, 152325, 450, 60 }, 15 },
            { new int[]{ 344250, 152325, 450, 75, 45 }, 15 },
            { new int[]{ 344250, 152325, 450, 75, 45, 60 }, 15 },
            { new int[]{ 344250, 152325, 450, 75, 45, 6 }, 3 },
            { new int[]{ 344250, 152325, 450, 75, 45, 8 }, 1 },
        };
        [Theory]
        [Trait("Category", "Gcd")]
        [MemberData(nameof(GcdIntParams_Data))]
        public void GcdIntParamsTest(int[] nums, int expected)
        {
            MathLibEx.Gcd(nums).Should().Be(expected);
        }

        public static TheoryData GcdLong_Data => new TheoryData<long, long, long>
        {
            { 1, 2, 1 },
            { 2, 845106, 2 },
            { 230895518700, 230811434700, 23100 },
            { 230811434700, 230895518700, 23100 },
        };
        [Theory]
        [Trait("Category", "Gcd")]
        [MemberData(nameof(GcdLong_Data))]
        public void GcdLongTest(long num1, long num2, long expected)
        {
            MathLibEx.Gcd(num1, num2).Should().Be(expected);
        }
        public static TheoryData GcdLongParams_Data => new TheoryData<long[], long>
        {
            { new long[]{ 230895518700, 230811434700, 1300 }, 100 },
            { new long[]{ 230895518700, 230811434700, 490 }, 70 },
            { new long[]{ 230895518700, 230811434700, 6370, 42 }, 14 },
            { new long[]{ 230895518700, 230811434700, 6370, 42, 13 }, 1 },
        };
        [Theory]
        [Trait("Category", "Gcd")]
        [MemberData(nameof(GcdLongParams_Data))]
        public void GcdLongParamsTest(long[] nums, long expected)
        {
            MathLibEx.Gcd(nums).Should().Be(expected);
        }


        public static TheoryData LcmInt_Data => new TheoryData<int, int, int>
        {
            { 1, 2, 2 },
            { 2, 845106, 845106 },
            { 44250, 2325, 1371750 },
        };
        [Theory]
        [Trait("Category", "Lcm")]
        [MemberData(nameof(LcmInt_Data))]
        public void LcmIntTest(int num1, int num2, int expected)
        {
            MathLibEx.Lcm(num1, num2).Should().Be(expected);
        }
        public static TheoryData LcmIntParams_Data => new TheoryData<int[], int>
        {
            { new int[]{ 44250, 2325, 5, 25 }, 1371750 },
            { new int[]{ 44250, 2325, 5, 25, 3 }, 1371750 },
            { new int[]{ 44250, 2325, 5, 25, 11 }, 15089250 },
        };
        [Theory]
        [Trait("Category", "Lcm")]
        [MemberData(nameof(LcmIntParams_Data))]
        public void LcmIntParamsTest(int[] nums, int expected)
        {
            MathLibEx.Lcm(nums).Should().Be(expected);
        }

        public static TheoryData LcmLong_Data => new TheoryData<long, long, long>
        {
            { 1, 2, 2 },
            { 2, 845106, 845106 },
            { 9999973, 9999991, 99999640000243 },
        };
        [Theory]
        [Trait("Category", "Lcm")]
        [MemberData(nameof(LcmLong_Data))]
        public void LcmLongTest(long num1, long num2, long expected)
        {
            MathLibEx.Lcm(num1, num2).Should().Be(expected);
        }
        public static TheoryData LcmLongParams_Data => new TheoryData<long[], long>
        {
            { new long[]{ 44250, 2325, 5, 25 }, 1371750 },
            { new long[]{ 44250, 2325, 5, 25, 3 }, 1371750 },
            { new long[]{ 44250, 2325, 5, 25, 11 }, 15089250 },
            { new long[]{ 44250, 2325, 5, 25, 11, 9999973 }, 150892092590250 },
            { new long[]{ 44250, 2325, 5, 25, 11, 9999991 }, 150892364196750 },
        };
        [Theory]
        [Trait("Category", "Lcm")]
        [MemberData(nameof(LcmLongParams_Data))]
        public void LcmLongParamsTest(long[] nums, long expected)
        {
            MathLibEx.Lcm(nums).Should().Be(expected);
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
            MathLibEx.Divisor(num).Should().Equal(expected);
        }

        [Fact]
        public void DivisorIntLarge()
        {
            MathLibEx.Divisor(6480).Should()
                .StartWith(new int[] { 1, 2, 3, 4, 5, 6, 8, 9, 10, 12, 15, 16, 18, 20, 24, 27, 30, 36, 40, 45, 48, 54, 60, 72, 80, 81 })
                .And
                .EndWith(new int[] { 1620, 2160, 3240, 6480 })
                .And
                .HaveCount(50);

            MathLibEx.Divisor(2095133040).Should().HaveCount(1600); //高度合成数
        }
        [Fact]
        public void DivisorLong()
        {
            MathLibEx.Divisor(1L).Should().Equal(new long[] { 1 });
            MathLibEx.Divisor(128100283921).Should().Equal(new long[] {
                1,
                71,
                5041,
                357911,
                25411681,
                1804229351,
                128100283921 });
            MathLibEx.Divisor(132147483703).Should().Equal(new long[] { 1, 132147483703 });
            MathLibEx.Divisor(963761198400).Should().HaveCount(6720); //高度合成数
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
            MathLibEx.PrimeFactoring(num).Should().Equal(expected);
        }

        public static IEnumerable<object[]> PrimeFactoringIntStress_Data()
        {
            return Inner().Select(i => new object[] { i });
            static IEnumerable<int> Inner()
            {
                for (int i = 0; i < 100; i++)
                    yield return int.MaxValue - i;
                for (int i = 1; i <= 100; i++)
                    yield return i;
                var sq = (int)Math.Sqrt(int.MaxValue);
                for (int i = -50; i <= 50; i++)
                    yield return sq + i;

                var rnd = new Xoshiro256(227);
                for (int i = 1; i <= 100; i++)
                    yield return (int)(rnd.NextUInt64() >> 33);
            }
        }

        [Theory]
        [MemberData(nameof(PrimeFactoringIntStress_Data))]
        public void PrimeFactoringIntStress(int num)
        {
            long x = 1;
            foreach (var (p, c) in MathLibEx.PrimeFactoring(num))
                x *= ((long)p).Pow(c);
            x.Should().Be(num);
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
        };

        [Theory]
        [MemberData(nameof(PrimeFactoringLong_Data))]
        public void PrimeFactoringLong(long num, Dictionary<long, int> expected)
        {
            MathLibEx.PrimeFactoring(num).Should().Equal(expected);
        }

        [Fact]
        public void Combination()
        {
            for (int i = 0; i <= 10; i++)
                for (int j = 0; j <= i; j++)
                {
                    long n = 1, d = 1;
                    for (int k = 0; k < j; k++)
                    {
                        n *= i - k;
                        d *= k + 1;
                    }
                    MathLibEx.Combination(i, j).Should().Be(n / d);
                }
        }

        [Fact]
        public void CombinationTable()
        {
            var c = MathLibEx.CombinationTable<long>(10);
            for (int i = 0; i <= 10; i++)
                for (int j = 0; j <= i; j++)
                {
                    long n = 1, d = 1;
                    for (int k = 0; k < j; k++)
                    {
                        n *= i - k;
                        d *= k + 1;
                    }
                    c[i][j].Should().Be(n / d);
                }
        }
    }
}
