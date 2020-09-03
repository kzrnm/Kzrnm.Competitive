using AtCoderProject;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.IO;
using System.Text;


[Config(typeof(ShortBenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class ConsoleReaderBenchSingle
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
    [BenchmarkCategory("Console")]
    public void ReadConsole()
    {
        stream.Position = 0;
        var tr = new StreamReader(stream);
        for (var i = 0; i < N; ++i)
        {
            _ = tr.ReadLine();
        }
    }


    [Benchmark]
    [BenchmarkCategory("Console")]
    public void ReadConsoleSplit()
    {
        stream.Position = 0;
        var tr = new StreamReader(stream);
        for (var i = 0; i < N; ++i)
        {
            _ = tr.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
        }
    }


    [Benchmark]
    public void ReadChar()
    {
        stream.Position = 0;
        for (var i = 0; i < N; ++i)
        {
            _ = cr.Char;
            _ = cr.Char;
            _ = cr.Char;
            _ = cr.Char;
            _ = cr.Char;
            _ = cr.Char;
        }
    }

    [Benchmark(Baseline = true)]
    public void ReadInt()
    {
        stream.Position = 0;
        for (var i = 0; i < N; ++i)
        {
            _ = cr.Int;
            _ = cr.Int;
            _ = cr.Int;
        }
    }
    [Benchmark]
    public void ReadLong()
    {
        stream.Position = 0;
        for (var i = 0; i < N; ++i)
        {
            _ = cr.Long;
            _ = cr.Long;
            _ = cr.Long;
        }
    }

    [Benchmark]
    public void ReadDouble()
    {
        stream.Position = 0;
        for (var i = 0; i < N; ++i)
        {
            _ = cr.Double;
            _ = cr.Double;
            _ = cr.Double;
        }
    }

    [Benchmark]
    public void ReadString()
    {
        stream.Position = 0;
        for (var i = 0; i < N; ++i)
        {
            _ = cr.String;
            _ = cr.String;
            _ = cr.String;
        }
    }

    [Benchmark]
    public void ReadAscii()
    {
        stream.Position = 0;
        for (var i = 0; i < N; ++i)
        {
            _ = cr.Ascii;
            _ = cr.Ascii;
            _ = cr.Ascii;
        }
    }

    [Benchmark]
    public void ReadLine()
    {
        stream.Position = 0;
        for (var i = 0; i < N; ++i)
        {
            _ = cr.Line;
        }
    }
}
