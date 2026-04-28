namespace Kzrnm.Competitive.Testing.DataStructure.String;

[NotInParallel]
public class RollingHashTests
{
    Random rnd = new Random(227);
    [Test, MultipleAssertions]
    public async Task Simple()
    {
        const string a2m = "abcdefghijklm";
        const string str = a2m + a2m;
        var rh = RollingHash.Create(str);

        var notMatchCount = 0;
        var notMatchHashNotMatchCount = 0;

        for (int l1 = 0; l1 < str.Length; l1++)
            for (int r1 = l1 + 1; r1 <= str.Length; r1++)
                for (int l2 = 0; l2 < str.Length; l2++)
                    for (int r2 = l2 + 1; r2 <= str.Length; r2++)
                    {
                        if (str.AsSpan()[l1..r1].Equals(str.AsSpan()[l2..r2], StringComparison.Ordinal))
                        {
                            await rh[l1..r1].Should().BeEqualTo(rh[l2..r2]);
                        }
                        else
                        {
                            notMatchCount++;
                            if (rh[l1..r1] != rh[l2..r2])
                                notMatchHashNotMatchCount++;
                        }
                    }
        await ((double)notMatchHashNotMatchCount / notMatchCount).Should().BeGreaterThan(0.9999);
    }
    [Test, MultipleAssertions]
    public async Task Repeat()
    {
        var str = string.Join("", Enumerable.Repeat(rnd.NextString(7), 4));
        var rh = RollingHash.Create((ReadOnlySpan<char>)str);

        var notMatchCount = 0;
        var notMatchHashNotMatchCount = 0;

        for (int l1 = 0; l1 < str.Length; l1++)
            for (int r1 = l1 + 1; r1 <= str.Length; r1++)
                for (int l2 = 0; l2 < str.Length; l2++)
                    for (int r2 = l2 + 1; r2 <= str.Length; r2++)
                    {
                        if (str[l1..r1].Equals(str[l2..r2], StringComparison.Ordinal))
                        {
                            await rh[l1..r1].Should().BeEqualTo(rh[l2..r2]);
                        }
                        else
                        {
                            notMatchCount++;
                            if (rh[l1..r1] != rh[l2..r2])
                                notMatchHashNotMatchCount++;
                        }
                    }
        await ((double)notMatchHashNotMatchCount / notMatchCount).Should().BeGreaterThan(0.9999);
    }
    [Test, MultipleAssertions]
    public async Task Large()
    {
        var str = rnd.NextIntArray(40, 1, 4);
        var rh = RollingHash.Create((ReadOnlySpan<int>)str);

        var notMatchCount = 0;
        var notMatchHashNotMatchCount = 0;

        for (int l1 = 0; l1 < str.Length; l1++)
            for (int r1 = l1 + 1; r1 <= str.Length; r1++)
                for (int l2 = 0; l2 < str.Length; l2++)
                    for (int r2 = l2 + 1; r2 <= str.Length; r2++)
                    {
                        if (str[l1..r1].SequenceEqual(str[l2..r2]))
                        {
                            await rh[l1..r1].Should().BeEqualTo(rh[l2..r2]);
                        }
                        else
                        {
                            notMatchCount++;
                            if (rh[l1..r1] != rh[l2..r2])
                                notMatchHashNotMatchCount++;
                        }
                    }
        await ((double)notMatchHashNotMatchCount / notMatchCount).Should().BeGreaterThan(0.9999);
    }
}