using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;


public partial class Program
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public PropertyConsoleReader cr;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        int Q = 1;
#pragma warning disable CS0162 // 到達できないコードが検出されました
        if (__ManyTestCases)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Error.WriteLine("many test cases mode");
            Console.ResetColor();
            Q = cr;
        }
#pragma warning restore CS0162 // 到達できないコードが検出されました
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
