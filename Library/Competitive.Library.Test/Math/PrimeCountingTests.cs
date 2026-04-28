namespace Kzrnm.Competitive.Testing.MathNS;

public class PrimeCountingTests
{
    [Test, MultipleAssertions]
    public async Task PrimeCount()
    {
        var naiveCounting = new uint[10001];
        var p = new PrimeNumber(10000).ToHashSet();
        await PrimeCounting.Count(0).Should().BeEqualTo(0);
        await PrimeCounting.Count(1).Should().BeEqualTo(0);
        for (int i = 2; i < naiveCounting.Length; i++)
        {
            naiveCounting[i] = naiveCounting[i - 1];
            if (p.Contains(i))
                naiveCounting[i]++;

            await PrimeCounting.Count(i).Should().BeEqualTo(naiveCounting[i]);
        }
    }

    [Test]
    [Arguments(1ul << 40, 41203088796)]
    [Arguments(100000000000, 4118054813)]
    [Arguments(45261156424, 1926914621)]
    [Arguments(94261156424, 3891216451)]
    [Arguments(5233235398, 245387790)]
    public async Task PrimeCountLarge(ulong num, ulong expected)
    {
        await PrimeCounting.Count(num).Should().BeEqualTo(expected);
    }
}