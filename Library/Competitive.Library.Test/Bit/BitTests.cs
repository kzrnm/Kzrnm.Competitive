using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.Bit;

public class BitTests
{
    public static IEnumerable<(int, int, string)> BitStringInt32_Data =>
    [
        (0, 0, "0" ),
        (1, 3, "001" ),
        (2, 3, "010" ),
        (3, 3, "011" ),
        (4, 3, "100" ),
        (8, 3, "1000" ),
        (int.MaxValue, 32, "01111111111111111111111111111111" ),
        (-1, 32, "11111111111111111111111111111111" ),
        (int.MinValue, 32, "10000000000000000000000000000000" ),
    ];
    [Test]
    [MethodDataSource(nameof(BitStringInt32_Data))]
    [Property("Category", "BitString")]
    public async Task BitStringInt32(int num, int len, string expected)
    {
        await num.ToBitString(len).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(long, int, string)> BitStringInt64_Data =>
    [
        (0, 0, "0" ),
        (1, 3, "001" ),
        (2, 3, "010" ),
        (3, 3, "011" ),
        (4, 3, "100" ),
        (8, 3, "1000" ),

        (long.MaxValue, 64, "0111111111111111111111111111111111111111111111111111111111111111" ),
        (-1, 64, "1111111111111111111111111111111111111111111111111111111111111111" ),
        (long.MinValue, 64, "1000000000000000000000000000000000000000000000000000000000000000" ),
    ];
    [Test]
    [MethodDataSource(nameof(BitStringInt64_Data))]
    [Property("Category", "BitString")]
    public async Task BitStringInt64(long num, int len, string expected)
    {
        await num.ToBitString(len).Should().BeEqualTo(expected);
    }

    public static IEnumerable<(ulong, int, string)> BitStringUInt64_Data =>
    [
        (0, 0, "0" ),
        (1, 3, "001" ),
        (2, 3, "010" ),
        (3, 3, "011" ),
        (4, 3, "100" ),
        (8, 3, "1000" ),

        (ulong.MaxValue, 64, "1111111111111111111111111111111111111111111111111111111111111111" ),
    ];
    [Test]
    [MethodDataSource(nameof(BitStringUInt64_Data))]
    [Property("Category", "BitString")]
    public async Task BitStringUInt64(ulong num, int len, string expected)
    {
        await num.ToBitString(len).Should().BeEqualTo(expected);
    }

    [Test, MultipleAssertions]
    [Property("Category", "BitString")]
    public async Task BitStringDefault()
    {
        await 0.ToBitString().Should().BeEqualTo(new string('0', 32));
        await 0L.ToBitString().Should().BeEqualTo(new string('0', 64));
        await 0UL.ToBitString().Should().BeEqualTo(new string('0', 64));
    }

    public static IEnumerable<(uint, ImmutableArray<int>)> BitEnumerateByte_Data()
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
                                        yield return (num, lst.ToImmutableArray());
                                    }
    }

    [ThousandOfTestcases]
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(BitEnumerateByte_Data))]
    [Property("Category", "BitEnumerate")]
    public async Task BitEnumerateByte(uint num, ImmutableArray<int> expected)
    {
        await num.Bits().Should().BeEquivalentOrderTo(expected);
        await num.Bits().ToArray().Should().BeEquivalentOrderTo(expected);
    }

    public static IEnumerable<(int, int[])> BitEnumerateInt32_Data =>
    [
        (-1, [
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
            20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
            30, 31]
        ),
        (int.MinValue, [31]),
        (1, [0]),
        (3, [0, 1 ]),
        (10, [1, 3]),
        (1 << 20, [20]),
        (0, [] ),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(BitEnumerateInt32_Data))]
    [Property("Category", "BitEnumerate")]
    public async Task BitEnumerateInt32(int num, int[] expected)
    {
        await num.Bits().Should().BeEquivalentOrderTo(expected);
        await num.Bits().ToArray().Should().BeEquivalentOrderTo(expected);
    }

    public static IEnumerable<(uint, int[])> BitEnumerateUInt32_Data =>
    [
        (
            uint.MaxValue,
            [
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
            20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
            30, 31
            ]
        ),
        (1U << 31, [31]),
        (1, [0]),
        (3, [0, 1]),
        (10, [1, 3]),
        (1 << 20, [20]),
        (0, [] ),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(BitEnumerateUInt32_Data))]
    [Property("Category", "BitEnumerate")]
    public async Task BitEnumerateUInt32(uint num, int[] expected)
    {
        await num.Bits().Should().BeEquivalentOrderTo(expected);
        await num.Bits().ToArray().Should().BeEquivalentOrderTo(expected);
    }

    public static IEnumerable<(long, int[])> BitEnumerateInt64_Data =>
    [
        ( -1, [
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
            20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
            30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
            40, 41, 42, 43, 44, 45, 46, 47, 48, 49,
            50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
            60, 61, 62, 63]
        ),
        (long.MinValue, [63]),
        (1, [0]),
        (3, [0, 1]),
        (10, [1, 3]),
        (1L << 20, [20]),
        (0, [] ),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(BitEnumerateInt64_Data))]
    [Property("Category", "BitEnumerate")]
    public async Task BitEnumerateInt64(long num, int[] expected)
    {
        await num.Bits().Should().BeEquivalentOrderTo(expected);
        await num.Bits().ToArray().Should().BeEquivalentOrderTo(expected);
    }

    public static IEnumerable<(ulong, int[])> BitEnumerateUInt64_Data =>
    [
        ( ulong.MaxValue, [
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
            20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
            30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
            40, 41, 42, 43, 44, 45, 46, 47, 48, 49,
            50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
            60, 61, 62, 63]
        ),
        (1UL << 63, [63]),
        (1, [0]),
        (3, [0, 1]),
        (10, [1, 3]),
        (1L << 20, [20]),
        (0, [] ),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(BitEnumerateUInt64_Data))]
    [Property("Category", "BitEnumerate")]
    public async Task BitEnumerateUInt64(ulong num, int[] expected)
    {
        await num.Bits().Should().BeEquivalentOrderTo(expected);
        await num.Bits().ToArray().Should().BeEquivalentOrderTo(expected);
    }

    [Test, MultipleAssertions]
    [Property("Category", "BitEnumerate")]
    public async Task BitEnumerateUInt64Random()
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
                await binary[b].Should().BeEqualTo('1');
                binary[b] = '0';
            }

            await new string(binary).Should().BeEqualTo(zeroBinary);

            await value.Bits().ToArray().Should().BeEquivalentOrderTo(value.Bits().Cast<int>().ToArray());
        }
    }
}