using Kzrnm.Competitive;
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
    public Utf8ConsoleWriter cw;
    public Program(PropertyConsoleReader r, Utf8ConsoleWriter w)
    {
        cr = r;
        ConsoleOutput.cw = cw = w;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
    }
    public void Run()
    {
        int Q = 1;
#pragma warning disable IDE0079
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
            Calc(cr, cw);
        cw.Flush();
    }
}
