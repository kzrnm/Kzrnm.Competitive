using System.Linq;

namespace Kzrnm.Competitive.Testing.MathNS;

public class PrimeCountingTests
{
    [Fact]
    public void PrimeCount()
    {
        var naiveCounting = new uint[10001];
        var p = new PrimeNumber(10000).ToHashSet();
        PrimeCounting.Count(0).ShouldBe(0);
        PrimeCounting.Count(1).ShouldBe(0);
        for (int i = 2; i < naiveCounting.Length; i++)
        {
            naiveCounting[i] = naiveCounting[i - 1];
            if (p.Contains(i))
                naiveCounting[i]++;

            PrimeCounting.Count(i).ShouldBe(naiveCounting[i], $"Number: {i}");
        }
    }

    [Theory]
    [InlineData(1ul << 40, 41203088796)]
    [InlineData(100000000000, 4118054813)]
    [InlineData(45261156424, 1926914621)]
    [InlineData(94261156424, 3891216451)]
    [InlineData(5233235398, 245387790)]
    public void PrimeCountLarge(ulong num, ulong expected)
    {
        PrimeCounting.Count(num).ShouldBe(expected);
    }
}