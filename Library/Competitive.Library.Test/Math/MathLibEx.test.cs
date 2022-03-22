using AtCoder;
using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.MathNS
{
    [Verify] // verification-helper: PROBLEM https://judge.yosupo.jp/problem/aplusb
    public class MathLibExTests
    {

        public static TheoryData GcdInt_Data = new TheoryData<int, int, int>
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
        public static TheoryData GcdIntParams_Data = new TheoryData<int[], int>
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

        public static TheoryData GcdLong_Data = new TheoryData<long, long, long>
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
        public static TheoryData GcdLongParams_Data = new TheoryData<long[], long>
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


        public static TheoryData LcmInt_Data = new TheoryData<int, int, int>
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
        public static TheoryData LcmIntParams_Data = new TheoryData<int[], int>
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

        public static TheoryData LcmLong_Data = new TheoryData<long, long, long>
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
        public static TheoryData LcmLongParams_Data = new TheoryData<long[], long>
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

        [Fact]
        public void DivisorInt()
        {
            MathLibEx.Divisor(1).Should().Equal(new int[] { 1 });
            MathLibEx.Divisor(1000000007).Should().Equal(new int[] { 1, 1000000007 });
            MathLibEx.Divisor(49).Should().Equal(new int[] { 1, 7, 49 });
            MathLibEx.Divisor(720).Should().Equal(new int[] {
                1, 2, 3, 4, 5, 6, 8, 9, 10,
                12, 15, 16, 18, 20, 24, 30,
                36, 40, 45, 48, 60, 72, 80,
                90, 120, 144, 180, 240, 360, 720
            });
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
            var c = MathLibEx.CombinationTable<long, LongOperator>(10);
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
