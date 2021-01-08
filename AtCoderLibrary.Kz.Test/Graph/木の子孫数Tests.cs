using FluentAssertions;
using Xunit;

namespace AtCoder.Graph
{
    public class 木の子孫数Tests
    {
        [Fact]
        public void 重みなしグラフ()
        {
            var gb = new GraphBuilder(8, false);
            gb.Add(0, 1);
            gb.Add(0, 2);
            gb.Add(1, 3);
            gb.Add(1, 4);
            gb.Add(2, 5);
            gb.Add(2, 6);
            gb.Add(3, 7);
            gb.ToTree(0).DescendantsCounts().Should().Equal(8, 4, 3, 2, 1, 1, 1, 1);
            gb.ToTree(1).DescendantsCounts().Should().Equal(4, 8, 3, 2, 1, 1, 1, 1);
            gb.ToTree(2).DescendantsCounts().Should().Equal(5, 4, 8, 2, 1, 1, 1, 1);
            gb.ToTree(3).DescendantsCounts().Should().Equal(4, 6, 3, 8, 1, 1, 1, 1);
            gb.ToTree(4).DescendantsCounts().Should().Equal(4, 7, 3, 2, 8, 1, 1, 1);
            gb.ToTree(5).DescendantsCounts().Should().Equal(5, 4, 7, 2, 1, 8, 1, 1);
            gb.ToTree(6).DescendantsCounts().Should().Equal(5, 4, 7, 2, 1, 1, 8, 1);
            gb.ToTree(7).DescendantsCounts().Should().Equal(4, 6, 3, 7, 1, 1, 1, 8);
        }
        [Fact]
        public void 重み付きグラフ()
        {
            var gb = new WIntGraphBuilder(8, false);
            gb.Add(0, 1, 1);
            gb.Add(0, 2, 2);
            gb.Add(1, 3, 3);
            gb.Add(1, 4, 4);
            gb.Add(2, 5, 5);
            gb.Add(2, 6, 6);
            gb.Add(3, 7, 7);
            gb.ToTree(0).DescendantsCounts().Should().Equal(8, 4, 3, 2, 1, 1, 1, 1);
            gb.ToTree(1).DescendantsCounts().Should().Equal(4, 8, 3, 2, 1, 1, 1, 1);
            gb.ToTree(2).DescendantsCounts().Should().Equal(5, 4, 8, 2, 1, 1, 1, 1);
            gb.ToTree(3).DescendantsCounts().Should().Equal(4, 6, 3, 8, 1, 1, 1, 1);
            gb.ToTree(4).DescendantsCounts().Should().Equal(4, 7, 3, 2, 8, 1, 1, 1);
            gb.ToTree(5).DescendantsCounts().Should().Equal(5, 4, 7, 2, 1, 8, 1, 1);
            gb.ToTree(6).DescendantsCounts().Should().Equal(5, 4, 7, 2, 1, 1, 8, 1);
            gb.ToTree(7).DescendantsCounts().Should().Equal(4, 6, 3, 7, 1, 1, 1, 8);
        }
    }
}
