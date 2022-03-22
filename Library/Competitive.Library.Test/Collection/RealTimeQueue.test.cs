using FluentAssertions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace Kzrnm.Competitive.Testing.Collection
{
    [Verify] // verification-helper: PROBLEM https://judge.yosupo.jp/problem/aplusb
    public class RealTimeQueueTests
    {
        [Fact]
        public void Simple()
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
                    v.Should().Be(i);
                }
            }
            for (int i = 0; i + 1 < N; i++)
            {
                queues[i][i].Should().BeSameAs(RealTimeQueue<int>.Empty);
                for (int j = i + 1; j < N; j++)
                {
                    queues[i][j].Should().Equal(Enumerable.Range(i, j - i));
                }
            }
        }
    }
}
