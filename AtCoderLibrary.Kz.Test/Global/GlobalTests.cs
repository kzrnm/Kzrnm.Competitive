using FluentAssertions;
using System.Numerics;
using Xunit;

namespace AtCoder
{
    public class GlobalTests
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
            Global.Gcd(num1, num2).Should().Be(expected);
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
            Global.Gcd(nums).Should().Be(expected);
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
            Global.Gcd(num1, num2).Should().Be(expected);
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
            Global.Gcd(nums).Should().Be(expected);
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
            Global.Lcm(num1, num2).Should().Be(expected);
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
            Global.Lcm(nums).Should().Be(expected);
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
            Global.Lcm(num1, num2).Should().Be(expected);
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
            Global.Lcm(nums).Should().Be(expected);
        }


        [Theory]
        [InlineData(1, 0, 1)]
        [InlineData(1, 100, 1)]
        [InlineData(2, 0, 1)]
        [InlineData(2, 10, 1024)]
        [InlineData(2, 62, 4611686018427387904L)]
        [InlineData(4, 5, 1024)]
        [InlineData(17, 4, 83521)]
        [InlineData(17, 0, 1)]
        public void Pow(long x, int y, long expected)
        {
            Global.Pow(x, y).Should().Be(expected);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-0")]
        [InlineData("1234567890")]
        [InlineData("-1234567890")]
        [InlineData("-321903178318273190741289045432543262643262363268947289078189571895414896419761976417696741971241434468712463124873424184343720834082342428104301240873442148217850245031275176770641764013760671076131046076403176403176071643164316743854343425407")]
        [InlineData("43248098909218931428148434365435432523436436367208340823424281043012408734421482107")]
        [InlineData("321903178318273190741289041628089476545235432414344687124631248734241843437208340823424281043012408734421482107")]
        public void ParseBigInteger(string input)
        {
            Global.ParseBigInteger(input).Should().Be(BigInteger.Parse(input));
        }


        [Theory]
        [InlineData(int.MaxValue, 31, 30, 0)]
        [InlineData(int.MinValue, 1, 31, 31)]
        [InlineData(-1, 32, 31, 0)]
        [InlineData(1, 1, 0, 0)]
        [InlineData(0, 0, 0, 32)]
        public void BitOperationInt32(int input, int popCount, int msb, int lsb)
        {
            Global.PopCount(input).Should().Be(popCount);
            Global.MSB(input).Should().Be(msb);
            Global.LSB(input).Should().Be(lsb);
        }

        [Theory]
        [InlineData(uint.MaxValue, 32, 31, 0)]
        [InlineData(0, 0, 0, 32)]
        [InlineData(1, 1, 0, 0)]
        public void BitOperationUInt32(uint input, int popCount, int msb, int lsb)
        {
            Global.PopCount(input).Should().Be(popCount);
            Global.MSB(input).Should().Be(msb);
            Global.LSB(input).Should().Be(lsb);
        }
        [Theory]
        [InlineData(long.MaxValue, 63, 62, 0)]
        [InlineData(long.MinValue, 1, 63, 63)]
        [InlineData(-1, 64, 63, 0)]
        [InlineData(1, 1, 0, 0)]
        [InlineData(0, 0, 0, 64)]
        public void BitOperationInt64(long input, int popCount, int msb, int lsb)
        {
            Global.PopCount(input).Should().Be(popCount);
            Global.MSB(input).Should().Be(msb);
            Global.LSB(input).Should().Be(lsb);
        }

        [Theory]
        [InlineData(ulong.MaxValue, 64, 63, 0)]
        [InlineData(0, 0, 0, 64)]
        [InlineData(1, 1, 0, 0)]
        public void BitOperationUInt64(ulong input, int popCount, int msb, int lsb)
        {
            Global.PopCount(input).Should().Be(popCount);
            Global.MSB(input).Should().Be(msb);
            Global.LSB(input).Should().Be(lsb);
        }
    }
}
