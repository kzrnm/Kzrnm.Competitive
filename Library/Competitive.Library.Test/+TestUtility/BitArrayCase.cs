using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit.Sdk;

namespace Kzrnm.Competitive.Testing;

public class BitArrayCase(bool[] bits) : IXunitSerializable
{
    public BitArrayCase(BitArrayCase other) : this(other.ToBoolArray()) { }
    public BitArrayCase() : this([]) { }

    public BitArray ToBitArray() => new(bits);
    public bool[] ToBoolArray() => (bool[])bits.Clone();
    public int Length => bits.Length;
    public bool this[int index]
    {
        get => bits[index];
        set => bits[index] = value;
    }

    public override string ToString()
    {
        var chr = new char[Length];
        for (int i = 0; i < chr.Length; i++)
        {
            chr[i] = bits[i] ? '1' : '0';
        }
        return new string(chr);
    }

    public IEnumerator<bool> GetEnumerator() => ((IEnumerable<bool>)bits).GetEnumerator();
    void IXunitSerializable.Deserialize(IXunitSerializationInfo info)
    {
        bits = info.GetValue<bool[]>(nameof(bits));
    }

    void IXunitSerializable.Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(bits), bits);
    }

    public static BitArrayCase MakeRandomCase(Random random, int length)
    {
        var b = new bool[length];
        for (int i = 0; i < b.Length; i++)
        {
            b[i] = random.Next(2) != 0;
        }
        return new BitArrayCase(b);
    }

    public static IEnumerable<TheoryDataRow<BitArrayCase>> RandomCases()
    {
        var rnd = new Random(227);
        for (int i = 1; i < 12; i++)
            for (int q = 0; q < 4; q++)
            {
                yield return MakeRandomCase(rnd, 32 * i - 1);
                yield return MakeRandomCase(rnd, 32 * i);
                yield return MakeRandomCase(rnd, 32 * i + 1);
            }
        for (int q = 0; q < 256; q++)
        {
            yield return MakeRandomCase(rnd, rnd.Next(1, 140));
        }
    }

    public static TheoryData<string> LongBinaryTexts() => LongBinaryTexts(1000);
    public static TheoryData<string> LongBinaryTexts(int maxLength)
    {
        return new(Inner(maxLength).Distinct());
        static IEnumerable<string> Inner(int maxLength)
        {
            for (int i = 0; i < 20; i++)
            {
                yield return System.Convert.ToString(i, 2);
            }
            for (int i = 0; i < 20; i++)
            {
                yield return System.Convert.ToString(i + (1L << 32) - 10, 2);
            }
            for (int i = 0; i < 20; i++)
            {
                yield return System.Convert.ToString(i + (1L << 40), 2);
            }

            var rnd = new Random(227);
            for (int i = 0; i < 1000; i++)
            {
                int len = rnd.Next(1, maxLength) + 1;
                var chrs = new char[len];
                for (int j = 0; j < chrs.Length; j++)
                {
                    chrs[j] = (char)(rnd.Next(2) + '0');
                }
                yield return new string(chrs);
            }
        }
    }
}
