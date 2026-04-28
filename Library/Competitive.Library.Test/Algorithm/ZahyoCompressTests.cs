namespace Kzrnm.Competitive.Testing.Algorithm;

public class ZahyoCompressTests
{
    [Test]
    public async Task Compress()
    {
        var c = ZahyoCompress.Create("compress").Compress();
        var (decN, decO) = c;

        using (Assert.Multiple())
        {
            await decN.Should().BeSameReferenceAs(c.NewTable);
            await decO.Should().BeSameReferenceAs(c.Original);

            await c.NewTable.Should().HaveCount(7);
            await c.NewTable.Should().ContainKeyWithValue('c', 0);
            await c.NewTable.Should().ContainKeyWithValue('o', 3);
            await c.NewTable.Should().ContainKeyWithValue('m', 2);
            await c.NewTable.Should().ContainKeyWithValue('p', 4);
            await c.NewTable.Should().ContainKeyWithValue('r', 5);
            await c.NewTable.Should().ContainKeyWithValue('e', 1);
            await c.NewTable.Should().ContainKeyWithValue('s', 6);
            await c.Original.Should().BeEquivalentOrderTo(['c', 'e', 'm', 'o', 'p', 'r', 's']);
            await c.Replace("compress").Should().BeEquivalentOrderTo([0, 3, 2, 4, 5, 1, 6, 6]);
        }

        using (Assert.Multiple())
        {
            c.Compress(new ReverseComparer<char>());
            await c.NewTable.Should().HaveCount(7);
            await c.NewTable.Should().ContainKeyWithValue('c', 6);
            await c.NewTable.Should().ContainKeyWithValue('o', 3);
            await c.NewTable.Should().ContainKeyWithValue('m', 4);
            await c.NewTable.Should().ContainKeyWithValue('p', 2);
            await c.NewTable.Should().ContainKeyWithValue('r', 1);
            await c.NewTable.Should().ContainKeyWithValue('e', 5);
            await c.NewTable.Should().ContainKeyWithValue('s', 0);
            await c.Original.Should().BeEquivalentOrderTo(['s', 'r', 'p', 'o', 'm', 'e', 'c']);
            await c.Replace("compress").Should().BeEquivalentOrderTo([6, 3, 4, 2, 1, 5, 0, 0]);
        }

        c.Add('i');
        c.Add('r');
        c.Add('k');
        c.Compress();

        using (Assert.Multiple())
        {
            await c.NewTable.Should().HaveCount(9);
            await c.NewTable.Should().ContainKeyWithValue('c', 0);
            await c.NewTable.Should().ContainKeyWithValue('o', 5);
            await c.NewTable.Should().ContainKeyWithValue('m', 4);
            await c.NewTable.Should().ContainKeyWithValue('p', 6);
            await c.NewTable.Should().ContainKeyWithValue('r', 7);
            await c.NewTable.Should().ContainKeyWithValue('e', 1);
            await c.NewTable.Should().ContainKeyWithValue('s', 8);
            await c.NewTable.Should().ContainKeyWithValue('i', 2);
            await c.NewTable.Should().ContainKeyWithValue('k', 3);
            await c.Original.Should().BeEquivalentOrderTo(['c', 'e', 'i', 'k', 'm', 'o', 'p', 'r', 's']);
        }
    }

    [Test, MultipleAssertions]
    public async Task CompressedArray()
    {
        await ZahyoCompress.CompressedArray("compress".AsSpan()).Should().BeEquivalentOrderTo([0, 3, 2, 4, 5, 1, 6, 6]);

        var a = new int[] { 3, 5, 6, 41, 6 };
        await ZahyoCompress.CompressedArray(a).Should().BeEquivalentOrderTo([0, 1, 2, 3, 2]);
    }
}