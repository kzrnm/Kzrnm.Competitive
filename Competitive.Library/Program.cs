using Kzrnm.Competitive.IO;
using System.Globalization;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;
internal partial class Program
{
    static void Main() => new Program(new PropertyConsoleReader(), new ConsoleWriter()).Run();
    public PropertyConsoleReader cr;
    public ConsoleWriter cw;
    public Program(PropertyConsoleReader r, ConsoleWriter w)
    {
        this.cr = r;
        this.cw = w;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
    }
    public void Run()
    {
        var sw = cw.StreamWriter;
        int Q = __ManyTestCases ? cr.Int : 1;
        for (; Q > 0; Q--)
        {
            var res = Calc();
            if (res is double d) sw.WriteLine(d.ToString("0.####################", CultureInfo.InvariantCulture));
            else if (res is bool b) sw.WriteLine(YesNo(b));
            else if (res is char[] chrs) sw.WriteLine(chrs);
            else if (res != null && res != cw) sw.WriteLine(res.ToString());
        }
        cw.Flush();
    }
}
#if !LIBRARY
[SourceExpander.NotEmbeddingSource]
#endif
partial class Program
{
    [凾(256)]
    object Calc() => null;
    bool __ManyTestCases = false;
    string YesNo(bool b) => b ? "Yes" : "No";
}