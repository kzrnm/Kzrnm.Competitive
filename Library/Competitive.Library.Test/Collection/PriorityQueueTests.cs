
namespace Kzrnm.Competitive.Testing.Collection
{
    public class PriorityQueueTest
    {
        [Fact]
        public void TryDequeue()
        {
            var pq = PriorityQueue.CreateDictionary<int, (string A, char B)>();

            pq.Enqueue(1, ("A", 'E'));
            pq.Enqueue(5, ("B", 'F'));
            pq.Enqueue(4, ("C", 'G'));
            pq.Enqueue(2, ("D", 'H'));

            pq.TryDequeue(out var key, out var a, out var b).ShouldBeTrue();
            key.ShouldBe(1); a.ShouldBe("A"); b.ShouldBe('E');
            pq.TryDequeue(out key, out a, out b).ShouldBeTrue();
            key.ShouldBe(2); a.ShouldBe("D"); b.ShouldBe('H');
            pq.TryDequeue(out key, out a, out b).ShouldBeTrue();
            key.ShouldBe(4); a.ShouldBe("C"); b.ShouldBe('G');
            pq.TryDequeue(out key, out a, out b).ShouldBeTrue();
            key.ShouldBe(5); a.ShouldBe("B"); b.ShouldBe('F');

            pq.TryDequeue(out _, out _, out _).ShouldBeFalse();
        }
    }
}
