namespace Kzrnm.Competitive.Testing.DataStructure.String;

[NotInParallel]
public class RollingHashWritableTests
{
    Random rnd = new Random(227);
    [Test, MultipleAssertions]
    public async Task Static()
    {
        var str = rnd.NextIntArray(100, 1, 4);
        var rh = RollingHash.Create(str);
        var rhe = RollingHashWritable.Create(str);
        for (int l1 = 0; l1 < str.Length; l1++)
            for (int r1 = l1 + 1; r1 <= str.Length; r1++)
                await rhe[l1..r1].Should().BeEqualTo(rh[l1..r1]);
    }

    [Test, MultipleAssertions]
    public async Task Large()
    {
        const int length = 20;
        var str = rnd.NextIntArray(length, 1, 3);
        var rh = RollingHashWritable.Create((Span<int>)str);

        for (int q = 0; q < 4; q++)
        {
            for (int p = 0; p < 20; p++)
            {
                var ix = rnd.Next(length);
                var v = rnd.Next(3);
                rh.Set(ix, v);
                str[ix] = v;
            }
            var re = RollingHash.Create(str);

            await rh.Length.Should().BeEqualTo(re.Length);
            for (int l = 0; l < rh.Length; l++)
                for (int r = l + 1; r <= rh.Length; r++)
                    await rh[l..r].Should().BeEqualTo(re[l..r]);
        }
    }
}