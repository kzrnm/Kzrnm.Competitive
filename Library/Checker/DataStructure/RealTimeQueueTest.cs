using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.DataStructure
{
    internal class RealTimeQueueTest : BaseSolver
    {
        public override string Url => "https://judge.yosupo.jp/problem/persistent_queue";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int Q = cr;
            var S = new RealTimeQueue<uint>[Q + 1];
            S[^1] = RealTimeQueue<uint>.Empty;
            for (int i = 0; i < Q; i++)
            {
                int ty = cr;
                int t = cr;
                if (ty == 0)
                {
                    uint x = cr;
                    S[i] = S.Get(t).Enqueue(x);
                }
                else
                {
                    S[i] = S.Get(t).Dequeue(out var v);
                    cw.WriteLine(v);
                }
            }
            return null;
        }
    }
}

