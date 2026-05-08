using System.Collections.Immutable;

namespace Kzrnm.Competitive.Testing.Algorithm;

public class DecreasingPointsTests
{
    class DecreasingPointsResultComparer<T, U>(IComparer<T> cmpX, IComparer<U> cmpY) : IComparer<DecreasingPoints.Result<T, U>>
    {
        public int Compare(DecreasingPoints.Result<T, U> a, DecreasingPoints.Result<T, U> b)
        {
            if (cmpX.Compare(a.X, b.X) is not 0 and var cx)
                return cx;
            if (cmpY.Compare(a.Y, b.Y) is not 0 and var cy)
                return cy;
            return a.Index.CompareTo(b.Index);
        }
    }
    public static IEnumerable<ImmutableArray<(double, int)>> RandomPointCases()
    {
        var rnd = new Random(227);
        yield return Inner(rnd, 1).ToImmutableArray();
        for (int q = 0; q < 16; q++)
        {
            yield return Inner(rnd, size: rnd.Next(3, 8)).ToImmutableArray();
            yield return Inner(rnd, size: rnd.Next(50, 100)).ToImmutableArray();
        }

        static IEnumerable<(double, int)> Inner(Random random, int size)
        {
            for (int i = 0; i < size; i++)
            {
                yield return (random.NextDouble(), random.Next(50));
            }
        }
    }

    [Test]
    [MethodDataSource(nameof(RandomPointCases))]
    public async Task Strict(ImmutableArray<(double, int)> input)
    {
        var points = input.ToArray();
        var expected = Naive(points, true);
        await DecreasingPoints.Points(points).Should().BeStrictlyEquivalentTo(expected);
    }

    [Test]
    [MethodDataSource(nameof(RandomPointCases))]
    public async Task NotStrict(ImmutableArray<(double, int)> input)
    {
        var points = input.ToArray();
        var expected = Naive(points, false);
        await DecreasingPoints.Points(points, strict: false).Should().BeStrictlyEquivalentTo(expected);
    }

    [Test]
    [MethodDataSource(nameof(RandomPointCases))]
    public async Task StrictComparer(ImmutableArray<(double, int)> input)
    {
        var points = input.ToArray();
        var expected = Naive(points, true, ReverseComparerClass<double>.Default, ReverseComparerClass<int>.Default);
        await DecreasingPoints.Points(points, ReverseComparerClass<double>.Default, ReverseComparerClass<int>.Default).Should().BeStrictlyEquivalentTo(expected);
    }

    [Test]
    [MethodDataSource(nameof(RandomPointCases))]
    public async Task NotStrictComparer(ImmutableArray<(double, int)> input)
    {
        var points = input.ToArray();
        var expected = Naive(points, false, ReverseComparerClass<double>.Default, ReverseComparerClass<int>.Default);
        await DecreasingPoints.Points(points, ReverseComparerClass<double>.Default, ReverseComparerClass<int>.Default, strict: false).Should().BeStrictlyEquivalentTo(expected);
    }

    internal static DecreasingPoints.Result<T, U>[] Naive<T, U>((T X, U Y)[] points, bool strict, IComparer<T> cmpX = null, IComparer<U> cmpY = null)
    {
        if (points.Length == 0) return [];
        cmpX ??= Comparer<T>.Default;
        cmpY ??= Comparer<U>.Default;

        var rt = new List<DecreasingPoints.Result<T, U>>();
        for (int i = 0; i < points.Length; i++)
        {
            var (X, Y) = points[i];
            for (int j = 0; j < points.Length; j++)
            {
                var (otherX, otherY) = points[j];
                if (i == j) continue;
                switch (cmpX.Compare(X, otherX), cmpY.Compare(Y, otherY))
                {
                    case (0, 0): throw new InvalidOperationException(); // 同じ点が来るパターンは考慮しないことにする

                    case ( <= 0, < 0):
                    case ( < 0, 0) when strict:
                        goto FIN;
                }
            }
            rt.Add(new(X, Y, i));
        FIN:;
        }

        rt.Sort(new DecreasingPointsResultComparer<T, U>(cmpX, cmpY));
        return rt.ToArray();
    }
}