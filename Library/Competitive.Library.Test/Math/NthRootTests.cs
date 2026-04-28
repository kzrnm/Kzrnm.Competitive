
namespace Kzrnm.Competitive.Testing.MathNS;

public class NthRootTests
{
    [Test]
    [Arguments(1, 1, 1)]
    [Arguments(1, 10000, 1)]
    [Arguments(1, 1000000000, 1)]
    [Arguments(215, 3, 5)]
    [Arguments(216, 3, 6)]
    [Arguments(217, 3, 6)]
    [Arguments(9999999999, 10, 9)]
    [Arguments(10000000000, 10, 10)]
    [Arguments(10000000001, 10, 10)]
    [Arguments(18446744073709551615, 1, 18446744073709551615)]
    [Arguments(18446744073709551615, 2, 4294967295)]
    [Arguments(18446744073709551615, 63, 2)]
    [Arguments(18446744073709551615, 64, 1)]
    [Arguments(1796495231553, 3, 12156)]
    public async Task IntegerRoot(ulong num, long n, ulong expected)
        => await NthRoots.IntegerRoot(num, n).Should().BeEqualTo(expected);
}