using AtCoder;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Number;

public class ModIntExtensionTests
{
    [Fact]
    public void Sum()
    {
        Inner<StaticModInt<Mod998244353>>();
        Inner<StaticModInt<Mod1000000007>>();

        Inner<MontgomeryModInt<Mod998244353>>();
        Inner<MontgomeryModInt<Mod1000000007>>();

        static void Inner<T>() where T : IModInt<T>
        {
            var arrInt = Enumerable.Range(1, 10).ToArray();
            var arrMod = arrInt.Select(T.CreateTruncating).ToArray();
            for (int i = 0; i < 10; i++)
                for (int j = i; j < 10; j++)
                    arrMod[i..j].Sum().Value.ShouldBe(arrInt[i..j].Sum());
        }
    }
    [Fact]
    public void SumRandom()
    {
        Inner<StaticModInt<Mod998244353>>();
        Inner<StaticModInt<Mod1000000007>>();

        Inner<MontgomeryModInt<Mod998244353>>();
        Inner<MontgomeryModInt<Mod1000000007>>();

        static void Inner<T>() where T : IModInt<T>
        {
            var rnd = new Xoshiro256(227);
            for (int q = 0; q < 1000; q++)
            {
                var len = rnd.NextInt32(1, 500);
                var arrMod = new StaticModInt<Mod1000000007>[len];
                for (int i = 0; i < arrMod.Length; i++)
                {
                    arrMod[i] = rnd.NextInt32(1000000007);
                }
                arrMod.Sum().Value.ShouldBe((int)(arrMod.Select(m => (long)m.Value).Sum() % 1000000007));
            }
        }
    }
}
