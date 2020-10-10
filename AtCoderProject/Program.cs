using AtCoder;
using AtCoder.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public partial class Program { static void Main() => new Program(new ConsoleReader(), new ConsoleWriter()).Run();[DebuggerBrowsable(DebuggerBrowsableState.Never)] public ConsoleReader cr;[DebuggerBrowsable(DebuggerBrowsableState.Never)] public ConsoleWriter cw; public Program(ConsoleReader r, ConsoleWriter w) { this.cr = r; this.cw = w; System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture; } }
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
        int n = cr;
        int q = cr;
        var fw = new LongFenwickTree(n);
        int[] a = cr.Split.Int;
        for (int i = 0; i < a.Length; i++)
        {
            fw.Add(i, a[i]);
        }

        for (int i = 0; i < q; i++)
        {
            if (cr.Int == 0)
            {
                fw.Add(cr, cr);
            }
            else
            {
                cw.WriteLine(fw.Sum(cr, cr));
            }
        }
        return null;
    }
}
struct Op : ISegtreeOperator<int>
{
    public int Identity => 0;
    public int Operate(int x, int y) => x + y;
}