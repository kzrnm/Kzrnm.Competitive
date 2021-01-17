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
    partial void Run();
    public void RunPublic() => Run();
}
