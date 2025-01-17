using AtCoder.Internal;
using System;
using System.Linq;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod1000000007>;


namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class DualSegtreeTests
    {
        private readonly struct Multiply : IDualSegtreeOperator<ModInt>
        {
            public ModInt FIdentity => ModInt.One;

            public ModInt Composition(ModInt f, ModInt g) => f * g;
        }

        [Fact]
        public void Zero()
        {
            new DualSegtree<ModInt, Multiply>(0).d.ShouldBe(Enumerable.Repeat(ModInt.One, 2));
            new DualSegtree<ModInt, Multiply>(20).d.ShouldBe(Enumerable.Repeat(ModInt.One, 64));
        }

        [Fact]
        public void Invalid()
        {
            var s = new DualSegtree<ModInt, Multiply>(10);
            Should.Throw<ContractAssertException>(() => s[-1]);
            Should.Throw<ContractAssertException>(() => s[10]);
            Should.NotThrow(() => s[0]);
            Should.NotThrow(() => s[9]);
        }

        [Fact]
        public void NaiveProd()
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
                            seg[i].ShouldBe(p[i]);
                        }
                    }
                }
            }
        }

        [Fact]
        public void ToArray()
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
                            seg[i].ShouldBe(p[i]);
                        }
                    }
                }
            }
        }

        [Fact]
        public void Usage()
        {
            var seg = new DualSegtree<ModInt, Multiply>([1, 2, 3]);
            seg.d.ShouldBe([1, 1, 1, 1, 1, 2, 3, 1]);

            seg[0] = 4;
            seg.d.ShouldBe([1, 1, 1, 1, 4, 2, 3, 1]);

            seg.Apply(0, 2);
            seg.d.ShouldBe([1, 1, 1, 1, 8, 2, 3, 1]);

            seg.Apply(0, 1, 2);
            seg.d.ShouldBe([1, 1, 1, 1, 16, 2, 3, 1]);

            seg.Apply(0, 2, 2);
            seg.d.ShouldBe([1, 1, 2, 1, 16, 2, 3, 1]);

            seg.Apply(0, 3, 2);
            seg.d.ShouldBe([1, 1, 4, 1, 16, 2, 6, 1]);

            seg.ToArray().ShouldBe([64, 8, 6]);
            seg.d.ShouldBe([1, 1, 1, 1, 64, 8, 6, 1]);
        }
    }
}