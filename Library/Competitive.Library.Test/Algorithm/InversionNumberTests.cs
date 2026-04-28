using System.Collections.Immutable;

namespace Kzrnm.Competitive.Testing.Algorithm;

public class InversionNumberTests
{
    public IEnumerable<ImmutableArray<int>> RandomInt32Cases()
    {
        var rnd = new Random(227);
        for (int len = 0; len < 10; len++)
            for (int q = 0; q < 2; q++)
            {
                var a = new int[len];
                yield return ImmutableArray.CreateRange(a);

                foreach (ref var v in a.AsSpan())
                    v = rnd.Next(len) + 1;
            }
    }
    [Test]
    [MethodDataSource(nameof(RandomInt32Cases))]
    public async Task Int32(ImmutableArray<int> input)
    {
        var a = input.ToArray();
        await InversionNumber.Inversion(a).Should().BeEqualTo(Naive((ReadOnlySpan<int>)a));
    }

    public IEnumerable<string> RandomStringCases()
    {
        var rnd = new Random(227);
        for (int len = 0; len < 30; len++)
            for (int q = 0; q < 2; q++)
            {
                var a = new char[len];
                yield return new(a);
                foreach (ref var v in a.AsSpan())
                    v = (char)(rnd.Next(26) + 'A');
            }
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(RandomStringCases))]
    public async Task String(string a)
    {
        await InversionNumber.Inversion(a.AsSpan()).Should().BeEqualTo(Naive<char>(a));
    }

    static long Naive<T>(ReadOnlySpan<T> a) where T : IComparable<T>
    {
        long s = 0;
        for (int i = 0; i < a.Length; i++)
            for (int j = i + 1; j < a.Length; j++)
                if (a[i].CompareTo(a[j]) > 0)
                    ++s;
        return s;
    }
}