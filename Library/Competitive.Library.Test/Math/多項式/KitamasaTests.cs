using AtCoder;
using System.Runtime.InteropServices;

#pragma warning disable CS0618 // 型またはメンバーが旧型式です
namespace Kzrnm.Competitive.Testing.MathNS;

public class KitamasaTests
{
    struct DMod1;
    struct DMod2;
    private static uint[] NativeDp<Mod>(ReadOnlySpan<uint> a, ReadOnlySpan<uint> c, int mod) where Mod : struct
    {
        DynamicModInt<Mod>.Mod = mod;
        var res = new uint[1000];
        var drr = MemoryMarshal.Cast<uint, DynamicModInt<Mod>>(res.AsSpan());
        a.CopyTo(res);
        for (int k = a.Length; k < drr.Length; k++)
            for (int i = 0; i < c.Length; i++)
            {
                drr[k] += c[i] * drr[k - i - 1];
            }
        return res;
    }

    [Test, MultipleAssertions]
    public async Task Mod1000000007()
    {
        var rnd = new Random(42);
        for (int n = 2; n < 10; n++)
        {
            var arr = (uint[])(object)rnd.NextIntArray(n, 0, 1000000007);
            var crr = (uint[])(object)rnd.NextIntArray(n, 0, 1000000007);
            var expected = NativeDp<DMod1>(arr, crr, 1000000007);
            for (int l = 0; l < 40; l++)
            {
                await Kitamasa.FastKitamasa<Mod1000000007>(arr, crr, l).Should().BeEqualTo(expected[l]);
                await Kitamasa.FastKitamasa(arr, crr, l, 1000000007).Should().BeEqualTo(expected[l]);
            }
        }
    }

    [Test, MultipleAssertions]
    public async Task Mod998244353()
    {
        var rnd = new Random(42);
        for (int n = 2; n < 10; n++)
        {
            var arr = (uint[])(object)rnd.NextIntArray(n, 0, 998244353);
            var crr = (uint[])(object)rnd.NextIntArray(n, 0, 998244353);
            var expected = NativeDp<DMod2>(arr, crr, 998244353);
            for (int l = 0; l < 40; l++)
            {
                await Kitamasa.FastKitamasa<Mod998244353>(arr, crr, l).Should().BeEqualTo(expected[l]);
                await Kitamasa.FastKitamasa(arr, crr, l, 998244353).Should().BeEqualTo(expected[l]);
            }
        }
    }
}