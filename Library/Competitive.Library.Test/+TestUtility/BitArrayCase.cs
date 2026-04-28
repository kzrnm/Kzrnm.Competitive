using System.Collections;
using System.Collections.Immutable;

namespace Kzrnm.Competitive.Testing;

public readonly record struct BitArrayCase(ImmutableArray<bool> Bits)
{
    public BitArrayCase(IEnumerable<bool> bits) : this(bits.ToImmutableArray()) { }
    public BitArray ToBitArray() => new(Bits.ToArray());
    public bool[] ToBoolArray() => Bits.ToArray();
    public int Length => Bits.Length;
    public bool this[int index] => Bits[index];

    public override string ToString()
    {
        var chr = new char[Length];
        for (int i = 0; i < chr.Length; i++)
        {
            chr[i] = Bits[i] ? '1' : '0';
        }
        return new string(chr);
    }

    public IEnumerator<bool> GetEnumerator() => ((IEnumerable<bool>)Bits).GetEnumerator();

    public static BitArrayCase MakeRandomCase(Random random, int length)
    {
        var b = new bool[length];
        for (int i = 0; i < b.Length; i++)
        {
            b[i] = random.Next(2) != 0;
        }
        return new BitArrayCase(b.ToImmutableArray());
    }

    public static IEnumerable<BitArrayCase> RandomCases()
    {
        var rnd = new Random(227);
        for (int i = 1; i < 12; i++)
            for (int q = 0; q < 4; q++)
            {
                yield return MakeRandomCase(rnd, 32 * i - 1);
                yield return MakeRandomCase(rnd, 32 * i);
                yield return MakeRandomCase(rnd, 32 * i + 1);
            }
        for (int q = 0; q < 128; q++)
        {
            yield return MakeRandomCase(rnd, rnd.Next(1, 140));
        }
    }

    public static IEnumerable<string> LongBinaryTexts() => LongBinaryTexts(1000);
    public static IEnumerable<string> LongBinaryTexts(int maxLength)
    {
        return Inner(maxLength).Distinct();
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