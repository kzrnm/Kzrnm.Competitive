namespace Kzrnm.Competitive.Testing.Collection;

public class PriorityDeque2Tests
{
    [Test, MultipleAssertions]
    public async Task TryDequeue()
    {
        var pq = new PriorityDequeDictionary<int, (string A, char B)>();

        pq.Enqueue(1, ("A", 'E'));
        pq.Enqueue(5, ("B", 'F'));
        pq.Enqueue(4, ("C", 'G'));
        pq.Enqueue(2, ("D", 'H'));

        await pq.Count.Should().BeEqualTo(4);
        await pq.TryDequeueMin(out var key, out var a, out var b).Should().BeTrue();
        await key.Should().BeEqualTo(1);
        await a.Should().BeEqualTo("A");
        await b.Should().BeEqualTo('E');
        await pq.Count.Should().BeEqualTo(3);
        await pq.TryDequeueMax(out key, out a, out b).Should().BeTrue();
        await key.Should().BeEqualTo(5);
        await a.Should().BeEqualTo("B");
        await b.Should().BeEqualTo('F');
        await pq.Count.Should().BeEqualTo(2);
        await pq.TryDequeueMin(out key, out a, out b).Should().BeTrue();
        await key.Should().BeEqualTo(2);
        await a.Should().BeEqualTo("D");
        await b.Should().BeEqualTo('H');
        await pq.Count.Should().BeEqualTo(1);
        await pq.TryDequeueMin(out key, out a, out b).Should().BeTrue();
        await key.Should().BeEqualTo(4);
        await a.Should().BeEqualTo("C");
        await b.Should().BeEqualTo('G');

        await pq.Count.Should().BeEqualTo(0);
        await pq.TryDequeueMin(out _, out _, out _).Should().BeFalse();
        await pq.TryDequeueMax(out _, out _, out _).Should().BeFalse();
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
        PriorityDequeDictionary<int, (char, byte), ReverseComparer<int>> pq = new();
        List<(int, (char, byte))> keyValues = new();

        public int Count => keyValues.Count;
        HashSet<int> usedKeys = new HashSet<int>();

        KeyValuePair<int, (char, byte)> ToKeyValue((int, (char, byte)) tup)
        {
            var (k, v) = tup;
            return KeyValuePair.Create(k, v);
        }
        (int, (char, byte)) MakeNext()
        {
            var key = rnd.Next();
            while (!usedKeys.Add(key))
            {
                key = rnd.Next();
            }
            var c = (char)rnd.Next(0x7f);
            var b = (byte)rnd.Next();
            return (key, (c, b));
        }
        public async Task Enqueue()
        {
            var (key, (c, b)) = MakeNext();
            keyValues.Add((key, (c, b)));
            pq.Enqueue(key, (c, b));

            keyValues.Sort();
            keyValues.Reverse();

            await pq.Count.Should().BeEqualTo(keyValues.Count);
            await pq.PeekMin.Should().BeEqualTo(ToKeyValue(keyValues[0]));
            await pq.PeekMax.Should().BeEqualTo(ToKeyValue(keyValues[^1]));
        }
        public async Task DequeueMin()
        {
            var (k, v) = pq.DequeueMin();

            await (k, v).Should().BeEqualTo(keyValues[0]);
            keyValues.RemoveAt(0);
            keyValues.Sort();
            keyValues.Reverse();

            await pq.Count.Should().BeEqualTo(keyValues.Count);
            if (Count > 0)
            {
                await pq.PeekMin.Should().BeEqualTo(ToKeyValue(keyValues[0]));
                await pq.PeekMax.Should().BeEqualTo(ToKeyValue(keyValues[^1]));
            }
        }
        public async Task DequeueMax()
        {
            var (k, v) = pq.DequeueMax();

            await (k, v).Should().BeEqualTo(keyValues[^1]);
            keyValues.RemoveAt(keyValues.Count - 1);
            keyValues.Sort();
            keyValues.Reverse();

            await pq.Count.Should().BeEqualTo(keyValues.Count);
            if (Count > 0)
            {
                await pq.PeekMin.Should().BeEqualTo(ToKeyValue(keyValues[0]));
                await pq.PeekMax.Should().BeEqualTo(ToKeyValue(keyValues[^1]));
            }
        }

        public async Task EnqueueDequeueMin()
        {
            var (key, tup) = MakeNext();
            var (k, v) = pq.EnqueueDequeueMin(key, tup);

            keyValues.Add((key, tup));
            keyValues.Sort();
            keyValues.Reverse();
            await (k, v).Should().BeEqualTo(keyValues[0]);
            keyValues.RemoveAt(0);

            await pq.Count.Should().BeEqualTo(keyValues.Count);
            await pq.PeekMin.Should().BeEqualTo(ToKeyValue(keyValues[0]));
            await pq.PeekMax.Should().BeEqualTo(ToKeyValue(keyValues[^1]));
        }
        public async Task EnqueueDequeueMax()
        {
            var (key, tup) = MakeNext();
            var (k, v) = pq.EnqueueDequeueMax(key, tup);

            keyValues.Add((key, tup));
            keyValues.Sort();
            keyValues.Reverse();
            await (k, v).Should().BeEqualTo(keyValues[^1]);
            keyValues.RemoveAt(keyValues.Count - 1);

            await pq.Count.Should().BeEqualTo(keyValues.Count);
            if (Count > 0)
            {
                await pq.PeekMin.Should().BeEqualTo(ToKeyValue(keyValues[0]));
                await pq.PeekMax.Should().BeEqualTo(ToKeyValue(keyValues[^1]));
            }
        }
    }
}