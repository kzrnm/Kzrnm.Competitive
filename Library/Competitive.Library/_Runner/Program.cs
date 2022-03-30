using Kzrnm.Competitive;
using Kzrnm.Competitive.IO;
using System.Globalization;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;
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
        int Q = __ManyTestCases ? cr.Int : 1;
        for (; Q > 0; Q--)
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
    public static string YesNo(bool b) => b ? "Yes" : "No";
#pragma warning restore
}
// @brief 起動クラス
// @docs Library/docs/_Runner/Program.md
