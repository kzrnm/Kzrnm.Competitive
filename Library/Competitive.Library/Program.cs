using Kzrnm.Competitive;
using Kzrnm.Competitive.IO;
using System;
using System.Globalization;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;
internal partial class Program
{
    static void Main() => new Program(new PropertyConsoleReader(), new Utf8ConsoleWriter()).Run();
    public PropertyConsoleReader cr;
    public Utf8ConsoleWriter cw;
    public Program(PropertyConsoleReader r, Utf8ConsoleWriter w)
    {
        cr = r;
        ConsoleOutput.cw = cw = w;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
    }
    public void Run()
    {
        int Q = __ManyTestCases ? cr.Int : 1;
        for (; Q > 0; Q--)
            Calc(cr, cw);
        cw.Flush();
    }
}
namespace Kzrnm.Competitive
{
    public static class ConsoleOutputExt
    {
        public static ConsoleOutput ToConsoleOutput(this IUtf8ConsoleWriterFormatter f)
        {
            var cw = ConsoleOutput.cw;
            f.Write(cw);
            return cw.WriteLine();
        }
    }
    public struct ConsoleOutput
    {
        public static Utf8ConsoleWriter cw;
        public static implicit operator ConsoleOutput(int v) => cw.WriteLine(v);
        public static implicit operator ConsoleOutput(long v) => cw.WriteLine(v);
        public static implicit operator ConsoleOutput(uint v) => cw.WriteLine(v);
        public static implicit operator ConsoleOutput(ulong v) => cw.WriteLine(v);
        public static implicit operator ConsoleOutput(double v) => cw.WriteLine(v);
        public static implicit operator ConsoleOutput(decimal v) => cw.WriteLine(v);
        public static implicit operator ConsoleOutput(char v) => cw.WriteLine(v);
        public static implicit operator ConsoleOutput(char[] v) => cw.WriteLine((ReadOnlySpan<char>)v);
        public static implicit operator ConsoleOutput(string v) => cw.WriteLine((ReadOnlySpan<char>)v);
        public static implicit operator ConsoleOutput(bool v) => cw.WriteLine((ReadOnlySpan<char>)Program.YesNo(v));
        public static implicit operator ConsoleOutput(Utf8ConsoleWriter _) => default;
    }
}
#if !LIBRARY
[SourceExpander.NotEmbeddingSource]
#endif
partial class Program
{
#pragma warning disable
    [凾(256)]
    Kzrnm.Competitive.ConsoleOutput? Calc(PropertyConsoleReader cr, Utf8ConsoleWriter cw) => null;
    bool __ManyTestCases = false;
    public static string YesNo(bool b) => b ? "Yes" : "No";
#pragma warning restore
}