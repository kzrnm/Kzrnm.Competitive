namespace Kzrnm.Competitive.Testing.Collection;

public class PersistentSetTests
{
    [Test, MultipleAssertions]
    public async Task Set()
    {
        var ss = new PersistentSet<int>[100];
        ss[0] = PersistentSet<int>.Empty();
        await ss[0].Should().BeEmpty();
        for (int i = 1; i < ss.Length; i++)
        {
            ss[i] = ss[i - 1].Add(i);
            await ss[i].Should().BeStrictlyEquivalentTo(Enumerable.Range(1, i));
            await ss[i].Count.Should().BeEqualTo(i);
            await ss[i].Reverse().Should().BeStrictlyEquivalentTo(Enumerable.Range(1, i).Reverse());
        }

        for (int i = ss.Length - 1; i >= 0; i--)
        {
            await ss[i].Should().BeStrictlyEquivalentTo(Enumerable.Range(1, i));
            await ss[i].Count.Should().BeEqualTo(i);

            var other0 = ss[i].Remove(0);
            await other0.Should().BeStrictlyEquivalentTo(Enumerable.Range(1, i));
            await other0.Count.Should().BeEqualTo(i);

            if (i > 0)
            {
                var other1 = ss[i].Remove(1);
                await other1.Should().BeStrictlyEquivalentTo(Enumerable.Range(2, i - 1));
                await other1.Count.Should().BeEqualTo(i - 1);
            }

            var add2 = ss[i].Add(i).Add(i);
            await add2.Count.Should().BeEqualTo(i + 2);
            await add2.Should().BeStrictlyEquivalentTo(Enumerable.Range(1, i).Append(i).Append(i).OrderBy(i => i).ToArray());
        }
    }
    [Test, MultipleAssertions]
    public async Task SetDictionary()
    {
        var ss = new PersistentSetDictionary<int, string>[100];
        ss[0] = PersistentSetDictionary<int, string>.Empty();
        await ss[0].Should().BeEmpty();
        for (int i = 1; i < ss.Length; i++)
        {
            ss[i] = ss[i - 1].Add(i, i.ToString());
            await ss[i].Should().BeStrictlyEquivalentTo(Enumerable.Range(1, i).Select(i => KeyValuePair.Create(i, i.ToString())));
            await ss[i].Count.Should().BeEqualTo(i);
            await ss[i].Reverse().Should().BeStrictlyEquivalentTo(Enumerable.Range(1, i).Select(i => KeyValuePair.Create(i, i.ToString())).Reverse());
        }

        for (int i = ss.Length - 1; i >= 0; i--)
        {
            await ss[i].Should().BeStrictlyEquivalentTo(Enumerable.Range(1, i).Select(i => KeyValuePair.Create(i, i.ToString())));
            await ss[i].Count.Should().BeEqualTo(i);

            var other0 = ss[i].Remove(0);
            await other0.Should().BeStrictlyEquivalentTo(Enumerable.Range(1, i).Select(i => KeyValuePair.Create(i, i.ToString())));
            await other0.Count.Should().BeEqualTo(i);

            if (i > 0)
            {
                var other1 = ss[i].Remove(1);
                await other1.Should().BeStrictlyEquivalentTo(Enumerable.Range(2, i - 1).Select(i => KeyValuePair.Create(i, i.ToString())));
                await other1.Count.Should().BeEqualTo(i - 1);
            }

            var add2 = ss[i].Add(i, i.ToString()).Add(i, i.ToString());
            await add2.Count.Should().BeEqualTo(i + 2);
            await add2.Should().BeStrictlyEquivalentTo(Enumerable.Range(1, i).Append(i).Append(i).OrderBy(i => i).Select(i => KeyValuePair.Create(i, i.ToString())).ToArray());
        }
    }


    [Test, MultipleAssertions]
    public async Task RemoveMinMax()
    {
        var sd = PersistentSetDictionary<int, int>.Empty(isMulti: true);
        await sd.Should().BeEmpty();
        sd = sd.Add(1, 1);
        sd = sd.Add(1, 2);
        sd = sd.Add(2, 1);
        sd = sd.Add(2, 2);
        await sd.Should().BeStrictlyEquivalentTo([
            KeyValuePair.Create(1, 2),
            KeyValuePair.Create(1, 1),
            KeyValuePair.Create(2, 2),
            KeyValuePair.Create(2, 1),
        ]);

        await sd.Min.Should().BeEqualTo(KeyValuePair.Create(1, 2));
        await sd.RemoveMin(out var min).Should().BeStrictlyEquivalentTo([
            KeyValuePair.Create(1, 1),
            KeyValuePair.Create(2, 2),
            KeyValuePair.Create(2, 1),
        ]);
        await min.Should().BeEqualTo(KeyValuePair.Create(1, 2));

        await sd.Max.Should().BeEqualTo(KeyValuePair.Create(2, 1));
        await sd.RemoveMax(out var max).Should().BeStrictlyEquivalentTo([
            KeyValuePair.Create(1, 2),
            KeyValuePair.Create(1, 1),
            KeyValuePair.Create(2, 2),
        ]);
        await max.Should().BeEqualTo(KeyValuePair.Create(2, 1));
    }
}