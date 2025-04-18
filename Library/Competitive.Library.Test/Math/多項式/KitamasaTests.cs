using AtCoder;
using System;
using System.Runtime.InteropServices;

#pragma warning disable CS0618 // 型またはメンバーが旧型式です
namespace Kzrnm.Competitive.Testing.MathNS;

public class KitamasaTests
{
    private struct DMod : IDynamicModIntId { }
    private static uint[] NativeDp(ReadOnlySpan<uint> a, ReadOnlySpan<uint> c, int mod)
    {
        DynamicModInt<DMod>.Mod = mod;
        var res = new uint[1000];
        var drr = MemoryMarshal.Cast<uint, DynamicModInt<DMod>>(res);
        a.CopyTo(res);
        for (int k = a.Length; k < drr.Length; k++)
            for (int i = 0; i < c.Length; i++)
            {
                drr[k] += c[i] * drr[k - i - 1];
            }
        return res;
    }

    [Fact]
    public void Mod1000000007()
    {
        var rnd = new Random(42);
        for (int n = 2; n < 10; n++)
        {
            var arr = MemoryMarshal.Cast<int, uint>(rnd.NextIntArray(n, 0, 1000000007));
            var crr = MemoryMarshal.Cast<int, uint>(rnd.NextIntArray(n, 0, 1000000007));
            var expected = NativeDp(arr, crr, 1000000007);
            for (int l = 0; l < 40; l++)
            {
                Kitamasa.FastKitamasa<Mod1000000007>(arr, crr, l).ShouldBe(expected[l]);
                Kitamasa.FastKitamasa(arr, crr, l, 1000000007).ShouldBe(expected[l]);
            }
        }
    }

    [Fact]
    public void Mod998244353()
    {
        var rnd = new Random(42);
        for (int n = 2; n < 10; n++)
        {
            var arr = MemoryMarshal.Cast<int, uint>(rnd.NextIntArray(n, 0, 998244353));
            var crr = MemoryMarshal.Cast<int, uint>(rnd.NextIntArray(n, 0, 998244353));
            var expected = NativeDp(arr, crr, 998244353);
            for (int l = 0; l < 40; l++)
            {
                Kitamasa.FastKitamasa<Mod998244353>(arr, crr, l).ShouldBe(expected[l]);
                Kitamasa.FastKitamasa(arr, crr, l, 998244353).ShouldBe(expected[l]);
            }
        }
    }
}
