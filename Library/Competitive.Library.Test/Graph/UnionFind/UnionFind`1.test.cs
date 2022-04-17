using FluentAssertions;
using System;
using System.Linq;
using Xunit;


namespace Kzrnm.Competitive.Testing.Graph
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
    public class UnionFindDataTests
    {
        [Fact]
        public void Zero()
        {
            var uf = new UnionFind<int>(Array.Empty<int>(), (a, b) => a + b);
            uf.Groups().Should().Equal(Array.Empty<int[]>());
        }

        [Fact]
        public void Simple()
        {
            var uf = new UnionFind<int>(new int[2] { 1, 2 }, (a, b) => a + b);
            uf.Same(0, 1).Should().BeFalse();

            uf.Data(0).Should().Be(1);
            uf.Data(1).Should().Be(2);

            uf.Merge(0, 1).Should().BeTrue();
            uf.Same(0, 1).Should().BeTrue();
            uf.Size(0).Should().Be(2);

            uf.Data(0).Should().Be(3);
            uf.Data(1).Should().Be(3);
        }

        [Fact]
        public void Line()
        {
            int n = 10000;
            var uf = new UnionFind<long>(Enumerable.Range(0, n).Select(a => (long)a).ToArray(), (a, b) => a + b);
            for (int i = 0; i < n - 1; i++)
            {
                uf.Merge(i, i + 1);
            }
            uf.Size(0).Should().Be(n);
            uf.Groups().Should().HaveCount(1);
            uf.Data(0).Should().Be((long)n * (n - 1) / 2);
        }

        [Fact]
        public void LineReverse()
        {
            int n = 10000;
            var uf = new UnionFind<long>(Enumerable.Range(0, n).Select(a => (long)a).ToArray(), (a, b) => a + b);
            for (int i = n - 2; i >= 0; i--)
            {
                uf.Merge(i, i + 1);
            }
            uf.Size(0).Should().Be(n);
            uf.Groups().Should().HaveCount(1);
            uf.Data(0).Should().Be((long)n * (n - 1) / 2);
        }
    }
}
