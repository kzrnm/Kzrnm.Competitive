namespace Kzrnm.Competitive.Testing.Collection;

[NotInParallel(nameof(Set<>))]
public class SetTests
{
    [Test, MultipleAssertions]
    public async Task InitSingleSet()
    {
        for (int i = 0; i < 64; i++)
            await new Set<int>(Enumerable.Range(0, i).Reverse().Concat(Enumerable.Range(0, i)))
                 .Should().BeStrictlyEquivalentTo(Enumerable.Range(0, i));
    }

    [Test, MultipleAssertions]
    public async Task InitMultiSet()
    {
        for (int i = 0; i < 64; i++)
            await new Set<int>(Enumerable.Range(0, i).Reverse().Concat(Enumerable.Range(0, i)), true)
                 .Should().BeStrictlyEquivalentTo(Enumerable.Range(0, i).SelectMany(n => new[] { n, n }));
    }

    [Test]
    public async Task Random()
    {
        var rnd = new Random(227);
        var set = new Set<int>();
        var ss = new SortedSet<int>();
        for (int i = 0; i < 10000; i++)
        {
            var r = rnd.Next(i < 5000 ? 10 : 20);

            if (r < 8)
            {
                ss.Add(i);
                set.Add(i);
            }
            else
            {
                r = rnd.Next(i);
                ss.Remove(r);
                set.Remove(r);
            }
        }
        await set.Should().BeStrictlyEquivalentTo(ss);
    }

