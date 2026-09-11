namespace Kzrnm.Competitive.Testing.Extensions;

public class LinkedListExtensionTests
{
    [Test]
    public async Task EnumerateNodes()
    {
        await new LinkedList<int>().EnumerateNodes().Should().BeEmpty();

        var nodes = new LinkedList<int>([1, 2, 3]).EnumerateNodes();
        await nodes.Should().HaveCount(3);
        await nodes.Select(n => n.Value).Should().BeStrictlyEquivalentTo([1, 2, 3]);
    }
}