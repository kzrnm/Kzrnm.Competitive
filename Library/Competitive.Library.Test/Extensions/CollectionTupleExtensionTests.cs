namespace Kzrnm.Competitive.Testing.Extensions;

public class CollectionTupleExtensionTests
{
    [Test, MultipleAssertions]
    public async Task StackTryPop2()
    {
        var collection = new Stack<(string, int)>();
        await collection.TryPop(out _, out _).Should().BeFalse();

        collection.Push(("foo", 227));
        await collection.TryPop(out var v1, out var v2).Should().BeTrue();
        await v1.Should().BeEqualTo("foo");
        await v2.Should().BeEqualTo(227);
    }
    [Test, MultipleAssertions]
    public async Task StackTryPeek2()
    {
        var collection = new Stack<(string, int)>();
        await collection.TryPeek(out _, out _).Should().BeFalse();

        collection.Push(("foo", 227));
        await collection.TryPeek(out var v1, out var v2).Should().BeTrue();
        await v1.Should().BeEqualTo("foo");
        await v2.Should().BeEqualTo(227);
    }
    [Test, MultipleAssertions]
    public async Task StackTryPop3()
    {
        var collection = new Stack<(string, int, DateTime)>();
        await collection.TryPop(out _, out _, out _).Should().BeFalse();

        collection.Push(("foo", 227, new DateTime(2022, 2, 22)));
        await collection.TryPop(out var v1, out var v2, out var v3).Should().BeTrue();
        await v1.Should().BeEqualTo("foo");
        await v2.Should().BeEqualTo(227);
        await v3.Should().BeEqualTo(new DateTime(2022, 2, 22));
    }
    [Test, MultipleAssertions]
    public async Task StackTryPeek3()
    {
        var collection = new Stack<(string, int, DateTime)>();
        await collection.TryPeek(out _, out _, out _).Should().BeFalse();

        collection.Push(("foo", 227, new DateTime(2022, 2, 22)));
        await collection.TryPeek(out var v1, out var v2, out var v3).Should().BeTrue();
        await v1.Should().BeEqualTo("foo");
        await v2.Should().BeEqualTo(227);
        await v3.Should().BeEqualTo(new DateTime(2022, 2, 22));
    }

    [Test, MultipleAssertions]
    public async Task QueueTryDequeue2()
    {
        var collection = new Queue<(string, int)>();
        await collection.TryDequeue(out _, out _).Should().BeFalse();

        collection.Enqueue(("foo", 227));
        await collection.TryDequeue(out var v1, out var v2).Should().BeTrue();
        await v1.Should().BeEqualTo("foo");
        await v2.Should().BeEqualTo(227);
    }
    [Test, MultipleAssertions]
    public async Task QueueTryPeek2()
    {
        var collection = new Queue<(string, int)>();
        await collection.TryPeek(out _, out _).Should().BeFalse();

        collection.Enqueue(("foo", 227));
        await collection.TryPeek(out var v1, out var v2).Should().BeTrue();
        await v1.Should().BeEqualTo("foo");
        await v2.Should().BeEqualTo(227);
    }
    [Test, MultipleAssertions]
    public async Task QueueTryDequeue3()
    {
        var collection = new Queue<(string, int, DateTime)>();
        await collection.TryDequeue(out _, out _, out _).Should().BeFalse();

        collection.Enqueue(("foo", 227, new DateTime(2022, 2, 22)));
        await collection.TryDequeue(out var v1, out var v2, out var v3).Should().BeTrue();
        await v1.Should().BeEqualTo("foo");
        await v2.Should().BeEqualTo(227);
        await v3.Should().BeEqualTo(new DateTime(2022, 2, 22));
    }
    [Test, MultipleAssertions]
    public async Task QueueTryPeek3()
    {
        var collection = new Queue<(string, int, DateTime)>();
        await collection.TryPeek(out _, out _, out _).Should().BeFalse();

        collection.Enqueue(("foo", 227, new DateTime(2022, 2, 22)));
        await collection.TryPeek(out var v1, out var v2, out var v3).Should().BeTrue();
        await v1.Should().BeEqualTo("foo");
        await v2.Should().BeEqualTo(227);
        await v3.Should().BeEqualTo(new DateTime(2022, 2, 22));
    }
}