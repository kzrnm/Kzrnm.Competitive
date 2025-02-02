using System;
using System.Collections.Generic;
using System.Linq;

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
    public static IEnumerable<TheoryDataRow<SerializableTuple<double, int>[]>> RandomPointCases()
    {
        var rnd = new Random(227);
        yield return Inner(rnd, 1).ToArray();
        for (int q = 0; q < 16; q++)
        {
            yield return Inner(rnd, size: rnd.Next(3, 8)).ToArray();
            yield return Inner(rnd, size: rnd.Next(50, 100)).ToArray();
        }

        static IEnumerable<SerializableTuple<double, int>> Inner(Random random, int size)
        {
            for (int i = 0; i < size; i++)
            {
                yield return new SerializableTuple<double, int>(random.NextDouble(), random.Next(50));
            }
        }
    }

    [Theory]
    [MemberData(nameof(RandomPointCases), DisableDiscoveryEnumeration = true)]
    public void Strict(SerializableTuple<double, int>[] input)
    {
        var points = input.Select(t => t.ToTuple()).ToArray();
        var expected = Naive(points, true);
        DecreasingPoints.Points(points).ShouldBe(expected);
    }

    [Theory]
    [MemberData(nameof(RandomPointCases), DisableDiscoveryEnumeration = true)]
    public void NotStrict(SerializableTuple<double, int>[] input)
    {
        var points = input.Select(t => t.ToTuple()).ToArray();
        var expected = Naive(points, false);
        DecreasingPoints.Points(points, strict: false).ShouldBe(expected);
    }

    [Theory]
    [MemberData(nameof(RandomPointCases), DisableDiscoveryEnumeration = true)]
    public void StrictComparer(SerializableTuple<double, int>[] input)
    {
        var points = input.Select(t => t.ToTuple()).ToArray();
        var expected = Naive(points, true, ReverseComparerClass<double>.Default, ReverseComparerClass<int>.Default);
        DecreasingPoints.Points(points, ReverseComparerClass<double>.Default, ReverseComparerClass<int>.Default).ShouldBe(expected);
    }

    [Theory]
    [MemberData(nameof(RandomPointCases), DisableDiscoveryEnumeration = true)]
    public void NotStrictComparer(SerializableTuple<double, int>[] input)
    {
        var points = input.Select(t => t.ToTuple()).ToArray();
        var expected = Naive(points, false, ReverseComparerClass<double>.Default, ReverseComparerClass<int>.Default);
        DecreasingPoints.Points(points,
            ReverseComparerClass<double>.Default, ReverseComparerClass<int>.Default,
            strict: false).ShouldBe(expected);
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
