using AtCoder;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Number
{
    public class ModSqrtTests
    {
        [Fact]
        public void Long()
        {
            foreach (var mod in new PrimeNumber(120))
            {
                var hs = Build(mod);
                for (int i = 0; i < mod; i++)
                {
                    if (hs[i].Count == 0)
                        ModSqrt.Solve(i, mod).Should().Be(-1, "nothing^2 ≡ {0} mod {1}", i, mod);
                    else
                        hs[i].Should().Contain(ModSqrt.Solve(i, mod), "result^2 ≡ {0} mod {1}", i, mod);
                }
            }
        }
        [Fact]
        public void DynamicModInt()
        {
            foreach (var mod in new PrimeNumber(120))
            {
                var hs = Build(mod);
                DynamicModInt<bool>.Mod = mod;
                for (int i = 0; i < mod; i++)
                {
                    if (hs[i].Count == 0)
                        ModSqrt.Solve(DynamicModInt<bool>.Raw(i)).Should().Be(-1, "nothing^2 ≡ {0} mod {1}", i, mod);
                    else
                        hs[i].Should().Contain(ModSqrt.Solve(DynamicModInt<bool>.Raw(i)), "result^2 ≡ {0} mod {1}", i, mod);
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
        [Fact]
        public void StaticModInt()
        {
            int mod = 6221;
            var hs = Build(mod);
            for (int i = 0; i < mod; i++)
            {
                if (hs[i].Count == 0)
                    ModSqrt.Solve(StaticModInt<Mod6221>.Raw(i)).Should().Be(-1, "nothing^2 ≡ {0} mod {1}", i, mod);
                else
                    hs[i].Should().Contain(ModSqrt.Solve(StaticModInt<Mod6221>.Raw(i)), "result^2 ≡ {0} mod {1}", i, mod);
            }

            mod = 1097;
            hs = Build(mod);
            for (int i = 0; i < mod; i++)
            {
                if (hs[i].Count == 0)
                    ModSqrt.Solve(StaticModInt<Mod1097>.Raw(i)).Should().Be(-1, "nothing^2 ≡ {0} mod {1}", i, mod);
                else
                    hs[i].Should().Contain(ModSqrt.Solve(StaticModInt<Mod1097>.Raw(i)), "result^2 ≡ {0} mod {1}", i, mod);
            }
        }

        [Fact]
        public void MontgomeryModInt()
        {
            int mod = 6221;
            var hs = Build(mod);
            for (int i = 0; i < mod; i++)
            {
                if (hs[i].Count == 0)
                    ModSqrt.Solve((MontgomeryModInt<Mod6221>)i).Should().Be(-1, "nothing^2 ≡ {0} mod {1}", i, mod);
                else
                    hs[i].Should().Contain(ModSqrt.Solve((MontgomeryModInt<Mod6221>)i), "result^2 ≡ {0} mod {1}", i, mod);
            }

            mod = 1097;
            hs = Build(mod);
            for (int i = 0; i < mod; i++)
            {
                if (hs[i].Count == 0)
                    ModSqrt.Solve((MontgomeryModInt<Mod1097>)i).Should().Be(-1, "nothing^2 ≡ {0} mod {1}", i, mod);
                else
                    hs[i].Should().Contain(ModSqrt.Solve((MontgomeryModInt<Mod1097>)i), "result^2 ≡ {0} mod {1}", i, mod);
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
}
