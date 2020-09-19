using AtCoder;
using AtCoder.GraphAcl;
using AtCoder.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class Program
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)] public ConsoleReader cr;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)] public ConsoleWriter cw;
    public Program(ConsoleReader reader, ConsoleWriter writer) { this.cr = reader; this.cw = writer; System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture; }
    static void Main() => new Program(new ConsoleReader(), new ConsoleWriter()).Run();
    public void Run()
    {
        var res = Calc();
        if (res is double) cw.WriteLine(((double)res).ToString("0.####################", System.Globalization.CultureInfo.InvariantCulture));
        else if (res is bool) cw.WriteLine(((bool)res) ? "Yes" : "No");
        else if (res != null) cw.WriteLine(res.ToString());
        cw.Flush();
    }
}
public partial class Program
{
    private object Calc()
    {
        int N = cr;

        return N;
    }
}
