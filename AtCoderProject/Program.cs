using AtCoder;
using AtCoder.GraphAcl;
using AtCoder.IO;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Program
{
    [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)] public ConsoleReader cr;
    [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)] public ConsoleWriter cw;
    public Program(ConsoleReader reader, ConsoleWriter writer) { this.cr = reader; this.cw = writer; System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture; }
    static void Main() => new Program(new ConsoleReader(), new ConsoleWriter()).Run();
    public void Run()
    {
        AtCoder.Expander.Expand(expandMethod: AtCoder.ExpandMethod.Strict);
        var res = Calc();
        if (res is double)
            cw.WriteLine(Result((double)res));
        else if (res is bool)
            cw.WriteLine(Result((bool)res));
        else if (res != null)
            cw.WriteLine(res.ToString());
        cw.Flush();
    }
}
public partial class Program
{
    public static string Result(double d) => d.ToString("0.####################", System.Globalization.CultureInfo.InvariantCulture);
    public static string Result(bool b) => b ? "Yes" : "No";
    private object Calc()
    {
        new DSU(1);
        return null;
    }
    int N;
    int Q;
    int[] Query;
}
