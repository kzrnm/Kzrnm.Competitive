using FluentAssertions;
using System;
using Xunit;


namespace Kzrnm.Competitive.Testing.Graph
{
    public class PartiallyPersistentUnionFindTests
    {
        [Fact]
        public void Simple()
        {
            var uf = new PartiallyPersistentUnionFind(2);
            uf.Merge(0, 1).Should().BeTrue();
            uf.Same(0, 1, 0).Should().BeFalse();
            uf.Same(0, 1, 1).Should().BeTrue();
            uf.Size(0, 1).Should().Be(2);
        }

        [Fact]
        public void Line()
        {
            int n = 10000;
            var uf = new PartiallyPersistentUnionFind(n);
            for (int i = 0; i < n - 1; i++)
            {
                uf.Merge(i, i + 1);
            }
            for (int i = 0; i < n - 1; i++)
            {
                uf.Size(0, i).Should().Be(i + 1);
                uf.Size(i, i).Should().Be(i + 1);
            }
            uf.Version.Should().Be(n - 1);
            uf.Size(0, uf.Version).Should().Be(n);
        }

        [Fact]
        public void LineReverse()
        {
            int n = 50000;
            var uf = new PartiallyPersistentUnionFind(n);
            for (int i = n - 2; i >= 0; i--)
            {
                uf.Merge(i, i + 1);
            }
            uf.Version.Should().Be(n - 1);
            uf.Size(0, uf.Version).Should().Be(n);
        }
    }
}
