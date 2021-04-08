using Kzrnm.Competitive.IO;
using System.Globalization;

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
        int Q = __ManyTestCases ? cr.Int : 1;
        for (; Q > 0; Q--)
        {
            var res = Calc();
            if (res is double d) cw.WriteLine(d.ToString("0.####################", CultureInfo.InvariantCulture));
            else if (res is bool b) cw.WriteLine(YesNo(b));
            else if (res != null) cw.WriteLine(res.ToString());
        }
        cw.Flush();
    }
}
[SourceExpander.NotEmbeddingSource]
partial class Program
{
    object Calc() => null;
    bool __ManyTestCases = false;
    string YesNo(bool b) => b ? "Yes" : "No";
}