    [Test]
    public async Task Set()
    {
        var set = new Set<int>([6, 7, 8, 1, 2, 3, 4, 5, 1, 2, 3]);
        set.Add(9);
        set.Add(5);
        await set.Should().BeStrictlyEquivalentTo((int[])[1, 2, 3, 4, 5, 6, 7, 8, 9]);
        await set.Should().HaveCount(9);
        set.Remove(5);
        await set.Should().HaveCount(8);
        await set.Should().BeStrictlyEquivalentTo((int[])[1, 2, 3, 4, 6, 7, 8, 9]);
        await set.FindByIndex(8).Index.Should().BeEqualTo(-1);
        await set.FindByIndex(7).Node.Value.Should().BeEqualTo(9);
        await set.FindNode(5).Should().BeNull();

        await set.FindNodeLowerBound(4).Node.Value.Should().BeEqualTo(4);
        await set.FindNodeUpperBound(4).Node.Value.Should().BeEqualTo(6);
        await set.FindNodeReverseUpperBound(4).Node.Value.Should().BeEqualTo(3);
        await set.FindNodeReverseLowerBound(4).Node.Value.Should().BeEqualTo(4);
        await set.FindNodeLowerBound(5).Node.Value.Should().BeEqualTo(6);
        await set.FindNodeUpperBound(5).Node.Value.Should().BeEqualTo(6);
        await set.FindNodeReverseUpperBound(5).Node.Value.Should().BeEqualTo(4);
        await set.FindNodeReverseUpperBound(5).Node.Value.Should().BeEqualTo(4);

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

        await set.FindNodeLowerBound(10).NodeRef.Should().BeEqualTo(-1);
        await set.FindNodeUpperBound(10).NodeRef.Should().BeEqualTo(-1);
        await set.FindNodeReverseLowerBound(0).NodeRef.Should().BeEqualTo(-1);
        await set.FindNodeReverseUpperBound(1).NodeRef.Should().BeEqualTo(-1);

        set.Remove(set.FindNodeLowerBound(5));
        await set.Should().BeStrictlyEquivalentTo((int[])[1, 2, 3, 4, 7, 8, 9]);

        await set.Reversed().Should().BeStrictlyEquivalentTo([9, 8, 7, 4, 3, 2, 1]);
        await set.EnumerateNode().Select(n => n.Node.Value).Should().BeStrictlyEquivalentTo([1, 2, 3, 4, 7, 8, 9]);
        await set.EnumerateNodeUpper(5).Select(n => n.Node.Value).Should().BeStrictlyEquivalentTo([7, 8, 9]);
        await set.EnumerateNodeLower(5).Select(n => n.Node.Value).Should().BeStrictlyEquivalentTo([4, 3, 2, 1]);

        set.Remove(set.FindNodeLowerBound(0));
        await set.Should().BeStrictlyEquivalentTo((int[])[2, 3, 4, 7, 8, 9]);

        set.Remove(set.FindNodeLowerBound(9));
        await set.Should().BeStrictlyEquivalentTo((int[])[2, 3, 4, 7, 8]);

        await set.LowerBoundIndex(3).Should().BeEqualTo(1);
        await set.UpperBoundIndex(3).Should().BeEqualTo(2);

        await set.LowerBoundIndex(8).Should().BeEqualTo(4);
        await set.UpperBoundIndex(8).Should().BeEqualTo(5);

        await set.ReverseLowerBoundIndex(3).Should().BeEqualTo(1);
        await set.ReverseUpperBoundIndex(3).Should().BeEqualTo(0);

        await set.ReverseLowerBoundIndex(2).Should().BeEqualTo(0);
        await set.ReverseUpperBoundIndex(2).Should().BeEqualTo(-1);

        for (int i = 10; i < 20; i++)
            set.Add(i);
        await set.Should().BeStrictlyEquivalentTo((int[])[2, 3, 4, 7, 8, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19]);
        set.RemoveAt(13);
        await set.Should().BeStrictlyEquivalentTo((int[])[2, 3, 4, 7, 8, 10, 11, 12, 13, 14, 15, 16, 17, 19]);
        set.RemoveAt(8);
        await set.Should().BeStrictlyEquivalentTo((int[])[2, 3, 4, 7, 8, 10, 11, 12, 14, 15, 16, 17, 19]);
        set.RemoveAt(1);
        await set.Should().BeStrictlyEquivalentTo((int[])[2, 4, 7, 8, 10, 11, 12, 14, 15, 16, 17, 19]);
    }
    [Test]
    public async Task MultiSet()
    {
        var set = new Set<int>([6, 7, 8, 1, 2, 3, 4, 5, 1, 2, 3], true);
        set.Add(9);
        set.Add(5);
        await set.Should().BeStrictlyEquivalentTo((int[])[1, 1, 2, 2, 3, 3, 4, 5, 5, 6, 7, 8, 9]);
        await set.Should().HaveCount(13);
        set.Remove(5);
        await set.Should().HaveCount(12);
        await set.Should().BeStrictlyEquivalentTo((int[])[1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9]);
        await set.FindByIndex(12).NodeRef.Should().BeEqualTo(-1);
        await set.FindByIndex(11).Node.Value.Should().BeEqualTo(9);
        await Assert.That(set.FindNode(5)).IsNotNull();

        await set.Reversed().Should().BeStrictlyEquivalentTo([9, 8, 7, 6, 5, 4, 3, 3, 2, 2, 1, 1]);
        await set.EnumerateNode().Select(n => n.Node.Value).Should().BeStrictlyEquivalentTo([1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9]);
        await set.EnumerateNodeUpper(6).Select(n => n.Node.Value).Should().BeStrictlyEquivalentTo([6, 7, 8, 9]);
        await set.EnumerateNodeLower(6).Select(n => n.Node.Value).Should().BeStrictlyEquivalentTo([6, 5, 4, 3, 3, 2, 2, 1, 1]);

        await set.FindNodeLowerBound(3).Node.Value.Should().BeEqualTo(3);
        await set.FindNodeUpperBound(3).Node.Value.Should().BeEqualTo(4);
        await set.FindNodeReverseLowerBound(3).Node.Value.Should().BeEqualTo(3);
        await set.FindNodeReverseUpperBound(3).Node.Value.Should().BeEqualTo(2);

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

        await set.FindNodeLowerBound(10).NodeRef.Should().BeEqualTo(-1);
        await set.FindNodeUpperBound(10).NodeRef.Should().BeEqualTo(-1);
        await set.FindNodeReverseLowerBound(0).NodeRef.Should().BeEqualTo(-1);
        await set.FindNodeReverseUpperBound(1).NodeRef.Should().BeEqualTo(-1);
    }
    [Test]
    public async Task ReverseComparer()
    {
        var set = new Set<int, ReverseComparer<int>>([6, 7, 8, 1, 2, 3, 4, 5, 1, 2, 3]);
        set.Add(9);
        set.Add(5);
        await set.Should().BeStrictlyEquivalentTo((int[])[9, 8, 7, 6, 5, 4, 3, 2, 1]);
        await set.Should().HaveCount(9);
        set.Remove(5);
        await set.Should().HaveCount(8);
        await set.Should().BeStrictlyEquivalentTo((int[])[9, 8, 7, 6, 4, 3, 2, 1]);
        await set.FindByIndex(8).NodeRef.Should().BeEqualTo(-1);
        await set.FindByIndex(7).Node.Value.Should().BeEqualTo(1);
        await set.FindNode(5).Should().BeNull();

        await set.FindNodeLowerBound(6).Node.Value.Should().BeEqualTo(6);
        await set.FindNodeUpperBound(6).Node.Value.Should().BeEqualTo(4);
        await set.FindNodeLowerBound(5).Node.Value.Should().BeEqualTo(4);
        await set.FindNodeUpperBound(5).Node.Value.Should().BeEqualTo(4);

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

        await set.FindNodeLowerBound(0).NodeRef.Should().BeEqualTo(-1);
        await set.FindNodeUpperBound(0).NodeRef.Should().BeEqualTo(-1);
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
                await set.FindByIndex(i).Node.Value.Should().BeEqualTo(i);
            }
        }
    }

    [Test]
    public async Task Enumerate()
    {
        for (int count = 0; count < 64; count++)
        {
            IList<int> arr = Enumerable.Range(0, count).ToArray();
            var set = new Set<int>(arr);
            await set.Reversed().Should().BeStrictlyEquivalentTo(arr.Reverse());
            await set.EnumerateNode().Select(n => n.Node.Value).Should().BeStrictlyEquivalentTo(arr);
            await set.EnumerateNode(reverse: true).Select(n => n.Node.Value).Should().BeStrictlyEquivalentTo(arr.Reverse());

            for (int i = 0; i < count; i++)
            {
                await set.EnumerateNodeSkip(i).Select(n => n.Node.Value).Should().BeStrictlyEquivalentTo(arr.Skip(i));
                await set.EnumerateNodeRev(i)
                    .Select(n => n.Node.Value).Should().BeStrictlyEquivalentTo(arr.Take(i + 1).Reverse());
            }
        }
    }

    [Test, MultipleAssertions]
    public async Task EnumerateMulti()
    {
        var arr = new[] { 1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9 };
        var set = new Set<int>(arr, true);
        await set.Reversed().Should().BeStrictlyEquivalentTo([9, 8, 7, 6, 5, 4, 3, 3, 2, 2, 1, 1]);
        await set.EnumerateNode().Select(n => n.Node.Value).Should().BeStrictlyEquivalentTo([1, 1, 2, 2, 3, 3, 4, 5, 6, 7, 8, 9]);
        await set.EnumerateNode(reverse: true).Select(n => n.Node.Value).Should().BeStrictlyEquivalentTo([9, 8, 7, 6, 5, 4, 3, 3, 2, 2, 1, 1]);

        for (int i = 0; i < arr.Length; i++)
        {
            await set.EnumerateNodeSkip(i).Select(n => n.Node.Value).Should().BeStrictlyEquivalentTo(arr.Skip(i));
            await set.EnumerateNodeRev(i)
                .Select(n => n.Node.Value).Should().BeStrictlyEquivalentTo(arr.Take(i + 1).Reverse());
        }
    }
}