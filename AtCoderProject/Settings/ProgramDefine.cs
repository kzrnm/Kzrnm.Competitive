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
}
