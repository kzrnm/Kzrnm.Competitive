using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.Graph
{
    // verification-helper: SAMEAS Library/run.test.py
    public class 閉路検出DFSTests
    {
        [Fact]
        public void 重みなしグラフ()
        {
            var gb = new GraphBuilder(8, true);
            gb.Add(0, 1);
            gb.Add(1, 2);
            gb.Add(2, 3);
            gb.Add(3, 4);
            gb.Add(4, 5);
            gb.Add(5, 6);
            gb.Add(4, 7);
            gb.Add(7, 3);
            var (from, edges) = gb.ToGraph().GetCycleDFS();
            from.Should().Be(3);
            edges.Should().Equal(new GraphEdge[] {
                new GraphEdge(4),
                new GraphEdge(7),
                new GraphEdge(3),
            });
        }
        [Fact]
        public void 重み付きグラフ()
        {
            var gb = new WIntGraphBuilder(8, true);
            gb.Add(0, 1, 1);
            gb.Add(1, 2, 2);
            gb.Add(2, 3, 3);
            gb.Add(3, 4, 4);
            gb.Add(4, 5, 5);
            gb.Add(5, 6, 6);
            gb.Add(4, 7, 7);
            gb.Add(7, 3, 8);
            var (from, edges) = gb.ToGraph().GetCycleDFS();
            from.Should().Be(3);
            edges.Should().Equal(new WEdge<int>[] {
                new WEdge<int>(4, 4),
                new WEdge<int>(7, 7),
                new WEdge<int>(3, 8),
            });
        }
    }
}
