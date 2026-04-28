namespace Kzrnm.Competitive.Testing.Collection;

public class SetTests
{
    [Test, MultipleAssertions]
    public async Task InitSingleSet()
    {
        for (int i = 0; i < 64; i++)
            await new Set<int>(Enumerable.Range(0, i).Reverse().Concat(Enumerable.Range(0, i)))
                 .Should().BeEquivalentOrderTo(Enumerable.Range(0, i));
    }

    [Test, MultipleAssertions]
    public async Task InitMultiSet()
    {
        for (int i = 0; i < 64; i++)
            await new Set<int>(Enumerable.Range(0, i).Reverse().Concat(Enumerable.Range(0, i)), true)
                 .Should().BeEquivalentOrderTo(Enumerable.Range(0, i).SelectMany(n => new[] { n, n }));
    }

    [Test, MultipleAssertions]
    public async Task Set()
    {
        var set = new Set<int>([6, 7, 8, 1, 2, 3, 4, 5, 1, 2, 3]);
        set.Add(9);
        set.Add(5);
        await set.Should().BeEquivalentOrderTo((int[])[1, 2, 3, 4, 5, 6, 7, 8, 9]);
        await set.Should().HaveCount(9);
        set.Remove(5);
        await set.Should().HaveCount(8);
        await set.Should().BeEquivalentOrderTo((int[])[1, 2, 3, 4, 6, 7, 8, 9]);
        await set.FindByIndex(8).Should().BeNull();
        await set.FindByIndex(7).Value.Should().BeEqualTo(9);
        await set.FindNode(5).Should().BeNull();

        await set.FindNodeLowerBound(4).Value.Should().BeEqualTo(4);
        await set.FindNodeUpperBound(4).Value.Should().BeEqualTo(6);
        await set.FindNodeReverseUpperBound(4).Value.Should().BeEqualTo(3);
        await set.FindNodeReverseLowerBound(4).Value.Should().BeEqualTo(4);
        await set.FindNodeLowerBound(5).Value.Should().BeEqualTo(6);
        await set.FindNodeUpperBound(5).Value.Should().BeEqualTo(6);
        await set.FindNodeReverseUpperBound(5).Value.Should().BeEqualTo(4);
        await set.FindNodeReverseUpperBound(5).Value.Should().BeEqualTo(4);

        int v;
        await set.TryGetLowerBound(4, out v).Should().BeTrue();
        await v.Should().BeEqualTo(4);
        await set.TryGetUpperBound(4, out v).Should().BeTrue();
        await v.Should().BeEqualTo(6);
        await set.TryGetReverseLowerBound(4, out v).Should().BeTrue();
        await v.Should().BeEqualTo(4);
        await set.TryGetReverseUpperBound(4, out v).Should().BeTrue();
        await v.Should().BeEqualTo(3);
        await set.TryGetLowerBound(5, out v).Should().BeTrue();
        await v.Should().BeEqualTo(6);
        await set.TryGetUpperBound(5, out v).Should().BeTrue();
        await v.Should().BeEqualTo(6);
        await set.TryGetReverseLowerBound(5, out v).Should().BeTrue();
        await v.Should().BeEqualTo(4);
        await set.TryGetReverseUpperBound(5, out v).Should().BeTrue();
        await v.Should().BeEqualTo(4);

        await set.LowerBoundIndex(4).Should().BeEqualTo(3);
        await set.UpperBoundIndex(4).Should().BeEqualTo(4);
        await set.TryGetReverseLowerBound(4, out v).Should().BeTrue();
        await v.Should().BeEqualTo(4);
        await set.TryGetReverseUpperBound(4, out v).Should().BeTrue();
        await v.Should().BeEqualTo(3);
        await set.LowerBoundIndex(5).Should().BeEqualTo(4);
        await set.UpperBoundIndex(5).Should().BeEqualTo(4);
        await set.TryGetReverseLowerBound(5, out v).Should().BeTrue();
        await v.Should().BeEqualTo(4);
        await set.TryGetReverseUpperBound(5, out v).Should().BeTrue();
        await v.Should().BeEqualTo(4);

        await set.TryGetLowerBound(9, out _).Should().BeTrue();
        await set.TryGetLowerBound(10, out _).Should().BeFalse();
        await set.TryGetUpperBound(8, out _).Should().BeTrue();
        await set.TryGetUpperBound(9, out _).Should().BeFalse();

        await set.TryGetReverseLowerBound(1, out _).Should().BeTrue();
        await set.TryGetReverseLowerBound(0, out _).Should().BeFalse();
        await set.TryGetReverseUpperBound(2, out _).Should().BeTrue();
        await set.TryGetReverseUpperBound(1, out _).Should().BeFalse();

        await set.FindNodeLowerBound(10).Should().BeNull();
        await set.FindNodeUpperBound(10).Should().BeNull();
        await set.FindNodeReverseLowerBound(0).Should().BeNull();
        await set.FindNodeReverseUpperBound(1).Should().BeNull();

        set.RemoveNode(set.FindNodeLowerBound(5));
        await set.Should().BeEquivalentOrderTo((int[])[1, 2, 3, 4, 7, 8, 9]);

        await set.Reversed().Should().BeEquivalentOrderTo([9, 8, 7, 4, 3, 2, 1]);
        await set.EnumerateItem().Should().BeEquivalentOrderTo([1, 2, 3, 4, 7, 8, 9]);
        await set.EnumerateItem(set.FindNodeLowerBound(5)).Should().BeEquivalentOrderTo([7, 8, 9]);
        await set.EnumerateItem(set.FindNodeLowerBound(5), true).Should().BeEquivalentOrderTo([7, 4, 3, 2, 1]);

        set.RemoveNode(set.FindNodeLowerBound(0));
        await set.Should().BeEquivalentOrderTo((int[])[2, 3, 4, 7, 8, 9]);

        set.RemoveNode(set.FindNodeLowerBound(9));
        await set.Should().BeEquivalentOrderTo((int[])[2, 3, 4, 7, 8]);
    }
    [Test, MultipleAssertions]
    public async Task MultiSet()
    {
        var set = new Set<int>([6, 7, 8, 1, 2, 3, 4, 5, 1, 2, 3], true);
        set.Add(9);
        set.Add(5);
        await set.Should().BeEquivalentOrderTo((int[])[1, 1, 2, 2, 3, 3, 4, 5, 5, 6, 7, 8, 9]);
        await set.Should().HaveCount(13);
        set.Remove(5);
        await set.Should().HaveCount(12);
        await set.Should().BeEquivalentOrderTo((int[])[1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9]);
        await set.FindByIndex(12).Should().BeNull();
        await set.FindByIndex(11).Value.Should().BeEqualTo(9);
        await set.FindNode(5).Should().NotBeNull();

        await set.Reversed().Should().BeEquivalentOrderTo([9, 8, 7, 6, 5, 4, 3, 3, 2, 2, 1, 1]);
        await set.EnumerateItem().Should().BeEquivalentOrderTo([1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9]);
        await set.EnumerateItem(set.FindNodeLowerBound(6)).Should().BeEquivalentOrderTo([6, 7, 8, 9]);
        await set.EnumerateItem(set.FindNodeLowerBound(6), true).Should().BeEquivalentOrderTo([6, 5, 4, 3, 3, 2, 2, 1, 1]);

        await set.FindNodeLowerBound(3).Value.Should().BeEqualTo(3);
        await set.FindNodeUpperBound(3).Value.Should().BeEqualTo(4);
        await set.FindNodeReverseLowerBound(3).Value.Should().BeEqualTo(3);
        await set.FindNodeReverseUpperBound(3).Value.Should().BeEqualTo(2);

        int v;
        await set.TryGetLowerBound(3, out v).Should().BeTrue();
        await v.Should().BeEqualTo(3);
        await set.TryGetUpperBound(3, out v).Should().BeTrue();
        await v.Should().BeEqualTo(4);
        await set.TryGetReverseLowerBound(3, out v).Should().BeTrue();
        await v.Should().BeEqualTo(3);
        await set.TryGetReverseUpperBound(3, out v).Should().BeTrue();
        await v.Should().BeEqualTo(2);

        await set.TryGetLowerBound(9, out _).Should().BeTrue();
        await set.TryGetLowerBound(10, out _).Should().BeFalse();
        await set.TryGetUpperBound(8, out _).Should().BeTrue();
        await set.TryGetUpperBound(9, out _).Should().BeFalse();

        await set.TryGetReverseLowerBound(1, out _).Should().BeTrue();
        await set.TryGetReverseLowerBound(0, out _).Should().BeFalse();
        await set.TryGetReverseUpperBound(2, out _).Should().BeTrue();
        await set.TryGetReverseUpperBound(1, out _).Should().BeFalse();

        await set.LowerBoundIndex(3).Should().BeEqualTo(4);
        await set.UpperBoundIndex(3).Should().BeEqualTo(6);
        await set.ReverseLowerBoundIndex(3).Should().BeEqualTo(5);
        await set.ReverseUpperBoundIndex(3).Should().BeEqualTo(3);

        await set.FindNodeLowerBound(10).Should().BeNull();
        await set.FindNodeUpperBound(10).Should().BeNull();
        await set.FindNodeReverseLowerBound(0).Should().BeNull();
        await set.FindNodeReverseUpperBound(1).Should().BeNull();
    }
    [Test, MultipleAssertions]
    public async Task ReverseComparer()
    {
        var set = new Set<int, ReverseComparer<int>>([6, 7, 8, 1, 2, 3, 4, 5, 1, 2, 3]);
        set.Add(9);
        set.Add(5);
        await set.Should().BeEquivalentOrderTo((int[])[9, 8, 7, 6, 5, 4, 3, 2, 1]);
        await set.Should().HaveCount(9);
        set.Remove(5);
        await set.Should().HaveCount(8);
        await set.Should().BeEquivalentOrderTo((int[])[9, 8, 7, 6, 4, 3, 2, 1]);
        await set.FindByIndex(8).Should().BeNull();
        await set.FindByIndex(7).Value.Should().BeEqualTo(1);
        await set.FindNode(5).Should().BeNull();

        await set.FindNodeLowerBound(6).Value.Should().BeEqualTo(6);
        await set.FindNodeUpperBound(6).Value.Should().BeEqualTo(4);
        await set.FindNodeLowerBound(5).Value.Should().BeEqualTo(4);
        await set.FindNodeUpperBound(5).Value.Should().BeEqualTo(4);

        int v;
        await set.TryGetLowerBound(6, out v).Should().BeTrue();
        await v.Should().BeEqualTo(6);
        await set.TryGetUpperBound(6, out v).Should().BeTrue();
        await v.Should().BeEqualTo(4);
        await set.TryGetLowerBound(5, out v).Should().BeTrue();
        await v.Should().BeEqualTo(4);
        await set.TryGetUpperBound(5, out v).Should().BeTrue();
        await v.Should().BeEqualTo(4);

        await set.TryGetLowerBound(1, out _).Should().BeTrue();
        await set.TryGetLowerBound(0, out _).Should().BeFalse();
        await set.TryGetUpperBound(2, out _).Should().BeTrue();
        await set.TryGetUpperBound(1, out _).Should().BeFalse();

        await set.TryGetReverseLowerBound(9, out _).Should().BeTrue();
        await set.TryGetReverseLowerBound(10, out _).Should().BeFalse();
        await set.TryGetReverseUpperBound(8, out _).Should().BeTrue();
        await set.TryGetReverseUpperBound(9, out _).Should().BeFalse();

        await set.LowerBoundIndex(6).Should().BeEqualTo(3);
        await set.UpperBoundIndex(6).Should().BeEqualTo(4);
        await set.LowerBoundIndex(5).Should().BeEqualTo(4);
        await set.UpperBoundIndex(5).Should().BeEqualTo(4);

        await set.FindNodeLowerBound(0).Should().BeNull();
        await set.FindNodeUpperBound(0).Should().BeNull();
    }

