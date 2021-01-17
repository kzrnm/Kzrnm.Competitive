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
    partial void Run();
}
