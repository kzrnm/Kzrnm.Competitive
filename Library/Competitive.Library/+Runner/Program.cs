using Kzrnm.Competitive;
using Kzrnm.Competitive.IO;
using System.Globalization;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

[module: System.Runtime.CompilerServices.SkipLocalsInit]
internal partial class Program
{
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
        int Q = __ManyTestCases ? cr : 1;
        while (--Q >= 0)
            Calc();
        cw.Flush();
    }
}
[SourceExpander.NotEmbeddingSource]
partial class Program
{
#pragma warning disable
    [凾(256)]
    Kzrnm.Competitive.ConsoleOutput? Calc() => null;
    bool __ManyTestCases = false;
#pragma warning restore
}