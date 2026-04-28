using AtCoder;
using AtCoder.Internal;

namespace Kzrnm.Competitive.Testing.DataStructure;

public class 永続SegtreeTests
{
    [Test, MultipleAssertions]
    public async Task Invalid()
    {
        var s = new PersistentSegtree<string, MonoidOperator>(10);
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
    }

    [Test, MultipleAssertions]
    public async Task One()
    {
        var s = new PersistentSegtree<string, MonoidOperator>(1);
        await s.AllProd.Should().BeEqualTo("$");
        await s[0].Should().BeEqualTo("$");
        await s.Prod(0, 1).Should().BeEqualTo("$");
        await s[0..1].Should().BeEqualTo("$");
        var ns = s.SetItem(0, "dummy");
        await s.AllProd.Should().BeEqualTo("$");
        await s[0].Should().BeEqualTo("$");
        await s.Prod(0, 1).Should().BeEqualTo("$");
        await s[0..1].Should().BeEqualTo("$");
        await ns[0].Should().BeEqualTo("dummy");
        await ns.Prod(0, 0).Should().BeEqualTo("$");
        await ns.Prod(0, 1).Should().BeEqualTo("dummy");
        await ns.Prod(1, 1).Should().BeEqualTo("$");
        await ns[0..0].Should().BeEqualTo("$");
        await ns[0..1].Should().BeEqualTo("dummy");
        await ns[1..1].Should().BeEqualTo("$");
    }

    [Test, MultipleAssertions]
    public async Task CompareNaive()
    {
        for (int n = 1; n < 30; n++)
        {
            var seg0 = new SegtreeNaive(n);
            var seg1 = new PersistentSegtree<string, MonoidOperator>(n);

            for (int i = 0; i < n; i++)
            {
                var s = "";
                s += (char)('a' + i);
                seg0[i] = s;
                seg1 = seg1.SetItem(i, s);
            }

            for (int l = 0; l <= n; l++)
            {
                for (int r = l; r <= n; r++)
                {
                    await seg1.Prod(l, r).Should().BeEqualTo(seg0.Prod(l, r));
                    await seg1[l..r].Should().BeEqualTo(seg0.Prod(l, r));
                }
            }

            //for (int l = 0; l <= n; l++)
            //{
            //    for (int r = l; r <= n; r++)
            //    {
            //        var y = seg1.Prod(l, r);
            //        seg1.MaxRight(l, x => x.Length <= y.Length).Should().BeEqualTo(seg0.MaxRight(l, x => x.Length <= y.Length));
            //    }
            //}


            //for (int r = 0; r <= n; r++)
            //{
            //    for (int l = 0; l <= r; l++)
            //    {
            //        var y = seg1.Prod(l, r);
            //        seg1.MinLeft(l, x => x.Length <= y.Length).Should().BeEqualTo(seg0.MinLeft(l, x => x.Length <= y.Length));
            //    }
            //}
        }
    }

    readonly struct MonoidOperator : ISegtreeOperator<string>
    {
        public string Identity => "$";
        public string Operate(string a, string b)
        {
            if (!(a == "$" || b == "$" || StringComparer.Ordinal.Compare(a, b) <= 0)) throw new Exception();
            if (a == "$") return b;
            if (b == "$") return a;
            return a + b;
        }
    }
    class SegtreeNaive
    {
        private static readonly MonoidOperator op = default;
        int n;
        string[] d;

        public SegtreeNaive(int _n)
        {
            n = _n;
            d = new string[n];
            Array.Fill(d, op.Identity);
        }
        public string this[int p]
        {
            set => d[p] = value;
            get => d[p];
        }

        public string Prod(int l, int r)
        {
            var sum = op.Identity;
            for (int i = l; i < r; i++)
            {
                sum = op.Operate(sum, d[i]);
            }
            return sum;
        }
        public string AllProd => Prod(0, n);
        public int MaxRight(int l, Predicate<string> f)
        {
            var sum = op.Identity;
            if (!f(sum))
                throw new InvalidOperationException();
            for (int i = l; i < n; i++)
            {
                sum = op.Operate(sum, d[i]);
                if (!f(sum)) return i;
            }
            return n;
        }
        public int MinLeft(int r, Predicate<string> f)
        {
            var sum = op.Identity;
            if (!f(sum))
                throw new InvalidOperationException();
            for (int i = r - 1; i >= 0; i--)
            {
                sum = op.Operate(d[i], sum);
                if (!f(sum)) return i + 1;
            }
            return 0;
        }
    }
}