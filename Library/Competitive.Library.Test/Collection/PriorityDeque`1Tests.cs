
using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Collection
{
    public class PriorityDequeTests
    {
        [Fact]
        public void TryDequeue()
        {
            var pq = new PriorityDeque<int>();

            pq.Enqueue(1);
            pq.Enqueue(5);
            pq.Enqueue(4);
            pq.Enqueue(2);

            pq.Count.ShouldBe(4);
            pq.TryDequeueMin(out var data).ShouldBeTrue();
            data.ShouldBe(1);
            pq.Count.ShouldBe(3);
            pq.TryDequeueMax(out data).ShouldBeTrue();
            data.ShouldBe(5);
            pq.Count.ShouldBe(2);
            pq.TryDequeueMin(out data).ShouldBeTrue();
            data.ShouldBe(2);
            pq.Count.ShouldBe(1);
            pq.TryDequeueMin(out data).ShouldBeTrue();
            data.ShouldBe(4);

            pq.Count.ShouldBe(0);
            pq.TryDequeueMin(out _).ShouldBeFalse();
            pq.TryDequeueMax(out _).ShouldBeFalse();
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
            public void Enqueue()
            {
                var key = MakeNext();
                keyValues.Add(key);
                pq.Enqueue(key);

                keyValues.Sort();
                keyValues.Reverse();

                pq.Count.ShouldBe(keyValues.Count);
                pq.PeekMin.ShouldBe(keyValues[0]);
                pq.PeekMax.ShouldBe(keyValues[^1]);
            }
            public void DequeueMin()
            {
                var k = pq.DequeueMin();

                k.ShouldBe(keyValues[0]);
                keyValues.RemoveAt(0);
                keyValues.Sort();
                keyValues.Reverse();

                pq.Count.ShouldBe(keyValues.Count);
                if (Count > 0)
                {
                    pq.PeekMin.ShouldBe(keyValues[0]);
                    pq.PeekMax.ShouldBe(keyValues[^1]);
                }
            }
            public void DequeueMax()
            {
                var k = pq.DequeueMax();

                k.ShouldBe(keyValues[^1]);
                keyValues.RemoveAt(keyValues.Count - 1);
                keyValues.Sort();
                keyValues.Reverse();

                pq.Count.ShouldBe(keyValues.Count);
                if (Count > 0)
                {
                    pq.PeekMin.ShouldBe(keyValues[0]);
                    pq.PeekMax.ShouldBe(keyValues[^1]);
                }
            }

            public void EnqueueDequeueMin()
            {
                var inKey = MakeNext();
                var k = pq.EnqueueDequeueMin(inKey);

                keyValues.Add(inKey);
                keyValues.Sort();
                keyValues.Reverse();
                k.ShouldBe(keyValues[0]);
                keyValues.RemoveAt(0);

                pq.Count.ShouldBe(keyValues.Count);
                pq.PeekMin.ShouldBe(keyValues[0]);
                pq.PeekMax.ShouldBe(keyValues[^1]);
            }
            public void EnqueueDequeueMax()
            {
                var inKey = MakeNext();
                var k = pq.EnqueueDequeueMax(inKey);

                keyValues.Add(inKey);
                keyValues.Sort();
                keyValues.Reverse();
                k.ShouldBe(keyValues[^1]);
                keyValues.RemoveAt(keyValues.Count - 1);

                pq.Count.ShouldBe(keyValues.Count);
                if (Count > 0)
                {
                    pq.PeekMin.ShouldBe(keyValues[0]);
                    pq.PeekMax.ShouldBe(keyValues[^1]);
                }
            }
        }
    }
}