    [Test, MultipleAssertions]
    public async Task FindByIndex()
    {
        for (int count = 0; count < 64; count++)
        {
            IList<int> arr = Enumerable.Range(0, count).ToArray();
            var set = new Set<int>(arr);
            for (int i = 0; i < count; i++)
            {
                await set.FindByIndex(i).Value.Should().BeEqualTo(i);
            }
        }
    }

    [Test, MultipleAssertions]
    public async Task Enumerate()
    {
        for (int count = 0; count < 64; count++)
        {
            IList<int> arr = Enumerable.Range(0, count).ToArray();
            var set = new Set<int>(arr);
            await set.Reversed().Should().BeEquivalentOrderTo(arr.Reverse());
            await set.EnumerateItem().Should().BeEquivalentOrderTo(arr);
            await set.EnumerateItem(reverse: true).Should().BeEquivalentOrderTo(arr.Reverse());

            for (int i = 0; i < count; i++)
            {
                await set.EnumerateItem(set.FindByIndex(i)).Should().BeEquivalentOrderTo(arr.Skip(i));
                await set.EnumerateItem(set.FindByIndex(i), true)
                    .Should().BeEquivalentOrderTo(arr.Take(i + 1).Reverse());
            }
        }
    }

    [Test, MultipleAssertions]
    public async Task EnumerateMulti()
    {
        var arr = new[] { 1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9 };
        var set = new Set<int>(arr, true);
        await set.Reversed().Should().BeEquivalentOrderTo([9, 8, 7, 6, 5, 4, 3, 3, 2, 2, 1, 1]);
        await set.EnumerateItem().Should().BeEquivalentOrderTo([1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9]);
        await set.EnumerateItem(reverse: true).Should().BeEquivalentOrderTo([9, 8, 7, 6, 5, 4, 3, 3, 2, 2, 1, 1]);

        for (int i = 0; i < arr.Length; i++)
        {
            await set.EnumerateItem(set.FindByIndex(i)).Should().BeEquivalentOrderTo(arr.Skip(i));
            await set.EnumerateItem(set.FindByIndex(i), true)
                .Should().BeEquivalentOrderTo(arr.Take(i + 1).Reverse());
        }
    }
}