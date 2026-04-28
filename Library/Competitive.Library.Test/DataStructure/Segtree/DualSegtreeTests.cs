using AtCoder.Internal;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod1000000007>;


namespace Kzrnm.Competitive.Testing.DataStructure;

public class DualSegtreeTests
{
    private readonly struct Multiply : IDualSegtreeOperator<ModInt>
    {
        public ModInt FIdentity => ModInt.One;

        public ModInt Composition(ModInt f, ModInt g) => f * g;
    }

    [Test, MultipleAssertions]
    public async Task Zero()
    {
        await new DualSegtree<ModInt, Multiply>(0).d.Should().BeEquivalentOrderTo(Enumerable.Repeat(ModInt.One, 2));
        await new DualSegtree<ModInt, Multiply>(20).d.Should().BeEquivalentOrderTo(Enumerable.Repeat(ModInt.One, 64));
    }

    [Test, MultipleAssertions]
    public async Task Invalid()
    {
        var s = new DualSegtree<ModInt, Multiply>(10);
        await Assert.That(() => s[-1]).Throws<ContractAssertException>();
        await Assert.That(() => s[10]).Throws<ContractAssertException>();
        await Assert.That(() => s[0]).ThrowsNothing();
        await Assert.That(() => s[9]).ThrowsNothing();
    }

    [Test, MultipleAssertions]
    public async Task NaiveProd()
    {
        for (int n = 0; n <= 50; n++)
        {
            var seg = new DualSegtree<ModInt, Multiply>(n);
            var p = new ModInt[n];
            for (int i = 0; i < n; i++)
            {
                p[i] = (i * i + 100) % 31;
                seg[i] = p[i];
            }
            for (int l = 0; l <= n; l++)
            {
                for (int r = l; r <= n; r++)
                {
                    seg.Apply(l, r, l * l - 2);
                    for (int i = l; i < r; i++)
                        p[i] *= l * l - 2;

                    for (int i = 0; i < p.Length; i++)
                    {
                        await seg[i].Should().BeEqualTo(p[i]);
                    }
                }
            }
        }
    }

    [Test, MultipleAssertions]
    public async Task ToArray()
    {
        for (int n = 0; n <= 50; n++)
        {
            var seg = new DualSegtree<ModInt, Multiply>(n);
            var p = new ModInt[n];
            for (int i = 0; i < n; i++)
            {
                p[i] = (i * i + 100) % 31;
                seg[i] = p[i];
            }
            for (int l = 0; l <= n; l++)
            {
                for (int r = l; r <= n; r++)
                {
                    seg.Apply(l, r, l * l - 2);
                    for (int i = l; i < r; i++)
                        p[i] *= l * l - 2;

                    for (int i = 0; i < p.Length; i++)
                    {
                        await seg[i].Should().BeEqualTo(p[i]);
                    }
                }
            }
        }
    }

    [Test, MultipleAssertions]
    public async Task Usage()
    {
        var seg = new DualSegtree<ModInt, Multiply>([1, 2, 3]);
        await seg.d.Should().BeEquivalentOrderTo([(ModInt)1, 1, 1, 1, 1, 2, 3, 1]);

        seg[0] = 4;
        await seg.d.Should().BeEquivalentOrderTo([(ModInt)1, 1, 1, 1, 4, 2, 3, 1]);

        seg.Apply(0, 2);
        await seg.d.Should().BeEquivalentOrderTo([(ModInt)1, 1, 1, 1, 8, 2, 3, 1]);

        seg.Apply(0, 1, 2);
        await seg.d.Should().BeEquivalentOrderTo([(ModInt)1, 1, 1, 1, 16, 2, 3, 1]);

        seg.Apply(0, 2, 2);
        await seg.d.Should().BeEquivalentOrderTo([(ModInt)1, 1, 2, 1, 16, 2, 3, 1]);

        seg.Apply(0, 3, 2);
        await seg.d.Should().BeEquivalentOrderTo([(ModInt)1, 1, 4, 1, 16, 2, 6, 1]);

        await seg.ToArray().Should().BeEquivalentOrderTo([(ModInt)64, 8, 6]);
        await seg.d.Should().BeEquivalentOrderTo([(ModInt)1, 1, 1, 1, 64, 8, 6, 1]);
    }
}