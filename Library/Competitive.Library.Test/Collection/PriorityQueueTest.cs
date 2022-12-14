
namespace Kzrnm.Competitive.Testing.Collection
{
    public class PriorityQueueTest
    {
        [Fact]
        public void TryDequeue()
        {
            var pq = PriorityQueue.CreateDictionary<int, (string A, string B)>();

            pq.Enqueue(1, ("A", "E"));
            pq.Enqueue(5, ("B", "F"));
            pq.Enqueue(4, ("C", "G"));
            pq.Enqueue(2, ("D", "H"));

            pq.TryDequeue(out var key, out var a, out var b).Should().BeTrue();
            key.Should().Be(1); a.Should().Be("A"); b.Should().Be("E");
            pq.TryDequeue(out key, out a, out b).Should().BeTrue();
            key.Should().Be(2); a.Should().Be("D"); b.Should().Be("H");
            pq.TryDequeue(out key, out a, out b).Should().BeTrue();
            key.Should().Be(4); a.Should().Be("C"); b.Should().Be("G");
            pq.TryDequeue(out key, out a, out b).Should().BeTrue();
            key.Should().Be(5); a.Should().Be("B"); b.Should().Be("F");

            pq.TryDequeue(out _, out _, out _).Should().BeFalse();
        }
    }
}
