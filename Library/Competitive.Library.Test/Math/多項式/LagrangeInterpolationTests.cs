using AtCoder;

namespace Kzrnm.Competitive.Testing.MathNS;

public class LagrangeInterpolationTest
{
    public static IEnumerable<(int x, int y)[]> Coefficient_Data =>
    [
        [
            (0,1),
            (1,10),
        ],
        [
            (0,1),
            (1,10),
            (2,11),
        ],
        [
            (0,1),
            (1,10),
            (2,11),
            (3,-51),
        ]
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Coefficient_Data))]
    public async Task Coefficient((int x, int y)[] data)
    {
        await RunTest<Mod998244353>(data);
        await RunTest<Mod1000000007>(data);

        static async Task RunTest<T>((int x, int y)[] plots) where T : struct, IStaticMod
        {
            var modPlots = new (MontgomeryModInt<T> x, MontgomeryModInt<T> y)[plots.Length];
            for (int i = 0; i < plots.Length; i++)
                modPlots[i] = (plots[i].x, plots[i].y);

            var fps = LagrangeInterpolation.Coefficient(modPlots);
            await fps._cs.Length.Should().BeEqualTo(plots.Length);
            foreach (var (x, y) in modPlots)
                await fps.Eval(x).Should().BeEqualTo(y);
        }
    }

    [Test, MultipleAssertions]
    public async Task Eval()
    {
        await RunTest<Mod998244353>([1, 0, 2, 0, 3, 0, 4], 100);
        await RunTest<Mod998244353>([1, 0, 2, 0, 3, 0, 4], 1000);
        await RunTest<Mod998244353>([1, 0, 2, 0, 3, 0, 4], 10000);

        await RunTest<Mod1000000007>([1, 0, 2, 0, 3, 0, 4], 100);
        await RunTest<Mod1000000007>([1, 0, 2, 0, 3, 0, 4], 1000);
        await RunTest<Mod1000000007>([1, 0, 2, 0, 3, 0, 4], 10000);

        static async Task RunTest<T>(int[] y, long x) where T : struct, IStaticMod
        {
            var modY = new MontgomeryModInt<T>[y.Length];
            var modPlots = new (MontgomeryModInt<T> x, MontgomeryModInt<T> y)[y.Length];
            for (int i = 0; i < y.Length; i++)
            {
                modY[i] = y[i];
                modPlots[i] = (i, y[i]);
            }

            var fps = LagrangeInterpolation.Coefficient(modPlots);
            await LagrangeInterpolation.Eval<T>(modY, x).Should().BeEqualTo(fps.Eval(x));
        }
    }
}