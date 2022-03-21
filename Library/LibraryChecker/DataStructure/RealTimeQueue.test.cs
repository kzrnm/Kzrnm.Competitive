using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.DataStructure
{
    public class RealTimeQueueTest
    {
        static void Main() { using var cw = new Utf8ConsoleWriter(); Solve(new ConsoleReader(), cw); }
        // verification-helper: PROBLEM https://judge.yosupo.jp/problem/persistent_queue
        static ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int q = cr;
            var queues = new RealTimeQueue<int>[q];
            for (int i = 0; i < q; i++)
            {
                var type = cr.Int();
                var t = cr.Int();
                var prev = (uint)t < (uint)queues.Length ? queues[t] : RealTimeQueue<int>.Empty;
                if (type == 0)
                {
                    queues[i] = prev.Enqueue(cr.Int());
                }
                else
                {
                    queues[i] = prev.Dequeue(out var v);
                    cw.WriteLine(v);
                }
            }
            return null;
        }
    }
}

