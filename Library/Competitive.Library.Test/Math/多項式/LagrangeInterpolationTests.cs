using AtCoder;

namespace Kzrnm.Competitive.Testing.MathNS
{
    public class LagrangeInterpolationTest
    {
        public static TheoryData Coefficient_Data => new TheoryData<(int, int)[]>
        {
            new (int, int)[]
            {
                (0,1),
                (1,10),
            },
            new (int, int)[]
            {
                (0,1),
                (1,10),
                (2,11),
            },
            new (int, int)[]
            {
                (0,1),
                (1,10),
                (2,11),
                (3,-51),
            }
        };
        [Theory]
        [MemberData(nameof(Coefficient_Data))]
        public void Coefficient((int x, int y)[] data)
        {
            RunTest<Mod998244353>(data);
            RunTest<Mod1000000007>(data);

            static void RunTest<T>((int x, int y)[] plots) where T : struct, IStaticMod
            {
                var modPlots = new (MontgomeryModInt<T> x, MontgomeryModInt<T> y)[plots.Length];
                for (int i = 0; i < plots.Length; i++)
                    modPlots[i] = (plots[i].x, plots[i].y);

                var fps = LagrangeInterpolation.Coefficient(modPlots);
                fps._cs.Should().HaveCount(plots.Length);
                foreach (var (x, y) in modPlots)
                    fps.Eval(x).Should().Be(y);
            }
        }

        [Fact]
        public void Eval()
        {
            RunTest<Mod998244353>(new[] { 1, 0, 2, 0, 3, 0, 4 }, 100);
            RunTest<Mod998244353>(new[] { 1, 0, 2, 0, 3, 0, 4 }, 1000);
            RunTest<Mod998244353>(new[] { 1, 0, 2, 0, 3, 0, 4 }, 10000);

            RunTest<Mod1000000007>(new[] { 1, 0, 2, 0, 3, 0, 4 }, 100);
            RunTest<Mod1000000007>(new[] { 1, 0, 2, 0, 3, 0, 4 }, 1000);
            RunTest<Mod1000000007>(new[] { 1, 0, 2, 0, 3, 0, 4 }, 10000);

            static void RunTest<T>(int[] y, long x) where T : struct, IStaticMod
            {
                var modY = new MontgomeryModInt<T>[y.Length];
                var modPlots = new (MontgomeryModInt<T> x, MontgomeryModInt<T> y)[y.Length];
                for (int i = 0; i < y.Length; i++)
                {
                    modY[i] = y[i];
                    modPlots[i] = (i, y[i]);
                }

                var fps = LagrangeInterpolation.Coefficient(modPlots);
                LagrangeInterpolation.Eval<T>(modY, x).Should().Be(fps.Eval(x));
            }
        }
    }
}
