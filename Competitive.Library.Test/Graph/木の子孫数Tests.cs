using FluentAssertions;
using Xunit;

namespace AtCoder.Graph
{
    public class 木の子孫数Tests
    {
        GraphBuilder gb;
        WIntGraphBuilder wgb;

        public 木の子孫数Tests()
        {
            gb = new GraphBuilder(8, false);
            gb.Add(0, 1);
            gb.Add(0, 2);
            gb.Add(1, 3);
            gb.Add(1, 4);
            gb.Add(2, 5);
            gb.Add(2, 6);
            gb.Add(3, 7);

            wgb = new WIntGraphBuilder(8, false);
            wgb.Add(0, 1, 1);
            wgb.Add(0, 2, 2);
            wgb.Add(1, 3, 3);
            wgb.Add(1, 4, 4);
            wgb.Add(2, 5, 5);
            wgb.Add(2, 6, 6);
            wgb.Add(3, 7, 7);
        }

        public static TheoryData Data = new TheoryData<int, int[]>
        {
            { 0, new[] { 8, 4, 3, 2, 1, 1, 1, 1 }},
            { 1, new[] { 4, 8, 3, 2, 1, 1, 1, 1 }},
            { 2, new[] { 5, 4, 8, 2, 1, 1, 1, 1 }},
            { 3, new[] { 4, 6, 3, 8, 1, 1, 1, 1 }},
            { 4, new[] { 4, 7, 3, 2, 8, 1, 1, 1 }},
            { 5, new[] { 5, 4, 7, 2, 1, 8, 1, 1 }},
            { 6, new[] { 5, 4, 7, 2, 1, 1, 8, 1 }},
            { 7, new[] { 4, 6, 3, 7, 1, 1, 1, 8 }},
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void 重みなしグラフ(int root, int[] expected)
        {
            gb.ToTree(root).DescendantsCounts().Should().Equal(expected);
        }
        [Theory]
        [MemberData(nameof(Data))]
        public void 重み付きグラフ(int root, int[] expected)
        {
            wgb.ToTree(root).DescendantsCounts().Should().Equal(expected);
        }
    }
}
