using Kzrnm.Competitive;
using Kzrnm.Competitive.IO;
using System.Globalization;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

// @brief 起動クラス
// @docs Library/docs/_Runner/Program.md
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
    public static string YesNo(bool b) => b ? "Yes" : "No";
    public void Run()
    {
        int Q = __ManyTestCases ? cr.Int : 1;
        while (--Q >= 0)
            Calc(cr, cw);
        cw.Flush();
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
#pragma warning restore
}