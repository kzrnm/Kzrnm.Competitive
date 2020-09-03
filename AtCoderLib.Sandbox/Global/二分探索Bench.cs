using AtCoderProject;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;




[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class 二分探索Bench
{
    const int N = 100000;
    int[] arr;
    int[] rev;
    IComparer<int> revCmp = ExComparer<int>.DefaultReverse;
    public 二分探索Bench()
    {
        arr = Util.MakeIntArray(N);
        Array.Sort(arr);
        rev = (int[])arr.Clone();
        Array.Reverse(rev);
    }

    [Benchmark]
    [BenchmarkCategory("IList<T>")]
    public void LowerBound()
    {
        for (int i = 0; i < N; i++)
            arr.LowerBound(arr[i]);
    }

    [Benchmark]
    [BenchmarkCategory("IList<T>")]
    public void UpperBound()
    {
        for (int i = 0; i < N; i++)
            arr.UpperBound(arr[i]);
    }

    [Benchmark]
    [BenchmarkCategory("IList<T>")]
    public void LowerBoundCmp()
    {
        for (int i = 0; i < N; i++)
            rev.LowerBound(arr[i], revCmp);
    }

    [Benchmark]
    [BenchmarkCategory("IList<T>")]
    public void UpperBoundCmp()
    {
        for (int i = 0; i < N; i++)
            rev.UpperBound(arr[i], revCmp);
    }


    [Benchmark]
    [BenchmarkCategory("Span<T>")]
    public void SpanLowerBound()
    {
        for (int i = 0; i < N; i++)
            arr.AsSpan().LowerBound(arr[i]);
    }

    [Benchmark]
    [BenchmarkCategory("Span<T>")]
    public void SpanUpperBound()
    {
        for (int i = 0; i < N; i++)
            arr.AsSpan().UpperBound(arr[i]);
    }

    [Benchmark]
    [BenchmarkCategory("Span<T>")]
    public void SpanLowerBoundCmp()
    {
        for (int i = 0; i < N; i++)
            rev.AsSpan().LowerBound(arr[i], revCmp);
    }

    [Benchmark]
    [BenchmarkCategory("Span<T>")]
    public void SpanUpperBoundCmp()
    {
        for (int i = 0; i < N; i++)
            rev.AsSpan().UpperBound(arr[i], revCmp);
    }

    [Benchmark]
    [BenchmarkCategory("ReadOnlySpan<T>")]
    public void RSpanLowerBound()
    {
        for (int i = 0; i < N; i++)
            ((ReadOnlySpan<int>)arr).LowerBound(arr[i]);
    }

    [Benchmark]
    [BenchmarkCategory("ReadOnlySpan<T>")]
    public void RSpanUpperBound()
    {
        for (int i = 0; i < N; i++)
            ((ReadOnlySpan<int>)arr).UpperBound(arr[i]);
    }

    [Benchmark]
    [BenchmarkCategory("ReadOnlySpan<T>")]
    public void RSpanLowerBoundCmp()
    {
        for (int i = 0; i < N; i++)
            ((ReadOnlySpan<int>)rev).LowerBound(arr[i], revCmp);
    }

    [Benchmark]
    [BenchmarkCategory("ReadOnlySpan<T>")]
    public void RSpanUpperBoundCmp()
    {
        for (int i = 0; i < N; i++)
            ((ReadOnlySpan<int>)rev).UpperBound(arr[i], revCmp);
    }
}