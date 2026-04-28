namespace Kzrnm.Competitive.Testing.Collection;

public class PriorityDequeTests
{
    [Test, MultipleAssertions]
    public async Task TryDequeue()
    {
        var pq = new PriorityDeque<int>();

        pq.Enqueue(1);
        pq.Enqueue(5);
        pq.Enqueue(4);
        pq.Enqueue(2);

        await pq.Count.Should().BeEqualTo(4);
        await pq.TryDequeueMin(out var data).Should().BeTrue();
        await data.Should().BeEqualTo(1);
        await pq.Count.Should().BeEqualTo(3);
        await pq.TryDequeueMax(out data).Should().BeTrue();
        await data.Should().BeEqualTo(5);
        await pq.Count.Should().BeEqualTo(2);
        await pq.TryDequeueMin(out data).Should().BeTrue();
        await data.Should().BeEqualTo(2);
        await pq.Count.Should().BeEqualTo(1);
        await pq.TryDequeueMin(out data).Should().BeTrue();
        await data.Should().BeEqualTo(4);

        await pq.Count.Should().BeEqualTo(0);
        await pq.TryDequeueMin(out _).Should().BeFalse();
        await pq.TryDequeueMax(out _).Should().BeFalse();
    }

    [Test, MultipleAssertions]
    public async Task RandomNormal()
    {
        var rnd = new Random(227);
        var inner = new RandomInner(rnd);

        for (int i = 0; i < 200000; i++)
        {
            int t = rnd.Next(3);
            if (t == 0 || inner.Count == 0)
                await inner.Enqueue();
            else if (t == 1)
                await inner.DequeueMax();
            else
                await inner.DequeueMin();
        }
    }

    [Test, MultipleAssertions]
    public async Task RandomEnqueueDequeue()
    {
        var rnd = new Random(227);
        var inner = new RandomInner(rnd);

        for (int i = 0; i < 200000; i++)
        {
            int t = rnd.Next(5);
            if (t == 0 || inner.Count == 0)
                await inner.Enqueue();
            else if (t == 1)
                await inner.DequeueMax();
            else if (t == 2)
                await inner.DequeueMin();
            else if (t == 3)
                await inner.EnqueueDequeueMin();
            else if (t == 4)
                await inner.EnqueueDequeueMax();
        }
    }
    class RandomInner(Random rnd)
    {
        PriorityDeque<int, ReverseComparer<int>> pq = new();
        List<int> keyValues = new();

        public int Count => keyValues.Count;
        HashSet<int> usedKeys = new HashSet<int>();

        int MakeNext()
        {
            var key = rnd.Next();
            while (!usedKeys.Add(key))
            {
                key = rnd.Next();
            }
            return key;
        }
        public async Task Enqueue()
        {
            var key = MakeNext();
            keyValues.Add(key);
            pq.Enqueue(key);

            keyValues.Sort();
            keyValues.Reverse();

            await pq.Count.Should().BeEqualTo(keyValues.Count);
            await pq.PeekMin.Should().BeEqualTo(keyValues[0]);
            await pq.PeekMax.Should().BeEqualTo(keyValues[^1]);
        }
        public async Task DequeueMin()
        {
            var k = pq.DequeueMin();

            await k.Should().BeEqualTo(keyValues[0]);
            keyValues.RemoveAt(0);
            keyValues.Sort();
            keyValues.Reverse();

            await pq.Count.Should().BeEqualTo(keyValues.Count);
            if (Count > 0)
            {
                await pq.PeekMin.Should().BeEqualTo(keyValues[0]);
                await pq.PeekMax.Should().BeEqualTo(keyValues[^1]);
            }
        }
        public async Task DequeueMax()
        {
            var k = pq.DequeueMax();

            await k.Should().BeEqualTo(keyValues[^1]);
            keyValues.RemoveAt(keyValues.Count - 1);
            keyValues.Sort();
            keyValues.Reverse();

            await pq.Count.Should().BeEqualTo(keyValues.Count);
            if (Count > 0)
            {
                await pq.PeekMin.Should().BeEqualTo(keyValues[0]);
                await pq.PeekMax.Should().BeEqualTo(keyValues[^1]);
            }
        }

        public async Task EnqueueDequeueMin()
        {
            var inKey = MakeNext();
            var k = pq.EnqueueDequeueMin(inKey);

            keyValues.Add(inKey);
            keyValues.Sort();
            keyValues.Reverse();
            await k.Should().BeEqualTo(keyValues[0]);
            keyValues.RemoveAt(0);

            await pq.Count.Should().BeEqualTo(keyValues.Count);
            await pq.PeekMin.Should().BeEqualTo(keyValues[0]);
            await pq.PeekMax.Should().BeEqualTo(keyValues[^1]);
        }
        public async Task EnqueueDequeueMax()
        {
            var inKey = MakeNext();
            var k = pq.EnqueueDequeueMax(inKey);

            keyValues.Add(inKey);
            keyValues.Sort();
            keyValues.Reverse();
            await k.Should().BeEqualTo(keyValues[^1]);
            keyValues.RemoveAt(keyValues.Count - 1);

            await pq.Count.Should().BeEqualTo(keyValues.Count);
            if (Count > 0)
            {
                await pq.PeekMin.Should().BeEqualTo(keyValues[0]);
                await pq.PeekMax.Should().BeEqualTo(keyValues[^1]);
            }
        }
    }
}