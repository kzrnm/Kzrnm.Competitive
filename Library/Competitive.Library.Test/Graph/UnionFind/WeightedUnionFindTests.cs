using System;


namespace Kzrnm.Competitive.Testing.Graph
{
    public class WeightedUnionFindTests
    {
        [Fact]
        public void Zero()
        {
            var uf = new IntWeightedUnionFind(0);
            uf.Groups().ShouldBe([]);
        }

        [Fact]
        public void Simple()
        {
            var uf = new IntWeightedUnionFind(2);
            uf.Same(0, 1).ShouldBeFalse();
            uf.Merge(0, 1, 5).ShouldBeTrue();
            uf.Same(0, 1).ShouldBeTrue();
            uf.Size(0).ShouldBe(2);
            uf.WeightDiff(0, 1).ShouldBe(5);
            uf.WeightDiff(1, 0).ShouldBe(-5);
        }

        [Fact]
        public void Line()
        {
            int n = 10000;
            var uf = new IntWeightedUnionFind(n);
            for (int i = 0; i < n - 1; i++)
            {
                uf.Merge(i, i + 1, 1);
            }
            for (int i = 0; i < n; i++)
            {
                uf.WeightDiff(0, i).ShouldBe(i);
                uf.WeightDiff(10, i).ShouldBe(i - 10);
            }
            uf.Size(0).ShouldBe(n);
            uf.Groups().Length.ShouldBe(1);
        }

        [Fact]
        public void LineReverse()
        {
            int n = 10000;
            var uf = new IntWeightedUnionFind(n);
            for (int i = n - 2; i >= 0; i--)
            {
                uf.Merge(i, i + 1, 1);
            }
            for (int i = 0; i < n; i++)
            {
                uf.WeightDiff(0, i).ShouldBe(i);
                uf.WeightDiff(10, i).ShouldBe(i - 10);
            }
            uf.Size(0).ShouldBe(n);
            uf.Groups().Length.ShouldBe(1);
        }
    }
}
