
using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Collection;

public class PriorityDeque2Tests
{
    [Fact]
    public void TryDequeue()
    {
        var pq = new PriorityDequeDictionary<int, (string A, char B)>();

        pq.Enqueue(1, ("A", 'E'));
        pq.Enqueue(5, ("B", 'F'));
        pq.Enqueue(4, ("C", 'G'));
        pq.Enqueue(2, ("D", 'H'));

        pq.Count.ShouldBe(4);
        pq.TryDequeueMin(out var key, out var a, out var b).ShouldBeTrue();
        key.ShouldBe(1); a.ShouldBe("A"); b.ShouldBe('E');
        pq.Count.ShouldBe(3);
        pq.TryDequeueMax(out key, out a, out b).ShouldBeTrue();
        key.ShouldBe(5); a.ShouldBe("B"); b.ShouldBe('F');
        pq.Count.ShouldBe(2);
        pq.TryDequeueMin(out key, out a, out b).ShouldBeTrue();
        key.ShouldBe(2); a.ShouldBe("D"); b.ShouldBe('H');
        pq.Count.ShouldBe(1);
        pq.TryDequeueMin(out key, out a, out b).ShouldBeTrue();
        key.ShouldBe(4); a.ShouldBe("C"); b.ShouldBe('G');

        pq.Count.ShouldBe(0);
        pq.TryDequeueMin(out _, out _, out _).ShouldBeFalse();
        pq.TryDequeueMax(out _, out _, out _).ShouldBeFalse();
    }

    [Fact]
    public void RandomNormal()
    {
        var rnd = new Random(227);
        var inner = new RandomInner(rnd);

        for (int i = 0; i < 200000; i++)
        {
            int t = rnd.Next(3);
            if (t == 0 || inner.Count == 0)
                inner.Enqueue();
            else if (t == 1)
                inner.DequeueMax();
            else
                inner.DequeueMin();
        }
    }

    [Fact]
    public void RandomEnqueueDequeue()
    {
        var rnd = new Random(227);
        var inner = new RandomInner(rnd);

        for (int i = 0; i < 200000; i++)
        {
            int t = rnd.Next(5);
            if (t == 0 || inner.Count == 0)
                inner.Enqueue();
            else if (t == 1)
                inner.DequeueMax();
            else if (t == 2)
                inner.DequeueMin();
            else if (t == 3)
                inner.EnqueueDequeueMin();
            else if (t == 4)
                inner.EnqueueDequeueMax();
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
        public void Enqueue()
        {
            var (key, (c, b)) = MakeNext();
            keyValues.Add((key, (c, b)));
            pq.Enqueue(key, (c, b));

            keyValues.Sort();
            keyValues.Reverse();

            pq.Count.ShouldBe(keyValues.Count);
            pq.PeekMin.ShouldBe(ToKeyValue(keyValues[0]));
            pq.PeekMax.ShouldBe(ToKeyValue(keyValues[^1]));
        }
        public void DequeueMin()
        {
            var (k, v) = pq.DequeueMin();

            (k, v).ShouldBe(keyValues[0]);
            keyValues.RemoveAt(0);
            keyValues.Sort();
            keyValues.Reverse();

            pq.Count.ShouldBe(keyValues.Count);
            if (Count > 0)
            {
                pq.PeekMin.ShouldBe(ToKeyValue(keyValues[0]));
                pq.PeekMax.ShouldBe(ToKeyValue(keyValues[^1]));
            }
        }
        public void DequeueMax()
        {
            var (k, v) = pq.DequeueMax();

            (k, v).ShouldBe(keyValues[^1]);
            keyValues.RemoveAt(keyValues.Count - 1);
            keyValues.Sort();
            keyValues.Reverse();

            pq.Count.ShouldBe(keyValues.Count);
            if (Count > 0)
            {
                pq.PeekMin.ShouldBe(ToKeyValue(keyValues[0]));
                pq.PeekMax.ShouldBe(ToKeyValue(keyValues[^1]));
            }
        }

        public void EnqueueDequeueMin()
        {
            var (key, tup) = MakeNext();
            var (k, v) = pq.EnqueueDequeueMin(key, tup);

            keyValues.Add((key, tup));
            keyValues.Sort();
            keyValues.Reverse();
            (k, v).ShouldBe(keyValues[0]);
            keyValues.RemoveAt(0);

            pq.Count.ShouldBe(keyValues.Count);
            pq.PeekMin.ShouldBe(ToKeyValue(keyValues[0]));
            pq.PeekMax.ShouldBe(ToKeyValue(keyValues[^1]));
        }
        public void EnqueueDequeueMax()
        {
            var (key, tup) = MakeNext();
            var (k, v) = pq.EnqueueDequeueMax(key, tup);

            keyValues.Add((key, tup));
            keyValues.Sort();
            keyValues.Reverse();
            (k, v).ShouldBe(keyValues[^1]);
            keyValues.RemoveAt(keyValues.Count - 1);

            pq.Count.ShouldBe(keyValues.Count);
            if (Count > 0)
            {
                pq.PeekMin.ShouldBe(ToKeyValue(keyValues[0]));
                pq.PeekMax.ShouldBe(ToKeyValue(keyValues[^1]));
            }
        }
    }
}
