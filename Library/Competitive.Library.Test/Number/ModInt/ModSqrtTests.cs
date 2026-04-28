using AtCoder;

namespace Kzrnm.Competitive.Testing.Number;

public class ModSqrtTests
{
    [Test, MultipleAssertions]
    public async Task Long()
    {
        foreach (var mod in new PrimeNumber(120).AsEnumerable())
        {
            var hs = Build(mod);
            for (int i = 0; i < mod; i++)
            {
                if (hs[i].Count == 0)
                    await ModSqrt.Solve(i, mod).Should().BeEqualTo(-1);
                else
                    await hs[i].Should().Contain(ModSqrt.Solve(i, mod));
            }
        }
    }

    struct DId;
    [Test, MultipleAssertions]
    public async Task DynamicModInt()
    {
        foreach (var mod in new PrimeNumber(120).AsEnumerable())
        {
            var hs = Build(mod);
            DynamicModInt<DId>.Mod = mod;
            for (int i = 0; i < mod; i++)
            {
                if (hs[i].Count == 0)
                    await ModSqrt.Solve(DynamicModInt<DId>.Raw(i)).Should().BeEqualTo(-1);
                else
                    await hs[i].Should().Contain(ModSqrt.Solve(DynamicModInt<DId>.Raw(i)));
            }
        }
    }

    readonly struct Mod6221 : IStaticMod
    {
        public bool IsPrime => true;
        public uint Mod => 6221;
    }
    readonly struct Mod1097 : IStaticMod
    {
        public bool IsPrime => true;
        public uint Mod => 1097;
    }
    [Test, MultipleAssertions]
    public async Task StaticModInt()
    {
        int mod = 6221;
        var hs = Build(mod);
        for (int i = 0; i < mod; i++)
        {
            if (hs[i].Count == 0)
                await ModSqrt.Solve(StaticModInt<Mod6221>.Raw(i)).Should().BeEqualTo(-1);
            else
                await hs[i].Should().Contain(ModSqrt.Solve(StaticModInt<Mod6221>.Raw(i)));
        }

        mod = 1097;
        hs = Build(mod);
        for (int i = 0; i < mod; i++)
        {
            if (hs[i].Count == 0)
                await ModSqrt.Solve(StaticModInt<Mod1097>.Raw(i)).Should().BeEqualTo(-1);
            else
                await hs[i].Should().Contain(ModSqrt.Solve(StaticModInt<Mod1097>.Raw(i)));
        }
    }

    [Test, MultipleAssertions]
    public async Task MontgomeryModInt()
    {
        int mod = 6221;
        var hs = Build(mod);
        for (int i = 0; i < mod; i++)
        {
            if (hs[i].Count == 0)
                await ModSqrt.Solve((MontgomeryModInt<Mod6221>)i).Should().BeEqualTo(-1);
            else
                await hs[i].Should().Contain(ModSqrt.Solve((MontgomeryModInt<Mod6221>)i));
        }

        mod = 1097;
        hs = Build(mod);
        for (int i = 0; i < mod; i++)
        {
            if (hs[i].Count == 0)
                await ModSqrt.Solve((MontgomeryModInt<Mod1097>)i).Should().BeEqualTo(-1);
            else
                await hs[i].Should().Contain(ModSqrt.Solve((MontgomeryModInt<Mod1097>)i));
        }
    }

    private static HashSet<int>[] Build(int mod)
    {
        var hs = Global.NewArray(mod, () => new HashSet<int>());
        for (int i = 0; i < mod; i++)
        {
            hs[i * i % mod].Add(i);
        }
        return hs;
    }
}