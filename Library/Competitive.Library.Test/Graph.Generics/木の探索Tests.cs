namespace Kzrnm.Competitive.Testing.Graph;

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

    public static TheoryData<int, int[]> BfsData => new()
    {
        { 0, [0, 1, 2, 3, 4, 6, 5, 7]},
        { 1, [1, 0, 3, 4, 2, 7, 6, 5]},
        { 2, [2, 0, 5, 6, 1, 3, 4, 7]},
        { 3, [3, 1, 7, 0, 4, 2, 6, 5]},
        { 4, [4, 1, 0, 3, 2, 7, 6, 5]},
        { 5, [5, 2, 0, 6, 1, 3, 4, 7]},
        { 6, [6, 2, 0, 5, 1, 3, 4, 7]},
        { 7, [7, 3, 1, 0, 4, 2, 6, 5]},
    };

    [Theory]
    [MemberData(nameof(BfsData), DisableDiscoveryEnumeration = true)]
    public void Bfs(int root, int[] expected)
    {
        gb.ToTree(root).BfsDescendant().ShouldBe(expected);
        wgb.ToTree(root).BfsDescendant().ShouldBe(expected);
    }

    public static TheoryData<int, int[]> DfsData => new()
    {
        { 0, [0, 1, 3, 7, 4, 2, 6, 5]},
        { 1, [1, 0, 2, 6, 5, 3, 7, 4]},
        { 2, [2, 0, 1, 3, 7, 4, 5, 6]},
        { 3, [3, 1, 0, 2, 6, 5, 4, 7]},
        { 4, [4, 1, 0, 2, 6, 5, 3, 7]},
        { 5, [5, 2, 0, 1, 3, 7, 4, 6]},
        { 6, [6, 2, 0, 1, 3, 7, 4, 5]},
        { 7, [7, 3, 1, 0, 2, 6, 5, 4]},
    };

    [Theory]
    [MemberData(nameof(DfsData), DisableDiscoveryEnumeration = true)]
    public void Dfs(int root, int[] expected)
    {
        gb.ToTree(root).DfsDescendant().ShouldBe(expected);
        wgb.ToTree(root).DfsDescendant().ShouldBe(expected);
    }

    public static TheoryData<int, int[]> DfsLeafData => new()
    {
        { 0, [5, 6, 2, 4, 7, 3, 1, 0]},
        { 1, [4, 7, 3, 5, 6, 2, 0, 1]},
        { 2, [6, 5, 4, 7, 3, 1, 0, 2]},
        { 3, [7, 4, 5, 6, 2, 0, 1, 3]},
        { 4, [7, 3, 5, 6, 2, 0, 1, 4]},
        { 5, [6, 4, 7, 3, 1, 0, 2, 5]},
        { 6, [5, 4, 7, 3, 1, 0, 2, 6]},
        { 7, [4, 5, 6, 2, 0, 1, 3, 7]},
    };

    [Theory]
    [MemberData(nameof(DfsLeafData), DisableDiscoveryEnumeration = true)]
    public void DfsLeaf(int root, int[] expected)
    {
        gb.ToTree(root).DfsDescendantLeaf().ShouldBe(expected);
        wgb.ToTree(root).DfsDescendantLeaf().ShouldBe(expected);
    }

    public static TheoryData<int, int[]> DfsEventsData => new()
    {
        { 0, [0, 1, 3, 7, ~7, ~3, 4, ~4, ~1, 2, 6, ~6, 5, ~5, ~2, ~0]},
        { 1, [1, 0, 2, 6, ~6, 5, ~5, ~2, ~0, 3, 7, ~7, ~3, 4, ~4, ~1]},
        { 2, [2, 0, 1, 3, 7, ~7, ~3, 4, ~4, ~1, ~0, 5, ~5, 6, ~6, ~2]},
        { 3, [3, 1, 0, 2, 6, ~6, 5, ~5, ~2, ~0, 4, ~4, ~1, 7, ~7, ~3]},
        { 4, [4, 1, 0, 2, 6, ~6, 5, ~5, ~2, ~0, 3, 7, ~7, ~3, ~1, ~4]},
        { 5, [5, 2, 0, 1, 3, 7, ~7, ~3, 4, ~4, ~1, ~0, 6, ~6, ~2, ~5]},
        { 6, [6, 2, 0, 1, 3, 7, ~7, ~3, 4, ~4, ~1, ~0, 5, ~5, ~2, ~6]},
        { 7, [7, 3, 1, 0, 2, 6, ~6, 5, ~5, ~2, ~0, 4, ~4, ~1, ~3, ~7]},
    };

    [Theory]
    [MemberData(nameof(DfsEventsData), DisableDiscoveryEnumeration = true)]
    public void DfsEvents(int root, int[] expected)
    {
        gb.ToTree(root).DfsDescendantEvents().ShouldBe(expected);
        wgb.ToTree(root).DfsDescendantEvents().ShouldBe(expected);
    }
}
