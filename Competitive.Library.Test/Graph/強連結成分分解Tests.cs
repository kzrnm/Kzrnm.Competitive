using FluentAssertions;
using Xunit;

namespace AtCoder.Graph
{
    public class 強連結成分分解Tests
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
            var scc = gb.ToGraph().Scc();
            scc[0].Should().Equal(0);
            scc[1].Should().Equal(1);
            scc[2].Should().Equal(2);
            scc[3].Should().Equal(3, 4, 7);
            scc[4].Should().Equal(5);
            scc[5].Should().Equal(6);
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
            var scc = gb.ToGraph().Scc();
            scc[0].Should().Equal(0);
            scc[1].Should().Equal(1);
            scc[2].Should().Equal(2);
            scc[3].Should().Equal(3, 4, 7);
            scc[4].Should().Equal(5);
            scc[5].Should().Equal(6);
        }
    }
}
