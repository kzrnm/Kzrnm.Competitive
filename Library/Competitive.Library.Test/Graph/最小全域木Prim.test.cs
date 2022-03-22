using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Testing.Graph
{
    public class 最小全域木PrimTests
    {
        [Fact]
        public void Int()
        {
            var gb = new WIntGraphBuilder(5, false);
            gb.Add(0, 1, 1);
            gb.Add(0, 2, 10);
            gb.Add(0, 3, 30);
            gb.Add(0, 4, 40);
            gb.Add(1, 2, 5);
            gb.Add(2, 3, 605);
            gb.Add(2, 4, 6);
            gb.Add(4, 3, 6);
            gb.Add(4, 0, 1);
            var graph = gb.ToGraph();
            graph.Prim().Should().Equal(
                (0, new WEdge<int>(1, 1)),
                (0, new WEdge<int>(4, 1)),
                (1, new WEdge<int>(2, 5)),
                (4, new WEdge<int>(3, 6)));
        }

        [Fact]
        public void Root()
        {
            var gb = new WIntGraphBuilder(4, false);
            gb.Add(0, 1, 1);
            gb.Add(0, 2, 1);
            gb.Add(1, 2, 1);
            gb.Add(0, 3, 5);
            var graph = gb.ToGraph();
            graph.Prim().Should().Equal(
                (0, new WEdge<int>(1, 1)),
                (0, new WEdge<int>(2, 1)),
                (0, new WEdge<int>(3, 5)));
            graph.Prim(1).Should().Equal(
                (1, new WEdge<int>(0, 1)),
                (1, new WEdge<int>(2, 1)),
                (0, new WEdge<int>(3, 5)));
            graph.Prim(2).Should().Equal(
                (2, new WEdge<int>(0, 1)),
                (2, new WEdge<int>(1, 1)),
                (0, new WEdge<int>(3, 5)));
            graph.Prim(3).Should().Equal(
                (3, new WEdge<int>(0, 5)),
                (0, new WEdge<int>(1, 1)),
                (0, new WEdge<int>(2, 1)));
        }

        [Fact]
        public void Long()
        {
            var gb = new WLongGraphBuilder(5, false);
            gb.Add(0, 1, 1);
            gb.Add(0, 2, 10);
            gb.Add(0, 3, 30);
            gb.Add(0, 4, 40);
            gb.Add(1, 2, 5);
            gb.Add(2, 3, 605);
            gb.Add(2, 4, 6);
            gb.Add(4, 3, 6);
            gb.Add(4, 0, 1);
            var graph = gb.ToGraph();
            graph.Prim().Should().Equal(
                (0, new WEdge<long>(1, 1)),
                (0, new WEdge<long>(4, 1)),
                (1, new WEdge<long>(2, 5)),
                (4, new WEdge<long>(3, 6)));
        }
    }
}
