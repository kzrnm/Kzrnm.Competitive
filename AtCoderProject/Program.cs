using AtCoder;
using AtCoder.Graph;
using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using static AtCoder.Global;
using static AtCoder.BinarySearchEx;

public partial class Program { static void Main() => new Program(new PropertyConsoleReader(), new ConsoleWriter()).Run();[DebuggerBrowsable(DebuggerBrowsableState.Never)] public PropertyConsoleReader cr;[DebuggerBrowsable(DebuggerBrowsableState.Never)] public ConsoleWriter cw; public Program(PropertyConsoleReader r, ConsoleWriter w) { this.cr = r; this.cw = w; System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture; } }
public partial class Program
{
    public void Run()
    {
        var res = Calc();
        if (res is double) cw.WriteLine(((double)res).ToString("0.####################", System.Globalization.CultureInfo.InvariantCulture));
        else if (res is bool) cw.WriteLine(((bool)res) ? "Yes" : "No");
        else if (res != null) cw.WriteLine(res.ToString());
        cw.Flush();
    }
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private object Calc()
    {
        int N = cr;

        return N;
    }
}