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
        int Q = 1;
#pragma warning disable CS0162 // 到達できないコードが検出されました
        if (__ManyTestCases) Q = cr;
#pragma warning restore CS0162 // 到達できないコードが検出されました
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
