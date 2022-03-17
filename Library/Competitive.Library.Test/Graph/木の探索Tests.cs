using FluentAssertions;
using Xunit;

namespace Kzrnm.Competitive.Graph
{
    public class 木の探索Tests
    {
        GraphBuilder gb;
        WIntGraphBuilder wgb;

        public 木の探索Tests()
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

        public static TheoryData BfsData = new TheoryData<int, int[]>
        {
            { 0, new[] { 0, 1, 2, 3, 4, 5, 6, 7 }},
            { 1, new[] { 1, 0, 3, 4, 2, 7, 5, 6 }},
            { 2, new[] { 2, 0, 5, 6, 1, 3, 4, 7 }},
            { 3, new[] { 3, 1, 7, 0, 4, 2, 5, 6 }},
            { 4, new[] { 4, 1, 0, 3, 2, 7, 5, 6 }},
            { 5, new[] { 5, 2, 0, 6, 1, 3, 4, 7 }},
            { 6, new[] { 6, 2, 0, 5, 1, 3, 4, 7 }},
            { 7, new[] { 7, 3, 1, 0, 4, 2, 5, 6 }},
        };

        [Theory]
        [MemberData(nameof(BfsData))]
        public void Bfs重みなしグラフ(int root, int[] expected)
        {
            gb.ToTree(root).BfsDescendant().Should().Equal(expected);
        }

        [Theory]
        [MemberData(nameof(BfsData))]
        public void Bfs重み付きグラフ(int root, int[] expected)
        {
            wgb.ToTree(root).BfsDescendant().Should().Equal(expected);
        }

        [Theory]
        [MemberData(nameof(BfsData))]
        public void Bfs重みなしグラフSkipFirst(int root, int[] expected)
        {
            gb.ToTree(root).BfsDescendant(true).Should().Equal(expected[1..]);
        }

        [Theory]
        [MemberData(nameof(BfsData))]
        public void Bfs重み付きグラフSkipFirst(int root, int[] expected)
        {
            wgb.ToTree(root).BfsDescendant(true).Should().Equal(expected[1..]);
        }

        public static TheoryData DfsData = new TheoryData<int, int[]>
        {
            { 0, new[] { 0, 1, 3, 7, 4, 2, 5, 6 }},
            { 1, new[] { 1, 0, 2, 5, 6, 3, 7, 4 }},
            { 2, new[] { 2, 0, 1, 3, 7, 4, 5, 6 }},
            { 3, new[] { 3, 1, 0, 2, 5, 6, 4, 7 }},
            { 4, new[] { 4, 1, 0, 2, 5, 6, 3, 7 }},
            { 5, new[] { 5, 2, 0, 1, 3, 7, 4, 6 }},
            { 6, new[] { 6, 2, 0, 1, 3, 7, 4, 5 }},
            { 7, new[] { 7, 3, 1, 0, 2, 5, 6, 4 }},
        };

        [Theory]
        [MemberData(nameof(DfsData))]
        public void Dfs重みなしグラフ(int root, int[] expected)
        {
            gb.ToTree(root).DfsDescendant().Should().Equal(expected);
        }

        [Theory]
        [MemberData(nameof(DfsData))]
        public void Dfs重み付きグラフ(int root, int[] expected)
        {
            wgb.ToTree(root).DfsDescendant().Should().Equal(expected);
        }

        [Theory]
        [MemberData(nameof(DfsData))]
        public void Dfs重みなしグラフSkipFirst(int root, int[] expected)
        {
            gb.ToTree(root).DfsDescendant(true).Should().Equal(expected[1..]);
        }

        [Theory]
        [MemberData(nameof(DfsData))]
        public void Dfs重み付きグラフSkipFirst(int root, int[] expected)
        {
            wgb.ToTree(root).DfsDescendant(true).Should().Equal(expected[1..]);
        }
    }
}
