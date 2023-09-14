using Kzrnm.Competitive.IO;
using System.Collections.Immutable;

namespace Kzrnm.Competitive.Collection
{
    internal abstract class PersistentQueueTests<T> : BaseSolver where T : IImmutableQueue<uint>
    {
        public abstract T GetEmpty();
        public override string Url => "https://judge.yosupo.jp/problem/persistent_queue";
        public override ConsoleOutput? Solve(ConsoleReader cr, Utf8ConsoleWriter cw)
        {
            int Q = cr;
            var S = new IImmutableQueue<uint>[Q + 1];
            S[^1] = GetEmpty();
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
    internal class BankersQueueTests : PersistentQueueTests<BankersQueue<uint>>
    {
        public override BankersQueue<uint> GetEmpty() => BankersQueue<uint>.Empty;
    }
    internal class RealTimeQueueTests : PersistentQueueTests<RealTimeQueue<uint>>
    {
        public override RealTimeQueue<uint> GetEmpty() => RealTimeQueue<uint>.Empty;
    }
}
