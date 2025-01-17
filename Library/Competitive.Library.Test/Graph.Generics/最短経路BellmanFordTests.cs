using System;

namespace Kzrnm.Competitive.Testing.Graph
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
            graph.BellmanFord(0).ShouldBe([0, 1, 6, 18, 12]);
            graph.BellmanFord(1).ShouldBe([12, 0, 5, 17, 11]);
            graph.BellmanFord(2).ShouldBe([7, 8, 0, 12, 6]);
            graph.BellmanFord(3).ShouldBe([1073741823, 1073741823, 1073741823, 0, 1073741823]);
            graph.BellmanFord(4).ShouldBe([1, 2, 7, 6, 0]);
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
            graph.BellmanFord(0).ShouldBe([0, 1, 6, 18, 12]);
            graph.BellmanFord(1).ShouldBe([12, 0, 5, 17, 11]);
            graph.BellmanFord(2).ShouldBe([7, 8, 0, 12, 6]);
            graph.BellmanFord(3).ShouldBe([4611686018427387903, 4611686018427387903, 4611686018427387903, 0, 4611686018427387903]);
            graph.BellmanFord(4).ShouldBe([1, 2, 7, 6, 0]);
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
            graph.BellmanFord(0).ShouldBe([0, 1, 6, -599, 12]);
            graph.BellmanFord(1).ShouldBe([12, 0, 5, -600, 11]);
            graph.BellmanFord(2).ShouldBe([7, 8, 0, -605, 6]);
            graph.BellmanFord(3).ShouldBe([4611686018427387903, 4611686018427387903, 4611686018427387903, 0, 4611686018427387903]);
            graph.BellmanFord(4).ShouldBe([1, 2, 7, -598, 0]);
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
            Should.Throw<InvalidOperationException>(() => graph.BellmanFord(0));
            Should.Throw<InvalidOperationException>(() => graph.BellmanFord(1));
            Should.Throw<InvalidOperationException>(() => graph.BellmanFord(2));
            Should.Throw<InvalidOperationException>(() => graph.BellmanFord(3));
            Should.Throw<InvalidOperationException>(() => graph.BellmanFord(4));
        }
    }
}
