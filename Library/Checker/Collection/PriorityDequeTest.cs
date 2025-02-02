using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Collection;

internal class PriorityDequeTest : BaseSolver
{
    public override string Url => "https://judge.yosupo.jp/problem/double_ended_priority_queue";
    public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
    {
        int N = cr;
        int Q = cr;
        var pq = new PriorityDeque<int>(N + Q);
        while (--N >= 0) pq.Enqueue(cr);
        while (--Q >= 0)
        {
            int t = cr;
            if (t == 0)
                pq.Enqueue(cr);
            else if (t == 1)
                cw.WriteLine(pq.DequeueMin());
            else
                cw.WriteLine(pq.DequeueMax());
        }
        return null;
    }
}
