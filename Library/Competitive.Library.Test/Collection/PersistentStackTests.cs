namespace Kzrnm.Competitive.Testing.Collection;

public class PersistentStackTests
{
    [Test, MultipleAssertions]
    public async Task Stack()
    {
        var ss = new PersistentStack<int>[100];
        ss[0] = PersistentStack<int>.Empty;
        await ss[0].Should().BeEmpty();
        for (int i = 1; i < ss.Length; i++)
        {
            ss[i] = ss[i - 1].Push(i);
            await ss[i].Should().BeEquivalentOrderTo(Enumerable.Range(1, i).Reverse());
            await ss[i].Count.Should().BeEqualTo(i);
        }

        for (int i = ss.Length - 1; i >= 0; i--)
        {
            await ss[i].Should().BeEquivalentOrderTo(Enumerable.Range(1, i).Reverse());
            await ss[i].Count.Should().BeEqualTo(i);

            if (i > 0)
            {
                var other1 = ss[i].Pop();
                await other1.Should().BeEquivalentOrderTo(Enumerable.Range(1, i - 1).Reverse());
                await other1.Count.Should().BeEqualTo(i - 1);
            }

            var add2 = ss[i].Push(i).Push(i);
            await add2.Count.Should().BeEqualTo(i + 2);
            await add2.Should().BeEquivalentOrderTo(Enumerable.Range(1, i).Append(i).Append(i).OrderBy(i => i).Reverse());
        }

        await ss[50].Push(-1).Push(-3).Should().BeEquivalentOrderTo(new[] { -3, -1 }.Concat(Enumerable.Range(1, 50).Reverse()));
    }
    [Test, MultipleAssertions]
    public async Task Clear()
    {
        var s = PersistentStack<int>.Empty;
        await s.Should().BeEmpty();
        for (int i = 0; i < 100; i++)
        {
            await s.Clear().Should().BeSameReferenceAs(PersistentStack<int>.Empty);
            s = s.Push(i);
            await s.Clear().Should().BeSameReferenceAs(PersistentStack<int>.Empty);
        }
    }
}