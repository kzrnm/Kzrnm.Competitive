
namespace Kzrnm.Competitive.Testing.Collection;

public class PriorityQueueTest
{
    [Test, MultipleAssertions]
    public async Task TryDequeue()
    {
        var pq = PriorityQueue.CreateDictionary<int, (string A, char B)>();

        pq.Enqueue(1, ("A", 'E'));
        pq.Enqueue(5, ("B", 'F'));
        pq.Enqueue(4, ("C", 'G'));
        pq.Enqueue(2, ("D", 'H'));

        await pq.TryDequeue(out var key, out var a, out var b).Should().BeTrue();
        await key.Should().BeEqualTo(1);
        await a.Should().BeEqualTo("A");
        await b.Should().BeEqualTo('E');
        await pq.TryDequeue(out key, out a, out b).Should().BeTrue();
        await key.Should().BeEqualTo(2);
        await a.Should().BeEqualTo("D");
        await b.Should().BeEqualTo('H');
        await pq.TryDequeue(out key, out a, out b).Should().BeTrue();
        await key.Should().BeEqualTo(4);
        await a.Should().BeEqualTo("C");
        await b.Should().BeEqualTo('G');
        await pq.TryDequeue(out key, out a, out b).Should().BeTrue();
        await key.Should().BeEqualTo(5);
        await a.Should().BeEqualTo("B");
        await b.Should().BeEqualTo('F');

        await pq.TryDequeue(out _, out _, out _).Should().BeFalse();
    }
}