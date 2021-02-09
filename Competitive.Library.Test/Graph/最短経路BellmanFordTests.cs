using FluentAssertions;
using System;
using Xunit;

namespace Kzrnm.Competitive.Graph
{
    public class 最短経路BellmanFordTests
    {
        [Fact]
        public void Int()
        {
            var gb = new WIntGraphBuilder(5, true);
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
            graph.BellmanFord(0).Should().Equal(new int[] { 0, 1, 6, 18, 12 });
            graph.BellmanFord(1).Should().Equal(new int[] { 12, 0, 5, 17, 11 });
            graph.BellmanFord(2).Should().Equal(new int[] { 7, 8, 0, 12, 6 });
            graph.BellmanFord(3).Should().Equal(new int[] { 1073741823, 1073741823, 1073741823, 0, 1073741823 });
            graph.BellmanFord(4).Should().Equal(new int[] { 1, 2, 7, 6, 0 });
        }

        [Fact]
        public void Long()
        {
            var gb = new WLongGraphBuilder(5, true);
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
            graph.BellmanFord(0).Should().Equal(new long[] { 0, 1, 6, 18, 12 });
            graph.BellmanFord(1).Should().Equal(new long[] { 12, 0, 5, 17, 11 });
            graph.BellmanFord(2).Should().Equal(new long[] { 7, 8, 0, 12, 6 });
            graph.BellmanFord(3).Should().Equal(new long[] { 4611686018427387903, 4611686018427387903, 4611686018427387903, 0, 4611686018427387903 });
            graph.BellmanFord(4).Should().Equal(new long[] { 1, 2, 7, 6, 0 });
        }

        [Fact]
        public void NegativeLength()
        {
            var gb = new WLongGraphBuilder(5, true);
            gb.Add(0, 1, 1);
            gb.Add(0, 2, 10);
            gb.Add(0, 3, 30);
            gb.Add(0, 4, 40);
            gb.Add(1, 2, 5);
            gb.Add(2, 3, -605);
            gb.Add(2, 4, 6);
            gb.Add(4, 3, 6);
            gb.Add(4, 0, 1);
            var graph = gb.ToGraph();
            graph.BellmanFord(0).Should().Equal(new long[] { 0, 1, 6, -599, 12 });
            graph.BellmanFord(1).Should().Equal(new long[] { 12, 0, 5, -600, 11 });
            graph.BellmanFord(2).Should().Equal(new long[] { 7, 8, 0, -605, 6 });
            graph.BellmanFord(3).Should().Equal(new long[] { 4611686018427387903, 4611686018427387903, 4611686018427387903, 0, 4611686018427387903 });
            graph.BellmanFord(4).Should().Equal(new long[] { 1, 2, 7, -598, 0 });
        }

        [Fact]
        public void NegativeCycle()
        {
            var gb = new WLongGraphBuilder(5, true);
            gb.Add(0, 1, 1);
            gb.Add(0, 2, 10);
            gb.Add(0, 3, 30);
            gb.Add(0, 4, 40);
            gb.Add(1, 2, -5);
            gb.Add(2, 3, -605);
            gb.Add(2, 4, -6);
            gb.Add(4, 3, 6);
            gb.Add(4, 0, -1);
            var graph = gb.ToGraph();
            graph.Invoking(graph => graph.BellmanFord(0)).Should().Throw<InvalidOperationException>();
            graph.Invoking(graph => graph.BellmanFord(1)).Should().Throw<InvalidOperationException>();
            graph.Invoking(graph => graph.BellmanFord(2)).Should().Throw<InvalidOperationException>();
            graph.Invoking(graph => graph.BellmanFord(3)).Should().Throw<InvalidOperationException>();
            graph.Invoking(graph => graph.BellmanFord(4)).Should().Throw<InvalidOperationException>();
        }
    }
}
