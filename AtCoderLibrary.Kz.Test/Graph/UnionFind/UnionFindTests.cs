using FluentAssertions;
using System;
using Xunit;


namespace AtCoder.Graph
{
    public class UnionFindTests
    {
        [Fact]
        public void Zero()
        {
            var uf = new UnionFind(0);
            uf.Groups().Should().Equal(Array.Empty<int[]>());
        }

        [Fact]
        public void Simple()
        {
            var uf = new UnionFind(2);
            uf.Same(0, 1).Should().BeFalse();
            uf.Merge(0, 1).Should().BeTrue();
            uf.Same(0, 1).Should().BeTrue();
            uf.Size(0).Should().Be(2);
        }

        [Fact]
        public void Line()
        {
            int n = 500000;
            var uf = new UnionFind(n);
            for (int i = 0; i < n - 1; i++)
            {
                uf.Merge(i, i + 1);
            }
            uf.Size(0).Should().Be(n);
            uf.Groups().Should().HaveCount(1);
        }

        [Fact]
        public void LineReverse()
        {
            int n = 500000;
            var uf = new UnionFind(n);
            for (int i = n - 2; i >= 0; i--)
            {
                uf.Merge(i, i + 1);
            }
            uf.Size(0).Should().Be(n);
            uf.Groups().Should().HaveCount(1);
        }
    }
}
