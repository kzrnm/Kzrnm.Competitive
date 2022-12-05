using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.DataStructure
{
    internal class RealTimeQueueTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/persistent_queue";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
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

