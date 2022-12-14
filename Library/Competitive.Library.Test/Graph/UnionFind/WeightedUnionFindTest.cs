using System;


namespace Kzrnm.Competitive.Testing.Graph
{
    public class WeightedUnionFindTests
    {
        [Fact]
        public void Zero()
        {
            var uf = new IntWeightedUnionFind(0);
            uf.Groups().Should().Equal(Array.Empty<int[]>());
        }

        [Fact]
        public void Simple()
        {
            var uf = new IntWeightedUnionFind(2);
            uf.Same(0, 1).Should().BeFalse();
            uf.Merge(0, 1, 5).Should().BeTrue();
            uf.Same(0, 1).Should().BeTrue();
            uf.Size(0).Should().Be(2);
            uf.WeightDiff(0, 1).Should().Be(5);
            uf.WeightDiff(1, 0).Should().Be(-5);
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
                uf.WeightDiff(0, i).Should().Be(i);
                uf.WeightDiff(10, i).Should().Be(i - 10);
            }
            uf.Size(0).Should().Be(n);
            uf.Groups().Should().HaveCount(1);
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
                uf.WeightDiff(0, i).Should().Be(i);
                uf.WeightDiff(10, i).Should().Be(i - 10);
            }
            uf.Size(0).Should().Be(n);
            uf.Groups().Should().HaveCount(1);
        }
    }
}
