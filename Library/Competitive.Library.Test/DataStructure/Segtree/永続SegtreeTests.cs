using AtCoder;
using AtCoder.Internal;
using System;

namespace Kzrnm.Competitive.Testing.DataStructure;

public class 永続SegtreeTests
{
    [Fact]
    public void Invalid()
    {
        var s = new PersistentSegtree<string, MonoidOperator>(10);
        Should.Throw<ContractAssertException>(() => s[-1]);
        Should.Throw<ContractAssertException>(() => s[10]);
        Should.NotThrow(() => s[0]);
        Should.NotThrow(() => s[9]);

        Should.Throw<ContractAssertException>(() => s.Prod(-1, -1));
        Should.Throw<ContractAssertException>(() => s.Prod(3, 2));
        Should.Throw<ContractAssertException>(() => s.Prod(0, 11));
        Should.Throw<ContractAssertException>(() => s.Prod(-1, 11));
        Should.NotThrow(() => s.Prod(0, 0));
        Should.NotThrow(() => s.Prod(10, 10));
        Should.NotThrow(() => s.Prod(0, 10));

        //Should.Throw<ContractAssertException>(() => s.MaxRight(11, s => true));
        //Should.Throw<ContractAssertException>(() => s.MaxRight(-1, s => true));
        //Should.Throw<ContractAssertException>(() => s.MaxRight(0, s => false));
        //Should.NotThrow(() => s.MaxRight(0, s => true));
        //Should.NotThrow(() => s.MaxRight(10, s => true));

        //Should.Throw<ContractAssertException>(() => s.MinLeft(11, s => true));
        //Should.Throw<ContractAssertException>(() => s.MinLeft(-1, s => true));
        //Should.Throw<ContractAssertException>(() => s.MinLeft(0, s => false));
        //Should.NotThrow(() => s.MinLeft(0, s => true));
        //Should.NotThrow(() => s.MinLeft(10, s => true));
    }

    [Fact]
    public void One()
    {
        var s = new PersistentSegtree<string, MonoidOperator>(1);
        s.AllProd.ShouldBe("$");
        s[0].ShouldBe("$");
        s.Prod(0, 1).ShouldBe("$");
        s[0..1].ShouldBe("$");
        var ns = s.SetItem(0, "dummy");
        s.AllProd.ShouldBe("$");
        s[0].ShouldBe("$");
        s.Prod(0, 1).ShouldBe("$");
        s[0..1].ShouldBe("$");
        ns[0].ShouldBe("dummy");
        ns.Prod(0, 0).ShouldBe("$");
        ns.Prod(0, 1).ShouldBe("dummy");
        ns.Prod(1, 1).ShouldBe("$");
        ns[0..0].ShouldBe("$");
        ns[0..1].ShouldBe("dummy");
        ns[1..1].ShouldBe("$");
    }

    [Fact]
    public void CompareNaive()
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
                    seg1.Prod(l, r).ShouldBe(seg0.Prod(l, r));
                    seg1[l..r].ShouldBe(seg0.Prod(l, r));
                }
            }

            //for (int l = 0; l <= n; l++)
            //{
            //    for (int r = l; r <= n; r++)
            //    {
            //        var y = seg1.Prod(l, r);
            //        seg1.MaxRight(l, x => x.Length <= y.Length).ShouldBe(seg0.MaxRight(l, x => x.Length <= y.Length));
            //    }
            //}


            //for (int r = 0; r <= n; r++)
            //{
            //    for (int l = 0; l <= r; l++)
            //    {
            //        var y = seg1.Prod(l, r);
            //        seg1.MinLeft(l, x => x.Length <= y.Length).ShouldBe(seg0.MinLeft(l, x => x.Length <= y.Length));
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
            f(sum).ShouldBeTrue();
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
            f(sum).ShouldBeTrue();
            for (int i = r - 1; i >= 0; i--)
            {
                sum = op.Operate(d[i], sum);
                if (!f(sum)) return i + 1;
            }
            return 0;
        }
    }
}
