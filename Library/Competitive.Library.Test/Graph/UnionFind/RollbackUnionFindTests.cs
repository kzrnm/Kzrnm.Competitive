namespace Kzrnm.Competitive.Testing.Graph
{
    public class RollbackUnionFindTests
    {
        [Fact]
        public void Simple()
        {
            var uf = new RollbackUnionFind(8);
            uf.Same(0, 1).Should().BeFalse();
            uf.Merge(0, 1).Should().BeTrue();
            uf.Same(0, 1).Should().BeTrue();
            uf.Size(0).Should().Be(2);

            uf.Undo();
            uf.Same(0, 1).Should().BeFalse();
            uf.Size(0).Should().Be(1);

            uf.Merge(0, 1).Should().BeTrue();
            uf.Merge(0, 2).Should().BeTrue();
            uf.Size(0).Should().Be(3);
            uf.Snapshot();
            uf.Merge(0, 3).Should().BeTrue();
            uf.Merge(0, 4).Should().BeTrue();
            var version = uf.Version;
            uf.Merge(7, 6).Should().BeTrue();
            uf.Merge(7, 5).Should().BeTrue();
            uf.Merge(0, 7);

            uf.Size(0).Should().Be(8);
            uf.Rollback(version);
            uf.Size(0).Should().Be(5);

            uf.Rollback();
            uf.Size(0).Should().Be(3);
        }

        [Fact]
        public void Line()
        {
            int n = 10000;
            var uf = new RollbackUnionFind(n);
            for (int i = 0; i < n - 1; i++)
            {
                uf.Size(0).Should().Be(i + 1);
                uf.Merge(i, i + 1);
                uf.Size(0).Should().Be(i + 2);
                uf.Version.Should().Be(i + 1);
            }
            for (int i = n - 2; i >= 0; i--)
            {
                uf.Undo();
                uf.Size(0).Should().Be(i + 1);
                uf.Version.Should().Be(i);
            }
            uf.Version.Should().Be(0);
        }
    }
}
