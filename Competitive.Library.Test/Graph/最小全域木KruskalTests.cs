﻿using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Graph
{
    public class 最小全域木KruskalTests
    {
        [Fact]
        public void 重みなしグラフ()
        {
            var gb = new GraphBuilder(5, false);
            gb.Add(0, 1);
            gb.Add(0, 2);
            gb.Add(0, 3);
            gb.Add(0, 4);
            gb.Add(1, 2);
            gb.Add(2, 3);
            gb.Add(2, 4);
            gb.Add(4, 3);
            gb.Add(4, 0);
            var graph = gb.ToGraph();
            var res = graph.Kruskal();
            res.Should().HaveCount(1);
            res[0].Should().Equal(
                (0, new Edge(1)),
                (0, new Edge(2)),
                (0, new Edge(3)),
                (0, new Edge(4)));
        }

        [Fact]
        public void 連結ではない重みなし()
        {
            var gb = new GraphBuilder(8, false);
            gb.Add(0, 1);
            gb.Add(0, 2);
            gb.Add(1, 2);
            gb.Add(4, 3);
            gb.Add(4, 7);
            gb.Add(6, 5);
            var graph = gb.ToGraph();
            var res = graph.Kruskal();
            res.Should().HaveCount(3);
            res[0].Should().Equal(
                (0, new Edge(1)),
                (0, new Edge(2)));
            res[1].Should().Equal(
                (3, new Edge(4)),
                (4, new Edge(7)));
            res[2].Should().Equal(
                (5, new Edge(6)));
        }

        [Fact]
        public void 森の連結重みなし()
        {
            var gb = new GraphBuilder(4, false);
            gb.Add(0, 2);
            gb.Add(1, 3);
            gb.Add(2, 3);
            var graph = gb.ToGraph();
            var res = graph.Kruskal();
            res.Should().HaveCount(1);
            res[0].Should().Equal(
                (0, new Edge(2)),
                (1, new Edge(3)),
                (2, new Edge(3)));
        }

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
            var res = graph.Kruskal();
            res.Should().HaveCount(1);
            res[0].Should().Equal(
                (0, new WEdge<int>(1, 1)),
                (0, new WEdge<int>(4, 1)),
                (1, new WEdge<int>(2, 5)),
                (4, new WEdge<int>(3, 6)));
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
            var res = graph.Kruskal();
            res.Should().HaveCount(1);
            res[0].Should().Equal(
                (0, new WEdge<long>(1, 1)),
                (0, new WEdge<long>(4, 1)),
                (1, new WEdge<long>(2, 5)),
                (4, new WEdge<long>(3, 6)));
        }

        [Fact]
        public void 連結ではない重み付き()
        {
            var gb = new WIntGraphBuilder(8, false);
            gb.Add(0, 1, 1);
            gb.Add(0, 2, 2);
            gb.Add(1, 2, 3);
            gb.Add(4, 3, 4);
            gb.Add(4, 7, 5);
            gb.Add(3, 7, 10);
            gb.Add(6, 5, 6);
            var graph = gb.ToGraph();
            var res = graph.Kruskal();
            res.Should().HaveCount(3);
            res[0].Should().Equal(
                (0, new WEdge<int>(1, 1)),
                (0, new WEdge<int>(2, 2)));
            res[1].Should().Equal(
                (3, new WEdge<int>(4, 4)),
                (4, new WEdge<int>(7, 5)));
            res[2].Should().Equal(
                (5, new WEdge<int>(6, 6)));
        }

        [Fact]
        public void 森の連結重み付き()
        {
            var gb = new WIntGraphBuilder(4, false);
            gb.Add(0, 2, 1);
            gb.Add(1, 3, 1);
            gb.Add(2, 3, 10);
            var graph = gb.ToGraph();
            var res = graph.Kruskal();
            res.Should().HaveCount(1);
            res[0].Should().Equal(
                (0, new WEdge<int>(2, 1)),
                (1, new WEdge<int>(3, 1)),
                (2, new WEdge<int>(3, 10)));
        }
    }
}
