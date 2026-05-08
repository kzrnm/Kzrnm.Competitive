namespace Kzrnm.Competitive.Testing.Collection;

public class RealTimeQueueTests
{
    [Test, MultipleAssertions]
    public async Task Simple()
    {
        const int N = 100;
        var queues = new RealTimeQueue<int>[N][];
        for (int i = 0; i < queues.Length; i++)
            queues[0] = new RealTimeQueue<int>[N];
        queues[0][0] = RealTimeQueue<int>.Empty;
        for (int i = 0; i + 1 < N; i++)
        {
            queues[0][i + 1] = queues[0][i].Enqueue(i);
        }
        for (int i = 0; i + 1 < N; i++)
        {
            queues[i + 1] = new RealTimeQueue<int>[N];
            for (int j = i + 1; j < N; j++)
            {
                queues[i + 1][j] = queues[i][j].Dequeue(out var v);
                await v.Should().BeEqualTo(i);
            }
        }
        for (int i = 0; i + 1 < N; i++)
        {
            await queues[i][i].Should().BeSameReferenceAs(RealTimeQueue<int>.Empty);
            for (int j = i + 1; j < N; j++)
            {
                await queues[i][j].Should().BeStrictlyEquivalentTo(Enumerable.Range(i, j - i));
            }
        }
    }
}