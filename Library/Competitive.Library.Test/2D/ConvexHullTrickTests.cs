namespace Kzrnm.Competitive.Testing.TwoDimensional;

public class ConvexHullTrickTests
{
    [Test]
    public async Task Min()
    {
        var xs = new long[] { 0, 10, 12, 20, 29, 30, 40, 47, 50 };
        var cht = new LongMinConvexHullTrick(xs);
        var native = new MinConvexHullTrickNative();
        using (Assert.Multiple())
            for (int i = 0; i < xs.Length; i++)
                await cht.Query(i).Should().BeEqualTo(cht.YINF);

        cht.AddLine(2, 3);
        native.AddLine(2, 3);
        using (Assert.Multiple())
            for (int i = 0; i < xs.Length; i++)
                await cht.Query(i).Should().BeEqualTo(native.Min(xs[i]));

        cht.AddLine(-6, 300);
        native.AddLine(-6, 300);
        using (Assert.Multiple())
            for (int i = 0; i < xs.Length; i++)
                await cht.Query(i).Should().BeEqualTo(native.Min(xs[i]));

        cht.AddLine(1, 30);
        native.AddLine(1, 30);
        using (Assert.Multiple())
            for (int i = 0; i < xs.Length; i++)
                await cht.Query(i).Should().BeEqualTo(native.Min(xs[i]));

        cht.AddLine(1, 50);
        native.AddLine(1, 50);
        using (Assert.Multiple())
            for (int i = 0; i < xs.Length; i++)
                await cht.Query(i).Should().BeEqualTo(native.Min(xs[i]));

        cht.AddLine(1, 500);
        native.AddLine(1, 500);
        using (Assert.Multiple())
            for (int i = 0; i < xs.Length; i++)
                await cht.Query(i).Should().BeEqualTo(native.Min(xs[i]));

        cht.AddLine(0, 50);
        native.AddLine(0, 50);
        using (Assert.Multiple())
            for (int i = 0; i < xs.Length; i++)
                await cht.Query(i).Should().BeEqualTo(native.Min(xs[i]));
    }
    [Test]
    public async Task Max()
    {
        var xs = new long[] { 0, 10, 12, 20, 29, 30, 40, 47, 50 };
        var cht = new LongMaxConvexHullTrick(xs);
        var native = new MaxConvexHullTrickNative();
        using (Assert.Multiple())
            for (int i = 0; i < xs.Length; i++)
                await cht.Query(i).Should().BeEqualTo(cht.YINF);

        cht.AddLine(2, 3);
        native.AddLine(2, 3);
        using (Assert.Multiple())
            for (int i = 0; i < xs.Length; i++)
                await cht.Query(i).Should().BeEqualTo(native.Min(xs[i]));

        cht.AddLine(-6, 300);
        native.AddLine(-6, 300);
        using (Assert.Multiple())
            for (int i = 0; i < xs.Length; i++)
                await cht.Query(i).Should().BeEqualTo(native.Min(xs[i]));

        cht.AddLine(1, 30);
        native.AddLine(1, 30);
        using (Assert.Multiple())
            for (int i = 0; i < xs.Length; i++)
                await cht.Query(i).Should().BeEqualTo(native.Min(xs[i]));

        cht.AddLine(1, 50);
        native.AddLine(1, 50);
        using (Assert.Multiple())
            for (int i = 0; i < xs.Length; i++)
                await cht.Query(i).Should().BeEqualTo(native.Min(xs[i]));

        cht.AddLine(1, 500);
        native.AddLine(1, 500);
        using (Assert.Multiple())
            for (int i = 0; i < xs.Length; i++)
                await cht.Query(i).Should().BeEqualTo(native.Min(xs[i]));

        cht.AddLine(0, 50);
        native.AddLine(0, 50);
        using (Assert.Multiple())
            for (int i = 0; i < xs.Length; i++)
                await cht.Query(i).Should().BeEqualTo(native.Min(xs[i]));
    }

    private class MinConvexHullTrickNative
    {
        static long F(long a, long b, long x) => a * x + b;
        private readonly List<(long a, long b)> list = new();
        public void AddLine(long a, long b) => list.Add((a, b));
        public long Min(long x)
        {
            long min = 1000000000000000000;
            foreach (var (a, b) in list)
                min = Math.Min(min, F(a, b, x));
            return min;
        }
    }

    private class MaxConvexHullTrickNative
    {
        static long F(long a, long b, long x) => a * x + b;
        private readonly List<(long a, long b)> list = new();
        public void AddLine(long a, long b) => list.Add((a, b));
        public long Min(long x)
        {
            long min = -1000000000000000000;
            foreach (var (a, b) in list)
                min = Math.Max(min, F(a, b, x));
            return min;
        }
    }
}