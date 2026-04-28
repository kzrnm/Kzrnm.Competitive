using Kzrnm.Competitive.Internal;
using System.Collections;
using System.Collections.Immutable;

namespace Kzrnm.Competitive.Testing;

public readonly record struct BitMatrixCase
{
    public class Builder(bool[][] matrix)
    {
        public Builder(int height, int width) : this(Enumerable.Repeat(width, height).Select(w => new bool[w]).ToArray()) { }
        public Builder(BitMatrixCase bmc) : this(bmc.ToBoolArray()) { }

        public bool this[int h, int w]
        {
            get => matrix[h][w];
            set => matrix[h][w] = value;
        }

        public int Height => matrix.Length;
        public int Width => matrix.Length == 0 ? 0 : matrix[0].Length;

        public BitMatrix64 ToBitMatrix64()
            => new(matrix);
        public BitMatrix<UInt128> ToBitMatrix128()
            => new(matrix);
        public BitOrMatrix ToBitOrMatrix()
            => new(matrix.Select(c => new BitArray(c)).ToArray());
        public BitMatrix ToBitMatrix()
            => new(matrix.Select(c => new BitArray(c)).ToArray());
    }
    public BitMatrixCase(ArrayMatrixKind kind)
    {
        if (kind == ArrayMatrixKind.Normal)
            throw new InvalidOperationException();
        this.Kind = kind;
    }
    public BitMatrixCase(ImmutableArray<BitArrayCase> cases)
    {
        if (cases.Length == 0 || cases[0].Length == 0)
            throw new InvalidOperationException();
        Kind = ArrayMatrixKind.Normal;
        this.cases = cases;
    }
    public BitMatrixCase() : this([]) { }

    public ArrayMatrixKind Kind { get; }
    readonly ImmutableArray<BitArrayCase> cases;

    public int Height => cases.Length;
    public int Width => cases[0].Length;

    public BitMatrix64 ToBitMatrix64()
        => Kind == ArrayMatrixKind.Normal
        ? new(ToBoolArray())
        : new(Kind);
    public BitMatrix<UInt128> ToBitMatrix128()
        => Kind == ArrayMatrixKind.Normal
        ? new(ToBoolArray())
        : new(Kind);
    public BitOrMatrix ToBitOrMatrix()
        => Kind == ArrayMatrixKind.Normal
        ? new(cases.Select(c => c.ToBitArray()).ToArray())
        : new(Kind);
    public BitMatrix ToBitMatrix()
        => Kind == ArrayMatrixKind.Normal
        ? new(cases.Select(c => c.ToBitArray()).ToArray())
        : new(Kind);

    public bool[][] ToBoolArray() => cases.Select(c => c.ToBoolArray()).ToArray();

    public bool this[int h, int w] => cases[h][w];


    public override string ToString()
    {
        return Kind == ArrayMatrixKind.Normal
            ? $"{nameof(Height)} = {Height}, {nameof(Width)} = {Width}"
            : Kind.ToString();
    }

    public IEnumerator<BitArrayCase> GetEnumerator() => ((IEnumerable<BitArrayCase>)cases).GetEnumerator();
    public static BitMatrixCase MakeRandomCase(Random random, int height, int width)
    {
        var b = new BitArrayCase[height];
        for (int i = 0; i < b.Length; i++)
        {
            b[i] = BitArrayCase.MakeRandomCase(random, width);
        }
        return new BitMatrixCase(b.ToImmutableArray());
    }

    public static IEnumerable<BitMatrixCase> RandomCases(int maxWidth)
    {
        var rnd = new Random(227);
        for (int i = 1; i < 12; i++)
            for (int j = -1; j <= 1; j++)
            {
                int width = 32 * i + j;
                if (width <= maxWidth)
                    for (int q = 0; q < 4; q++)
                    {
                        int height = rnd.Next(1, 140);
                        yield return MakeRandomCase(rnd, height, width);
                    }
            }
        for (int q = 0; q < 256; q++)
        {
            int height = rnd.Next(1, 140);
            int width = rnd.Next(0, maxWidth) + 1;
            yield return MakeRandomCase(rnd, height, width);
        }
    }

    public static IEnumerable<(BitMatrixCase, BitMatrixCase)> RandomAddCases(int maxWidth)
    {
        var rnd = new Random(227);
        yield return (new(ArrayMatrixKind.Zero), new(ArrayMatrixKind.Zero));
        yield return (new(ArrayMatrixKind.Zero), new(ArrayMatrixKind.Identity));
        yield return (new(ArrayMatrixKind.Zero), MakeRandomCase(rnd, maxWidth, maxWidth));
        yield return (new(ArrayMatrixKind.Identity), MakeRandomCase(rnd, maxWidth, maxWidth));

        for (int i = 1; i < 12; i++)
            for (int j = -1; j <= 1; j++)
            {
                int width = 32 * i + j;
                if (width <= maxWidth)
                    for (int q = 0; q < 4; q++)
                    {
                        int height = rnd.Next(1, 140);
                        yield return (MakeRandomCase(rnd, height, width), MakeRandomCase(rnd, height, width));
                    }
            }
        for (int q = 0; q < 256; q++)
        {
            int height = rnd.Next(1, 140);
            int width = rnd.Next(0, maxWidth) + 1;
            yield return (MakeRandomCase(rnd, height, width), MakeRandomCase(rnd, height, width));
        }
    }

    public static IEnumerable<(BitMatrixCase, BitMatrixCase)> RandomMultiplyCases(int maxWidth)
    {
        var rnd = new Random(227);
        yield return (new(ArrayMatrixKind.Zero), new(ArrayMatrixKind.Zero));
        yield return (new(ArrayMatrixKind.Identity), new(ArrayMatrixKind.Zero));
        yield return (new(ArrayMatrixKind.Zero), new(ArrayMatrixKind.Identity));
        yield return (new(ArrayMatrixKind.Identity), new(ArrayMatrixKind.Identity));
        yield return (new(ArrayMatrixKind.Zero), MakeRandomCase(rnd, maxWidth, maxWidth));
        yield return (MakeRandomCase(rnd, maxWidth, maxWidth), new(ArrayMatrixKind.Zero));
        yield return (new(ArrayMatrixKind.Identity), MakeRandomCase(rnd, maxWidth, maxWidth));
        yield return (MakeRandomCase(rnd, maxWidth, maxWidth), new(ArrayMatrixKind.Identity));

        for (int i = 1; i < 12; i++)
            for (int j = -1; j <= 1; j++)
            {
                int width1 = 32 * i + j;
                if (width1 <= maxWidth)
                    for (int q = 0; q < 2; q++)
                    {
                        int height1 = rnd.Next(1, 140);
                        int width2 = rnd.Next(0, maxWidth) + 1;
                        yield return (MakeRandomCase(rnd, height1, width1), MakeRandomCase(rnd, width1, width2));
                    }
            }
        for (int q = 0; q < 128; q++)
        {
            int height1 = rnd.Next(1, 140);
            int width1 = rnd.Next(0, maxWidth) + 1;
            int width2 = rnd.Next(0, maxWidth) + 1;
            yield return (MakeRandomCase(rnd, height1, width1), MakeRandomCase(rnd, width1, width2));
        }
    }

    public static IEnumerable<(BitMatrixCase, BitArrayCase)> RandomMultiplyVectorCases(int maxWidth)
    {
        var rnd = new Random(227);

        for (int i = 1; i < 12; i++)
            for (int j = -1; j <= 1; j++)
            {
                int width1 = 32 * i + j;
                if (width1 <= maxWidth)
                    for (int q = 0; q < 2; q++)
                    {
                        int height1 = rnd.Next(1, 140);
                        yield return (MakeRandomCase(rnd, height1, width1), BitArrayCase.MakeRandomCase(rnd, width1));
                    }
            }
        for (int q = 0; q < 128; q++)
        {
            int height1 = rnd.Next(1, 140);
            int width1 = rnd.Next(0, maxWidth) + 1;
            yield return (MakeRandomCase(rnd, height1, width1), BitArrayCase.MakeRandomCase(rnd, width1));
        }
    }


    public static IEnumerable<BitMatrixCase> RandomPowCasesFixedSize(int size)
    {
        var rnd = new Random(227);
        for (int q = 0; q < 16; q++)
            yield return MakeRandomCase(rnd, size, size);
    }

    public static IEnumerable<BitMatrixCase> RandomSquareCases()
    {
        var rnd = new Random(227);

        foreach (var size in new int[] {
            16,
            63,
            64,
            65,
            99,
            127,
            128,
            136,
        })
            for (int q = 0; q < 4; q++)
                yield return MakeRandomCase(rnd, size, size);
    }
}