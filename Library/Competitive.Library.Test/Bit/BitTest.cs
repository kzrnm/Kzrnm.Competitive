using FluentAssertions;
using System;
using System.Runtime.Intrinsics.X86;

namespace Kzrnm.Competitive.Testing.Bits
{
    public class BitTests
    {
        sealed class OnlyX64TheoryAttribute : TheoryAttribute
        {
            public OnlyX64TheoryAttribute()
            {
                if (!Bmi1.X64.IsSupported)
                {
                    Skip = "Not Support Bmi1.X64";
                }
            }
        }


        public static TheoryData BitStringInt32_Data => new TheoryData<int, int, string>
        {
            { 0, 0, "0" },
            { 1, 3, "001" },
            { 2, 3, "010" },
            { 3, 3, "011" },
            { 4, 3, "100" },
            { 8, 3, "1000" },

            { int.MaxValue, 32, "01111111111111111111111111111111" },
            { -1, 32, "11111111111111111111111111111111" },
            { int.MinValue, 32, "10000000000000000000000000000000" },
        };
        [Theory]
        [MemberData(nameof(BitStringInt32_Data))]
        [Trait("Category", "BitString")]
        public void BitStringInt32Test(int num, int len, string expected)
        {
            num.ToBitString(len).Should().Be(expected);
        }

        public static TheoryData BitStringInt64_Data => new TheoryData<long, int, string>
        {
            { 0, 0, "0" },
            { 1, 3, "001" },
            { 2, 3, "010" },
            { 3, 3, "011" },
            { 4, 3, "100" },
            { 8, 3, "1000" },

            { long.MaxValue, 64, "0111111111111111111111111111111111111111111111111111111111111111" },
            { -1, 64, "1111111111111111111111111111111111111111111111111111111111111111" },
            { long.MinValue, 64, "1000000000000000000000000000000000000000000000000000000000000000" },
        };
        [Theory]
        [MemberData(nameof(BitStringInt64_Data))]
        [Trait("Category", "BitString")]
        public void BitStringInt64Test(long num, int len, string expected)
        {
            num.ToBitString(len).Should().Be(expected);
        }

        public static TheoryData BitStringUInt64_Data => new TheoryData<ulong, int, string>
        {
            { 0, 0, "0" },
            { 1, 3, "001" },
            { 2, 3, "010" },
            { 3, 3, "011" },
            { 4, 3, "100" },
            { 8, 3, "1000" },

            { ulong.MaxValue, 64, "1111111111111111111111111111111111111111111111111111111111111111" },
        };
        [Theory]
        [MemberData(nameof(BitStringUInt64_Data))]
        [Trait("Category", "BitString")]
        public void BitStringUInt64Test(ulong num, int len, string expected)
        {
            num.ToBitString(len).Should().Be(expected);
        }

        [Fact]
        [Trait("Category", "BitString")]
        public void BitStringDefaultTest()
        {
            0.ToBitString().Should().Be(new string('0', 32));
            0L.ToBitString().Should().Be(new string('0', 64));
            0UL.ToBitString().Should().Be(new string('0', 64));
        }


        public static TheoryData BitEnumerateInt32_Data => new TheoryData<int, int[]>
        {
            { -1, new[]{
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
                20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
                30, 31 } },
            { int.MinValue, new[]{ 31 } },
            { 1, new[]{ 0 } },
            { 3, new[]{ 0, 1  } },
            { 10, new[]{ 1, 3 } },
            { 1 << 20, new[]{ 20 } },
            { 0, Array.Empty<int>() },
        };
        [OnlyX64Theory]
        [MemberData(nameof(BitEnumerateInt32_Data))]
        [Trait("Category", "BitEnumerate")]
        public void BitEnumerateInt32Test(int num, int[] expected)
        {
            num.Bits().Should().Equal(expected);
        }

        public static TheoryData BitEnumerateUInt32_Data => new TheoryData<uint, int[]>
        {
            { uint.MaxValue, new[]{
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
                20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
                30, 31 } },
            { 1U << 31, new[]{ 31 } },
            { 1, new[]{ 0 } },
            { 3, new[]{ 0, 1 }},
            { 10, new[]{ 1, 3 }},
            { 1 << 20, new[]{ 20 } },
            { 0, Array.Empty<int>() },
        };
        [OnlyX64Theory]
        [MemberData(nameof(BitEnumerateUInt32_Data))]
        [Trait("Category", "BitEnumerate")]
        public void BitEnumerateUInt32Test(uint num, int[] expected)
        {
            num.Bits().Should().Equal(expected);
        }

        public static TheoryData BitEnumerateInt64_Data => new TheoryData<long, int[]>
        {
            { -1, new[]{
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
                20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
                30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
                40, 41, 42, 43, 44, 45, 46, 47, 48, 49,
                50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
                60, 61, 62, 63 } },
            { long.MinValue, new[]{ 63 } },
            { 1, new[]{ 0 } },
            { 3, new[]{ 0, 1 }},
            { 10, new[]{ 1, 3 }},
            { 1L << 20, new[]{ 20 } },
            { 0, Array.Empty<int>() },
        };
        [OnlyX64Theory]
        [MemberData(nameof(BitEnumerateInt64_Data))]
        [Trait("Category", "BitEnumerate")]
        public void BitEnumerateInt64Test(long num, int[] expected)
        {
            num.Bits().Should().Equal(expected);
        }

        public static TheoryData BitEnumerateUInt64_Data => new TheoryData<ulong, int[]>
        {
            { ulong.MaxValue, new[]{
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
                20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
                30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
                40, 41, 42, 43, 44, 45, 46, 47, 48, 49,
                50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
                60, 61, 62, 63 } },
            { 1UL << 63, new[]{ 63 } },
            { 1, new[]{ 0 } },
            { 3, new[]{ 0, 1 }},
            { 10, new[]{ 1, 3 }},
            { 1L << 20, new[]{ 20 } },
            { 0, Array.Empty<int>() },
        };
        [OnlyX64Theory]
        [MemberData(nameof(BitEnumerateUInt64_Data))]
        [Trait("Category", "BitEnumerate")]
        public void BitEnumerateUInt64Test(ulong num, int[] expected)
        {
            num.Bits().Should().Equal(expected);
        }
    }
}
