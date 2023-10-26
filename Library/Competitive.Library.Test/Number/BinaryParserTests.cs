using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Number
{
    public class BinaryParserTests
    {
        public static IEnumerable<Tuple<uint>> ParseUInt32_Data()
        {
            for (uint i = 0; i < 100; i++)
            {
                yield return Tuple.Create(i);
                yield return Tuple.Create(uint.MaxValue - i);
            }
            var rnd = new Xoshiro256(227);
            for (int i = 0; i < 100; i++)
            {
                yield return Tuple.Create(rnd.NextUInt32());
            }
        }

        [Theory]
        [TupleMemberData(nameof(ParseUInt32_Data))]
        public void ParseUInt32(uint num)
        {
            var str = System.Convert.ToString(num, 2);
            BinaryParser.ParseUInt32(str).Should().Be(num);
        }

        public static IEnumerable<Tuple<ulong>> ParseUInt64_Data()
        {
            for (ulong i = 0; i < 100; i++)
            {
                yield return Tuple.Create(i);
                yield return Tuple.Create(ulong.MaxValue - i);
            }
            var rnd = new Xoshiro256(227);
            for (int i = 0; i < 100; i++)
            {
                yield return Tuple.Create(rnd.NextUInt64());
            }
        }

        [Theory]
        [TupleMemberData(nameof(ParseUInt64_Data))]
        public void ParseUInt64(ulong num)
        {
            var str = System.Convert.ToString((long)num, 2);
            BinaryParser.ParseUInt64(str).Should().Be(num);
        }


        public static IEnumerable<Tuple<string>> ParseBitArray_Data()
        {
            for (int i = 0; i < 20; i++)
            {
                yield return Tuple.Create(System.Convert.ToString(i, 2));
            }
            for (int i = 0; i < 20; i++)
            {
                yield return Tuple.Create(System.Convert.ToString(i + (1L << 32) - 10, 2));
            }
            for (int i = 0; i < 20; i++)
            {
                yield return Tuple.Create(System.Convert.ToString(i + (1L << 40), 2));
            }

            var rnd = new Xoshiro256(227);
            for (int i = 0; i < 1000; i++)
            {
                int len = rnd.NextInt32(1, 1000);
                var chrs = new char[len];
                for (int j = 0; j < chrs.Length; j++)
                {
                    chrs[j] = (char)(rnd.NextInt32(2) + '0');
                }
                yield return Tuple.Create(new string(chrs));
            }
        }

        [Theory]
        [TupleMemberData(nameof(ParseBitArray_Data))]
        public void ParseBitArray(string input)
        {
            var naive = input.Select(c => c != '0').ToArray();
            BinaryParser.ParseBitArray(input).Cast<bool>().Should().Equal(naive);
        }

    }
}
