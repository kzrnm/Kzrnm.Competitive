using AtCoder;
using Kzrnm.Competitive.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using static System.Math;
using static System.Numerics.BitOperations;
using static AtCoder.BitOperationsEx;
using static AtCoder.Global;
using static AtCoder.MathLibEx;
using static AtCoder.__BinarySearchEx;

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
        CalcImpl();
        return null;
    }
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private void CalcImpl()
    {
        const long BIG = (long)1e17;
        int n = cr;
        int m = cr;
        var mcf = new McfGraphLong(n + 2);
        long[] bs = cr.Repeat(n);
        (int s, int t, long l, long u, long c)[] inputEdges = cr.Repeat(m).Select<(int s, int t, long l, long u, long c)>(cr => (cr, cr, cr, cr, cr));

        foreach (var (s, t, l, u, c) in inputEdges)
        {
            mcf.AddEdge(s, t, l, c);
        }
        long bsum = 0;
        for (int i = 0; i < bs.Length; i++)
        {
            var b = bs[i];
            if (b > 0)
            {
                bsum += b;
                mcf.AddEdge(n, i, b, 0);
            }
            else if (b < 0)
            {
                mcf.AddEdge(i, n + 1, -b, 0);
            }
        }
        var (cap, cost) = mcf.Flow(n, n + 1);
        for (int i = 0; i < inputEdges.Length; i++)
        {
            mcf.GetEdge(i).Cap = inputEdges[i].u;
        }
        (cap, cost) = mcf.Flow(n, n + 1);
        if (cap < bsum)
        {
            cw.WriteLine("infeasible");
            return;
        }
        var edges = mcf.Edges().ToArray();
        foreach (var e in edges.AsSpan(m))
        {
            if (e.Flow < e.Cap)
            {
                cw.WriteLine("infeasible");
                return;
            }
        }
        for (int i = 0; i < m; i++)
        {

            if (edges[i].Flow < inputEdges[i].l)
            {
                cw.WriteLine("infeasible");
                return;
            }
        }
    }
}