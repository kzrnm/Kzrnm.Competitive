using System;


namespace Kzrnm.Competitive.Testing.Graph
{
    public class UnionFindTests
    {
        [Fact]
        public void Zero()
        {
            var uf = new UnionFind(0);
            uf.Groups().ShouldBe([]);
        }

        [Fact]
        public void Simple()
        {
            var uf = new UnionFind(2);
            uf.Same(0, 1).ShouldBeFalse();
            uf.Merge(0, 1).ShouldBeTrue();
            uf.Same(0, 1).ShouldBeTrue();
            uf.Size(0).ShouldBe(2);
        }

        [Fact]
        public void Line()
        {
            int n = 10000;
            var uf = new UnionFind(n);
            for (int i = 0; i < n - 1; i++)
            {
                uf.Merge(i, i + 1);
            }
            uf.Size(0).ShouldBe(n);
            uf.Groups().Length.ShouldBe(1);
        }

        [Fact]
        public void LineReverse()
        {
            int n = 10000;
            var uf = new UnionFind(n);
            for (int i = n - 2; i >= 0; i--)
            {
                uf.Merge(i, i + 1);
            }
            uf.Size(0).ShouldBe(n);
            uf.Groups().Length.ShouldBe(1);
        }
    }
}
