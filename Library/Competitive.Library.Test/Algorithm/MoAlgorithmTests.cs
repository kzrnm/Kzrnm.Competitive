using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Algorithm;

public class MoAlgorithmTests
{
    [Fact]
    public void Solve()
    {
        const int N = 100000;
        var rnd = new Random(227);
        var array = Enumerable.Repeat(rnd, N).Select(r => (long)r.Next()).ToArray();
        var fw = new LongFenwickTree(N);
        for (int i = 0; i < N; i++) fw.Add(i, array[i]);

        var mo = new MoAlgorithm();

        var expected = new long[N];
        for (int i = 0; i < N; i++)
        {
            int l = rnd.Next(N);
            int r = rnd.Next(l + 1, N + 1);
            mo.AddQuery(l, r);
            expected[i] = fw[l..r];
        }
        mo.Solve<long, St>(new St(array)).ShouldBe(expected);

        long current1 = 0;
        mo.Solve(i => current1 += array[i], i => current1 -= array[i],
            update: i => current1.ShouldBe(expected[i]));

        long current2 = 0;
        mo.SolveStrict(
            i => current2 += array[i], i => current2 += array[i],
            i => current2 -= array[i], i => current2 -= array[i],
            update: i => current2.ShouldBe(expected[i]));
    }
}
class St(long[] array) : IMoAlgorithmState<long>
{
    public long Current { get; private set; }

    public void Add(int idx) => Current += array[idx];
    public void Remove(int idx) => Current -= array[idx];
}
