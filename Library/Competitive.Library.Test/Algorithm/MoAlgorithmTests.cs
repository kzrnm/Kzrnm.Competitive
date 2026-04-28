namespace Kzrnm.Competitive.Testing.Algorithm;

public class MoAlgorithmTests
{
    [Test, MultipleAssertions]
    public async Task Solve()
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
        await mo.Solve<long, St>(new St(array)).Should().BeEquivalentOrderTo(expected);

        var results = new long[N];
        results.AsSpan().Fill(long.MinValue);
        long current1 = 0;
        mo.Solve(i => current1 += array[i], i => current1 -= array[i],
            update: i => results[i] = current1);
        await results.Should().BeEquivalentOrderTo(expected);

        long current2 = 0;
        results.AsSpan().Fill(long.MinValue);
        mo.SolveStrict(
            i => current2 += array[i], i => current2 += array[i],
            i => current2 -= array[i], i => current2 -= array[i],
            update: i => results[i] = current2);
        await results.Should().BeEquivalentOrderTo(expected);
    }
}
class St(long[] array) : IMoAlgorithmState<long>
{
    public long Current { get; private set; }

    public void Add(int idx) => Current += array[idx];
    public void Remove(int idx) => Current -= array[idx];
}