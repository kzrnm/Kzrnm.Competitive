using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Solvers.DataStructure
{
    public class RealTimeQueueSolver : ISolver
    {
        public string Name => "persistent_queue";

        public void Solve(ConsoleReader cr, ConsoleWriter cw)
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
        }
    }
}

