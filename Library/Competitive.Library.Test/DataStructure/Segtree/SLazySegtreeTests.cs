using AtCoder.Internal;


namespace Kzrnm.Competitive.Testing.DataStructure;

public class SLazySegtreeTest
{
    private readonly struct Starry : ISLazySegtreeOperator<int, int>
    {
        public int Identity => -1_000_000_000;
        public int FIdentity => 0;

        public int Composition(int f, int g) => f + g;

        public int Mapping(int f, int x, int size) => f + x;

        public int Operate(int x, int y) => Math.Max(x, y);
    }

    [Test, MultipleAssertions]
    public async Task Zero()
    {
        await new SLazySegtree<int, int, Starry>(0).AllProd.Should().BeEqualTo(-1_000_000_000);
        await new SLazySegtree<int, int, Starry>(10).AllProd.Should().BeEqualTo(-1_000_000_000);
    }

    [Test, MultipleAssertions]
    public async Task Invalid()
    {
        var s = new SLazySegtree<int, int, Starry>(10);
        await Assert.That(() => s[-1]).Throws<ContractAssertException>();
        await Assert.That(() => s[10]).Throws<ContractAssertException>();
        await Assert.That(() => s[0]).ThrowsNothing();
        await Assert.That(() => s[9]).ThrowsNothing();

        await Assert.That(() => s.Prod(-1, -1)).Throws<ContractAssertException>();
        await Assert.That(() => s.Prod(3, 2)).Throws<ContractAssertException>();
        await Assert.That(() => s.Prod(0, 11)).Throws<ContractAssertException>();
        await Assert.That(() => s.Prod(-1, 11)).Throws<ContractAssertException>();
        await Assert.That(() => s.Prod(0, 0)).ThrowsNothing();
        await Assert.That(() => s.Prod(10, 10)).ThrowsNothing();
        await Assert.That(() => s.Prod(0, 10)).ThrowsNothing();

        await Assert.That(() => s.MaxRight(11, s => true)).Throws<ContractAssertException>();
        await Assert.That(() => s.MaxRight(-1, s => true)).Throws<ContractAssertException>();
        await Assert.That(() => s.MaxRight(0, s => false)).Throws<ContractAssertException>();
        await Assert.That(() => s.MaxRight(0, s => true)).ThrowsNothing();
        await Assert.That(() => s.MaxRight(10, s => true)).ThrowsNothing();

        await Assert.That(() => s.MinLeft(11, s => true)).Throws<ContractAssertException>();
        await Assert.That(() => s.MinLeft(-1, s => true)).Throws<ContractAssertException>();
        await Assert.That(() => s.MinLeft(0, s => false)).Throws<ContractAssertException>();
        await Assert.That(() => s.MinLeft(0, s => true)).ThrowsNothing();
        await Assert.That(() => s.MinLeft(10, s => true)).ThrowsNothing();
    }

    [Test, MultipleAssertions]
    public async Task NaiveProd()
    {
        for (int n = 0; n <= 50; n++)
        {
            var seg = new SLazySegtree<int, int, Starry>(n);
            var p = new int[n];
            for (int i = 0; i < n; i++)
            {
                p[i] = (i * i + 100) % 31;
                seg[i] = p[i];
            }
            for (int l = 0; l <= n; l++)
            {
                for (int r = l; r <= n; r++)
                {
                    int e = -1_000_000_000;
                    for (int i = l; i < r; i++)
                    {
                        e = Math.Max(e, p[i]);
                    }
                    await seg.Prod(l, r).Should().BeEqualTo(e);
                    await seg[l..r].Should().BeEqualTo(e);
                }
            }
        }
    }

    [Test, MultipleAssertions]
    public async Task Usage()
    {
        var seg = new SLazySegtree<int, int, Starry>(new int[10]);
        await seg.AllProd.Should().BeEqualTo(0);
        seg.Apply(0, 3, 5);
        await seg.AllProd.Should().BeEqualTo(5);
        seg.Apply(2, -10);
        await seg.Prod(2, 3).Should().BeEqualTo(-5);
        await seg[2..3].Should().BeEqualTo(-5);
        await seg.Prod(2, 4).Should().BeEqualTo(0);
        await seg[2..4].Should().BeEqualTo(0);
    }


    private readonly struct SumOp : ISLazySegtreeOperator<int, Guid>
    {
        public int Identity => 0;
        public Guid FIdentity => Guid.Empty;
        public Guid Composition(Guid nf, Guid cf) => Guid.Empty;
        public int Mapping(Guid f, int x, int size)
        {
            if (x != size)
                throw new InvalidOperationException();
            return x;
        }
        public int Operate(int x, int y) => x + y;
    }

    [Test, MultipleAssertions]
    public async Task Size()
    {
        var seg = new SLazySegtree<int, Guid, SumOp>(Enumerable.Repeat(1, 35).ToArray());
        for (int l = 0; l < seg.Length; l++)
            for (int r = l; r <= seg.Length; r++)
            {
                seg.Apply(l, r, Guid.Empty);
                await seg[l..r].Should().BeEqualTo(r - l);
            }
    }

    [Test, MultipleAssertions]
    public async Task ToArray()
    {
        for (int i = 0; i < 20; i++)
        {
            var seg = new SLazySegtree<int, int, Starry>(new int[i]);
            for (int j = 0; j < seg.Length - j; j++)
                seg.Apply(j, seg.Length - j, 1);
            await seg.ToArray().Should().BeEquivalentOrderTo(CreateExpected(i));
        }

        static int[] CreateExpected(int length)
        {
            var result = new int[length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Min(i + 1, length - i);
            }
            return result;
        }
    }
}