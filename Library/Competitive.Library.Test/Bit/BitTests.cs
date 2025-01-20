using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.Bits
{
    public class BitTests
    {
        public static TheoryData<int, int, string> BitStringInt32_Data => new()
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
        public void BitStringInt32(int num, int len, string expected)
        {
            num.ToBitString(len).ShouldBe(expected);
        }

        public static TheoryData<long, int, string> BitStringInt64_Data => new()
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
        public void BitStringInt64(long num, int len, string expected)
        {
            num.ToBitString(len).ShouldBe(expected);
        }

        public static TheoryData<ulong, int, string> BitStringUInt64_Data => new()
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
        public void BitStringUInt64(ulong num, int len, string expected)
        {
            num.ToBitString(len).ShouldBe(expected);
        }

        [Fact]
        [Trait("Category", "BitString")]
        public void BitStringDefault()
        {
            0.ToBitString().ShouldBe(new string('0', 32));
            0L.ToBitString().ShouldBe(new string('0', 64));
            0UL.ToBitString().ShouldBe(new string('0', 64));
        }

        public static IEnumerable<(uint, int[])> BitEnumerateByte_Data()
        {
            var s = new int[8];
            for (s[0] = 0; s[0] < 2; s[0]++)
                for (s[1] = 0; s[1] < 2; s[1]++)
                    for (s[2] = 0; s[2] < 2; s[2]++)
                        for (s[3] = 0; s[3] < 2; s[3]++)
                            for (s[4] = 0; s[4] < 2; s[4]++)
                                for (s[5] = 0; s[5] < 2; s[5]++)
                                    for (s[6] = 0; s[6] < 2; s[6]++)
                                        for (s[7] = 0; s[7] < 2; s[7]++)
                                        {
                                            uint num = 0;
                                            var lst = new List<int>(8);
                                            for (int i = 0; i < s.Length; i++)
                                                if (s[i] != 0)
                                                {
                                                    num |= 1u << i;
                                                    lst.Add(i);
                                                }
                                            yield return (num, lst.ToArray());
                                        }
        }
        [Theory]
        [MemberData(nameof(BitEnumerateByte_Data))]
        [Trait("Category", "BitEnumerate")]
        public void BitEnumerateByte(uint num, int[] expected)
        {
            num.Bits().ShouldBe(expected);
            num.Bits().ToArray().ShouldBe(expected);
        }

        public static TheoryData<int, int[]> BitEnumerateInt32_Data => new()
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
        [Theory]
        [MemberData(nameof(BitEnumerateInt32_Data))]
        [Trait("Category", "BitEnumerate")]
        public void BitEnumerateInt32(int num, int[] expected)
        {
            num.Bits().ShouldBe(expected);
            num.Bits().ToArray().ShouldBe(expected);
        }

        public static TheoryData<uint, int[]> BitEnumerateUInt32_Data => new()
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
        [Theory]
        [MemberData(nameof(BitEnumerateUInt32_Data))]
        [Trait("Category", "BitEnumerate")]
        public void BitEnumerateUInt32(uint num, int[] expected)
        {
            num.Bits().ShouldBe(expected);
            num.Bits().ToArray().ShouldBe(expected);
        }

        public static TheoryData<long, int[]> BitEnumerateInt64_Data => new()
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
        [Theory]
        [MemberData(nameof(BitEnumerateInt64_Data))]
        [Trait("Category", "BitEnumerate")]
        public void BitEnumerateInt64(long num, int[] expected)
        {
            num.Bits().ShouldBe(expected);
            num.Bits().ToArray().ShouldBe(expected);
        }

        public static TheoryData<ulong, int[]> BitEnumerateUInt64_Data => new()
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
        [Theory]
        [MemberData(nameof(BitEnumerateUInt64_Data))]
        [Trait("Category", "BitEnumerate")]
        public void BitEnumerateUInt64(ulong num, int[] expected)
        {
            num.Bits().ShouldBe(expected);
            num.Bits().ToArray().ShouldBe(expected);
        }

        [Fact]
        [Trait("Category", "BitEnumerate")]
        public void BitEnumerateUInt64Random()
        {
            var rnd = new Random(227);
            var array = new ulong[2000];
            rnd.NextBytes(MemoryMarshal.AsBytes(array.AsSpan()));
            var zeroBinary = new string('0', 64);
            foreach (var value in array)
            {
                var binary = System.Convert.ToString((long)value, 2).PadLeft(64, '0').ToCharArray();
                Array.Reverse(binary);
                foreach (var b in value.Bits())
                {
                    binary[b].ShouldBe('1');
                    binary[b] = '0';
                }

                new string(binary).ShouldBe(zeroBinary);

                value.Bits().ToArray()
                    .ShouldBe(value.Bits().Cast<int>().ToArray());
            }
        }
    }
}
