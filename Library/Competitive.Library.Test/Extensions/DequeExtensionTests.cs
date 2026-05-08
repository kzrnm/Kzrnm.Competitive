using AtCoder;

namespace Kzrnm.Competitive.Testing.Extensions;

public class DequeExtensionTests
{
    public static IEnumerable<int> Lengths => Enumerable.Range(0, 18);
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Lengths))]
    public async Task ToDeque(int length)
    {
        var orig = Enumerable.Range(1, length).ToArray();
        await Impl(orig.ToDeque());
        await Impl(orig.AsSpan().ToDeque());
        await Impl(orig.AsEnumerable().ToDeque());

        async Task Impl(Deque<int> deque)
        {
            await deque.Should().HaveCount(length);
            for (int i = 0; i < orig.Length; i++)
            {
                await deque[i].Should().BeEqualTo(orig[i]);
            }
            await deque.Should().BeStrictlyEquivalentTo(orig);
        }
    }
    [Test, MultipleAssertions]
    public async Task ToDequeEmpty()
    {
        var orig = new int[0];
        await Impl(orig.ToDeque());
        await Impl(orig.AsSpan().ToDeque());
        await Impl(orig.AsEnumerable().ToDeque());

        static async Task Impl(Deque<int> deque)
        {
            await deque.Should().HaveCount(0);
            await deque.Should().BeEmpty();
            await deque.Should().BeStrictlyEquivalentTo(new int[0]);
        }
    }

    [Test, MultipleAssertions]
    public async Task RemoveFirst()
    {
        for (int i = 0; i <= 7; i++)
        {
            var deque = new Deque<int> { 1, 2, 3, 4, 5, 6, 7, };
            deque.RemoveFirst(i);
            await deque.Should().BeStrictlyEquivalentTo(Enumerable.Range(i + 1, 7 - i));
        }
    }

    [Test, MultipleAssertions]
    public async Task RemoveLast()
    {
        for (int i = 0; i <= 7; i++)
        {
            var deque = new Deque<int> { 1, 2, 3, 4, 5, 6, 7, };
            deque.RemoveLast(i);
            await deque.Should().BeStrictlyEquivalentTo(Enumerable.Range(1, 7 - i));
        }
    }
}