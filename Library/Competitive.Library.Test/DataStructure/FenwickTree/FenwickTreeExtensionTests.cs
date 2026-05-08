using AtCoder.Extension;
using IntFenwickTree = AtCoder.FenwickTree<int>;

namespace Kzrnm.Competitive.Testing.DataStructure;

public class FenwickTreeExtensionTests
{
    [Test, MultipleAssertions]
    public async Task LowerBound()
    {
        var rnd = new Random(227);
        var rndArray = new int[128];
        for (int i = 0; i < rndArray.Length; i++)
        {
            rndArray[i] = rnd.Next(314);
        }
        foreach (var array in new[]
        {
            Enumerable.Range(1, 31).ToArray(),
            Enumerable.Range(1, 32).ToArray(),
            Enumerable.Range(1, 33).ToArray(),
            Enumerable.Range(1, 100).ToArray(),
            rndArray,
        })
        {
            var fw = new IntFenwickTree(array.Length);
            for (int i = 0; i < array.Length; i++)
                fw.Add(i, array[i]);
            var sums = array.ToArray();
            for (int i = 1; i < sums.Length; i++)
                sums[i] += sums[i - 1];

            var max = sums[^1] + 2;
            await fw.LowerBound(0).Should().BeEqualTo(0);
            for (int v = 1; v < max; v++)
            {
                var expected = sums.LowerBound(v) + 1;
                await fw.LowerBound(v).Should().BeEqualTo(expected);
                if (expected - 1 == sums.Length)
                    await fw[..].Should().BeLessThan(v);
                else
                {
                    await fw[..expected].Should().BeGreaterThanOrEqualTo(v);
                    await fw[..(expected - 1)].Should().BeLessThan(v);
                }
            }
        }
    }
    [Test, MultipleAssertions]
    public async Task UpperBound()
    {
        var rnd = new Random(227);
        var rndArray = new int[128];
        for (int i = 0; i < rndArray.Length; i++)
        {
            rndArray[i] = rnd.Next(314);
        }
        foreach (var array in new[]
        {
            Enumerable.Range(1, 31).ToArray(),
            Enumerable.Range(1, 32).ToArray(),
            Enumerable.Range(1, 33).ToArray(),
            Enumerable.Range(1, 100).ToArray(),
            rndArray,
        })
        {
            var fw = new IntFenwickTree(array.Length);
            for (int i = 0; i < array.Length; i++)
                fw.Add(i, array[i]);
            var sums = array.ToArray();
            for (int i = 1; i < sums.Length; i++)
                sums[i] += sums[i - 1];

            var max = sums[^1] + 2;
            for (int v = 0; v < max; v++)
            {
                var expected = sums.UpperBound(v) + 1;
                await fw.UpperBound(v).Should().BeEqualTo(expected);
                if (expected - 1 == sums.Length)
                    await fw[..].Should().BeLessThanOrEqualTo(v);
                else
                {
                    await fw[..expected].Should().BeGreaterThan(v);
                    await fw[..(expected - 1)].Should().BeLessThanOrEqualTo(v);
                }
            }
        }
    }

    [Test, MultipleAssertions]
    public async Task Get()
    {
        var rnd = new Random(227);
        var rndArray = new int[128];
        for (int i = 0; i < rndArray.Length; i++)
        {
            rndArray[i] = rnd.Next(314);
        }
        foreach (var array in new[]
        {
            Enumerable.Range(1, 31).ToArray(),
            Enumerable.Range(1, 32).ToArray(),
            Enumerable.Range(1, 33).ToArray(),
            Enumerable.Range(1, 100).ToArray(),
            rndArray,
        })
        {
            var fw = new IntFenwickTree(array.Length);
            for (int i = 0; i < array.Length; i++)
                fw.Add(i, array[i]);
            for (int i = 0; i < array.Length; i++)
                await fw.Get(i).Should().BeEqualTo(array[i]);
        }
    }

    [Test, MultipleAssertions]
    public async Task AddSpan()
    {
        var nums = Enumerable.Range(1, 100).ToArray().AsMemory();

        for (int n = 0; n <= 100; n++)
        {
            var fw1 = new IntFenwickTree(n);
            var fw2 = new IntFenwickTree(n);
            await fw1.data.Should().BeStrictlyEquivalentTo(fw2.data);

            fw1.Add(nums[..n].Span);
            for (int i = 0; i < n; i++)
                fw2.Add(i, i + 1);
            await fw1.data.Should().BeStrictlyEquivalentTo(fw2.data);

            fw1.Add(nums[..(n / 2)].Span);
            for (int i = 0; i < (n / 2); i++)
                fw2.Add(i, i + 1);
            await fw1.data.Should().BeStrictlyEquivalentTo(fw2.data);
        }
    }

    [Test, MultipleAssertions]
    public async Task ToArray()
    {
        var rnd = new Random(227);
        var rndArray = new int[128];
        for (int i = 0; i < rndArray.Length; i++)
        {
            rndArray[i] = rnd.Next(314);
        }
        foreach (var array in new[]
        {
            Enumerable.Range(1, 31).ToArray(),
            Enumerable.Range(1, 32).ToArray(),
            Enumerable.Range(1, 33).ToArray(),
            Enumerable.Range(1, 100).ToArray(),
            rndArray,
        })
        {
            var fw = new IntFenwickTree(array.Length);
            for (int i = 0; i < array.Length; i++)
                fw.Add(i, array[i]);
            var expected = array.Select<int, (int Item, int Sum)>(i => (i, i)).ToArray();
            for (int i = 1; i < expected.Length; i++)
            {
                expected[i].Sum += expected[i - 1].Sum;
            }
            await fw.ToArray().Should().BeStrictlyEquivalentTo(expected);
        }
    }
}