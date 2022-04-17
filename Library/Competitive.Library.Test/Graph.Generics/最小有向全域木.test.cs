using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.Graph
{
    // verification-helper: EXTERNAL_FAILURE_FLAG unittest_failure
    public class 最小有向全域木Tests
    {
        [Fact]
        public void Int()
        {
            var gb = new WIntGraphBuilder(5, true);
            gb.Add(0, 1, 442);
            gb.Add(0, 2, 104);
            gb.Add(0, 3, 16);
            gb.Add(0, 4, 5524);
            gb.Add(0, 4, 423);
            gb.Add(4, 0, 14214);
            gb.Add(1, 4, 44223);
            gb.Add(3, 4, 5024224);
            gb.Add(2, 3, 42110);
            gb.Add(4, 1, 4424223);
            var graph = gb.ToGraph();
            var mst = graph.DirectedMinimumSpanningTree(0);
            mst.Edges.Should().Equal(
                (0, new WEdge<int>(4, 423)),
                (0, new WEdge<int>(3, 16)),
                (0, new WEdge<int>(2, 104)),
                (0, new WEdge<int>(1, 442)));
            mst.Cost.Should().Be(423 + 16 + 104 + 442);

            mst = graph.DirectedMinimumSpanningTree(3);
            mst.Edges.Should().Equal(
                (3, new WEdge<int>(4, 5024224)),
                (0, new WEdge<int>(2, 104)),
                (0, new WEdge<int>(1, 442)),
                (4, new WEdge<int>(0, 14214)));
            mst.Cost.Should().Be(5024224 + 104 + 442 + 14214);
        }

        [Fact]
        public void Long()
        {
            var gb = new WLongGraphBuilder(5, true);
            gb.Add(0, 1, 442);
            gb.Add(0, 2, 104);
            gb.Add(0, 3, 16);
            gb.Add(0, 4, 5524);
            gb.Add(0, 4, 423);
            gb.Add(4, 0, 14214);
            gb.Add(1, 4, 44223);
            gb.Add(3, 4, 5024224);
            gb.Add(2, 3, 42110);
            gb.Add(4, 1, 4424223);
            var graph = gb.ToGraph();
            var mst = graph.DirectedMinimumSpanningTree(0);
            mst.Edges.Should().Equal(
                (0, new WEdge<long>(4, 423)),
                (0, new WEdge<long>(3, 16)),
                (0, new WEdge<long>(2, 104)),
                (0, new WEdge<long>(1, 442)));
            mst.Cost.Should().Be(423 + 16 + 104 + 442);

            mst = graph.DirectedMinimumSpanningTree(3);
            mst.Edges.Should().Equal(
                (3, new WEdge<long>(4, 5024224)),
                (0, new WEdge<long>(2, 104)),
                (0, new WEdge<long>(1, 442)),
                (4, new WEdge<long>(0, 14214)));
            mst.Cost.Should().Be(5024224 + 104 + 442 + 14214);
        }
    }
}
