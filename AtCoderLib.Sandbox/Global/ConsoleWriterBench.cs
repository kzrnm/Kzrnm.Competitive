using AtCoderProject;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class ConsoleWriterBench
{
    class DummyStream : MemoryStream
    {
        public override void Write(byte[] buffer, int offset, int count)
        {
            var b = this.GetBuffer();
            for (int i = 0; i < Math.Min(b.Length, count); i++)
            {
                b[i] = buffer[i + offset];
            }
        }
    }
    const int N = 500000;

    private ConsoleWriter cw;
    private MemoryStream stream;


    private string[] arr1;
    private int[] arr2;
    private int[][] grid;

    [GlobalSetup]
    public void GlobalSetup()
    {
        stream = new DummyStream();
        cw = new ConsoleWriter(stream, new UTF8Encoding(false));
        arr1 = new[] { "123456789", "123456789", "123456789", "123456789" };
        arr2 = Util.MakeIntArray(N);
        grid = arr2.Select((n, i) => (n, i)).GroupBy(t => t.i % 1000).Select(g => g.Select(t => t.n).ToArray()).ToArray();
    }


    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Single")]
    public void WriteString()
    {
        stream.Position = 0;
        for (int i = 0; i < N; i++)
            cw.WriteLine("123456789");
    }

    [Benchmark]
    [BenchmarkCategory("Single")]
    public void WriteSpan()
    {
        stream.Position = 0;
        for (int i = 0; i < N; i++)
            cw.WriteLine("123456789".AsSpan());
    }


    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Join")]
    public void WriteJoinDirect()
    {
        stream.Position = 0;
        for (int i = 0; i < N; i++)
            cw.WriteLineJoin("123456789", "123456789", "123456789", "123456789");
    }
    [Benchmark]
    [BenchmarkCategory("Join")]
    public void WriteJoinArray()
    {
        stream.Position = 0;
        for (int i = 0; i < N; i++)
            cw.WriteLineJoin(arr1);
    }

    [Benchmark]
    [BenchmarkCategory("Join")]
    public void WriteJoinSpan()
    {
        stream.Position = 0;
        for (int i = 0; i < N; i++)
            cw.WriteLineJoin((ReadOnlySpan<string>)arr1);
    }



    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Many")]
    public void WriteJoinManyArray()
    {
        stream.Position = 0;
        cw.WriteLineJoin(arr2);
    }
    [Benchmark]
    [BenchmarkCategory("Many")]
    public void WriteJoinManySpan()
    {
        stream.Position = 0;
        cw.WriteLineJoin((ReadOnlySpan<int>)arr2);
    }
    [Benchmark]
    [BenchmarkCategory("Many")]
    public void WriteLinesSpan()
    {
        stream.Position = 0;
        cw.WriteLines((ReadOnlySpan<int>)arr2);
    }
    [Benchmark]
    [BenchmarkCategory("Many")]
    public void WriteLinesEnumerable()
    {
        stream.Position = 0;
        cw.WriteLines(arr2);
    }
    [Benchmark]
    [BenchmarkCategory("Many")]
    public void WriteLineGrid()
    {
        stream.Position = 0;
        cw.WriteLineGrid(grid);
    }
}
