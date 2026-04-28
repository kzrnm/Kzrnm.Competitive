using AtCoder;

namespace Kzrnm.Competitive.Testing.MathNS;

public class NttTests
{
    static MontgomeryModInt<Mod998244353>[] ConvNative(MontgomeryModInt<Mod998244353>[] a, MontgomeryModInt<Mod998244353>[] b)
    {
        int n = a.Length, m = b.Length;
        var c = new MontgomeryModInt<Mod998244353>[n + m - 1];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                c[i + j] += a[i] * b[j];
            }
        }
        return c;
    }

    [Test]
    [Arguments(8)]
    [Arguments(16)]
    [Arguments(128)]
    [Arguments(1024)]
    [Arguments(2048)]
    public async Task Ntt(int n)
    {
        var rnd = new Random(42);
        var a = new MontgomeryModInt<Mod998244353>[n];
        for (int i = 0; i < n; i++) a[i] = rnd.NextUInt();
        var b = (MontgomeryModInt<Mod998244353>[])a.Clone();
        NumberTheoreticTransform<Mod998244353>.Ntt(a);
        NumberTheoreticTransform<Mod998244353>.NttLogical(b);
        await a.Should().BeEquivalentOrderTo(b);
    }

    [Test]
    [Arguments(8)]
    [Arguments(16)]
    [Arguments(128)]
    [Arguments(1024)]
    [Arguments(2048)]
    public async Task INtt(int n)
    {
        var rnd = new Random(42);
        var a = new MontgomeryModInt<Mod998244353>[n];
        for (int i = 0; i < n; i++) a[i] = rnd.NextUInt();
        var b = (MontgomeryModInt<Mod998244353>[])a.Clone();
        NumberTheoreticTransform<Mod998244353>.INtt(a);
        NumberTheoreticTransform<Mod998244353>.INttLogical(b);
        await a.Should().BeEquivalentOrderTo(b);
    }

    [Test, MultipleAssertions]
    [Arguments(123, 234)]
    [Arguments(1234, 2345)]
    [Arguments(1235, 2345)]
    public async Task Multiply(int n, int m)
    {
        var rnd = new Random(42);
        var a = new MontgomeryModInt<Mod998244353>[n];
        var b = new MontgomeryModInt<Mod998244353>[m];
        for (int i = 0; i < n; i++) a[i] = rnd.NextUInt();
        for (int i = 0; i < m; i++) b[i] = rnd.NextUInt();
        var expected = ConvNative(a, b);
        await NumberTheoreticTransform<Mod998244353>.MultiplyLogical(a, b).Should().BeEquivalentOrderTo(expected);
        await NumberTheoreticTransform<Mod998244353>.Multiply(a, b).Should().BeEquivalentOrderTo(expected);
    }
}