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
        int Q = 1;
        if (__ManyTestCases()) Q = cr;
        for (; Q > 0; Q--)
        {
            var res = Calc();
            if (res is double d) cw.WriteLine(d.ToString("0.####################", CultureInfo.InvariantCulture));
            else if (res is bool b) WriteBool(b);
            else if (res != null) cw.WriteLine(res.ToString());
        }
        cw.Flush();
    }
    partial void WriteBool(bool b);
#pragma warning disable IDE1006,IDE0060
    object Calc(Program dum = null) => dum;
    bool __ManyTestCases(Program dum = null) => false;
}
