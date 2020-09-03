using AtCoderProject;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.IO;
using System.Text;


[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class ConsoleReaderBenchSplit
{
    const int N = 500000;

    private ConsoleReader cr;
    private Stream stream;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var rnd = new Xorshift(42);
        var sb = new StringBuilder();
        for (int i = 0; i < N; i++)
        {
            sb.Append(rnd.Next());
            sb.Append(' ');
            sb.Append(rnd.Next());
            sb.Append(' ');
            sb.Append(rnd.Next());
            sb.AppendLine();
        }
        var input = new UTF8Encoding(false).GetBytes(sb.ToString());
        stream = new MemoryStream(input);
        cr = new ConsoleReader(stream, new UTF8Encoding(false));
    }

    [Benchmark(Baseline = true)]
    public void ReadInt()
    {
        stream.Position = 0;
        for (var i = 0; i < N; ++i)
            _ = cr.Split.Int;
    }
    [Benchmark]
    public void ReadLong()
    {
        stream.Position = 0;
        for (var i = 0; i < N; ++i)
            _ = cr.Split.Long;
    }

    [Benchmark]
    public void ReadDouble()
    {
        stream.Position = 0;
        for (var i = 0; i < N; ++i)
            _ = cr.Split.Double;
    }

    [Benchmark]
    public void ReadString()
    {
        stream.Position = 0;
        for (var i = 0; i < N; ++i)
            _ = cr.Split.String;
    }

    [Benchmark]
    public void ReadAscii()
    {
        stream.Position = 0;
        for (var i = 0; i < N; ++i)
            _ = cr.Split.Ascii;
    }
}
