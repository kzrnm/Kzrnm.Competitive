using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Graph;

public class 最小共通祖先Tests
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

        var tree = gb.ToTree();
        tree.HlDecomposition.LowestCommonAncestor(0, 0).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(0, 1).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(0, 2).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(0, 3).ShouldBe(0);
        tree.HlDecomposition.LowestCommonAncestor(0, 4).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(0, 5).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(0, 6).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(0, 7).ShouldBe(0);
        tree.HlDecomposition.LowestCommonAncestor(1, 0).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(1, 1).ShouldBe(1); tree.HlDecomposition.LowestCommonAncestor(1, 2).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(1, 3).ShouldBe(1);
        tree.HlDecomposition.LowestCommonAncestor(1, 4).ShouldBe(1); tree.HlDecomposition.LowestCommonAncestor(1, 5).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(1, 6).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(1, 7).ShouldBe(1);
        tree.HlDecomposition.LowestCommonAncestor(2, 0).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(2, 1).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(2, 2).ShouldBe(2); tree.HlDecomposition.LowestCommonAncestor(2, 3).ShouldBe(0);
        tree.HlDecomposition.LowestCommonAncestor(2, 4).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(2, 5).ShouldBe(2); tree.HlDecomposition.LowestCommonAncestor(2, 6).ShouldBe(2); tree.HlDecomposition.LowestCommonAncestor(2, 7).ShouldBe(0);
        tree.HlDecomposition.LowestCommonAncestor(3, 0).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(3, 1).ShouldBe(1); tree.HlDecomposition.LowestCommonAncestor(3, 2).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(3, 3).ShouldBe(3);
        tree.HlDecomposition.LowestCommonAncestor(3, 4).ShouldBe(1); tree.HlDecomposition.LowestCommonAncestor(3, 5).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(3, 6).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(3, 7).ShouldBe(3);
        tree.HlDecomposition.LowestCommonAncestor(4, 0).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(4, 1).ShouldBe(1); tree.HlDecomposition.LowestCommonAncestor(4, 2).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(4, 3).ShouldBe(1);
        tree.HlDecomposition.LowestCommonAncestor(4, 4).ShouldBe(4); tree.HlDecomposition.LowestCommonAncestor(4, 5).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(4, 6).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(4, 7).ShouldBe(1);
        tree.HlDecomposition.LowestCommonAncestor(5, 0).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(5, 1).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(5, 2).ShouldBe(2); tree.HlDecomposition.LowestCommonAncestor(5, 3).ShouldBe(0);
        tree.HlDecomposition.LowestCommonAncestor(5, 4).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(5, 5).ShouldBe(5); tree.HlDecomposition.LowestCommonAncestor(5, 6).ShouldBe(2); tree.HlDecomposition.LowestCommonAncestor(5, 7).ShouldBe(0);
        tree.HlDecomposition.LowestCommonAncestor(6, 0).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(6, 1).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(6, 2).ShouldBe(2); tree.HlDecomposition.LowestCommonAncestor(6, 3).ShouldBe(0);
        tree.HlDecomposition.LowestCommonAncestor(6, 4).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(6, 5).ShouldBe(2); tree.HlDecomposition.LowestCommonAncestor(6, 6).ShouldBe(6); tree.HlDecomposition.LowestCommonAncestor(6, 7).ShouldBe(0);
        tree.HlDecomposition.LowestCommonAncestor(7, 0).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(7, 1).ShouldBe(1); tree.HlDecomposition.LowestCommonAncestor(7, 2).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(7, 3).ShouldBe(3);
        tree.HlDecomposition.LowestCommonAncestor(7, 4).ShouldBe(1); tree.HlDecomposition.LowestCommonAncestor(7, 5).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(7, 6).ShouldBe(0); tree.HlDecomposition.LowestCommonAncestor(7, 7).ShouldBe(7);

        var lca0 = tree.LowestCommonAncestorDoubling();
        lca0.Lca(0, 0).ShouldBe(0); lca0.Lca(0, 1).ShouldBe(0); lca0.Lca(0, 2).ShouldBe(0); lca0.Lca(0, 3).ShouldBe(0);
        lca0.Lca(0, 4).ShouldBe(0); lca0.Lca(0, 5).ShouldBe(0); lca0.Lca(0, 6).ShouldBe(0); lca0.Lca(0, 7).ShouldBe(0);
        lca0.Lca(1, 0).ShouldBe(0); lca0.Lca(1, 1).ShouldBe(1); lca0.Lca(1, 2).ShouldBe(0); lca0.Lca(1, 3).ShouldBe(1);
        lca0.Lca(1, 4).ShouldBe(1); lca0.Lca(1, 5).ShouldBe(0); lca0.Lca(1, 6).ShouldBe(0); lca0.Lca(1, 7).ShouldBe(1);
        lca0.Lca(2, 0).ShouldBe(0); lca0.Lca(2, 1).ShouldBe(0); lca0.Lca(2, 2).ShouldBe(2); lca0.Lca(2, 3).ShouldBe(0);
        lca0.Lca(2, 4).ShouldBe(0); lca0.Lca(2, 5).ShouldBe(2); lca0.Lca(2, 6).ShouldBe(2); lca0.Lca(2, 7).ShouldBe(0);
        lca0.Lca(3, 0).ShouldBe(0); lca0.Lca(3, 1).ShouldBe(1); lca0.Lca(3, 2).ShouldBe(0); lca0.Lca(3, 3).ShouldBe(3);
        lca0.Lca(3, 4).ShouldBe(1); lca0.Lca(3, 5).ShouldBe(0); lca0.Lca(3, 6).ShouldBe(0); lca0.Lca(3, 7).ShouldBe(3);
        lca0.Lca(4, 0).ShouldBe(0); lca0.Lca(4, 1).ShouldBe(1); lca0.Lca(4, 2).ShouldBe(0); lca0.Lca(4, 3).ShouldBe(1);
        lca0.Lca(4, 4).ShouldBe(4); lca0.Lca(4, 5).ShouldBe(0); lca0.Lca(4, 6).ShouldBe(0); lca0.Lca(4, 7).ShouldBe(1);
        lca0.Lca(5, 0).ShouldBe(0); lca0.Lca(5, 1).ShouldBe(0); lca0.Lca(5, 2).ShouldBe(2); lca0.Lca(5, 3).ShouldBe(0);
        lca0.Lca(5, 4).ShouldBe(0); lca0.Lca(5, 5).ShouldBe(5); lca0.Lca(5, 6).ShouldBe(2); lca0.Lca(5, 7).ShouldBe(0);
        lca0.Lca(6, 0).ShouldBe(0); lca0.Lca(6, 1).ShouldBe(0); lca0.Lca(6, 2).ShouldBe(2); lca0.Lca(6, 3).ShouldBe(0);
        lca0.Lca(6, 4).ShouldBe(0); lca0.Lca(6, 5).ShouldBe(2); lca0.Lca(6, 6).ShouldBe(6); lca0.Lca(6, 7).ShouldBe(0);
        lca0.Lca(7, 0).ShouldBe(0); lca0.Lca(7, 1).ShouldBe(1); lca0.Lca(7, 2).ShouldBe(0); lca0.Lca(7, 3).ShouldBe(3);
        lca0.Lca(7, 4).ShouldBe(1); lca0.Lca(7, 5).ShouldBe(0); lca0.Lca(7, 6).ShouldBe(0); lca0.Lca(7, 7).ShouldBe(7);

        lca0[0, 0].ShouldBe(0); lca0[0, 1].ShouldBe(0); lca0[0, 2].ShouldBe(0); lca0[0, 3].ShouldBe(0);
        lca0[0, 4].ShouldBe(0); lca0[0, 5].ShouldBe(0); lca0[0, 6].ShouldBe(0); lca0[0, 7].ShouldBe(0);
        lca0[1, 0].ShouldBe(0); lca0[1, 1].ShouldBe(1); lca0[1, 2].ShouldBe(0); lca0[1, 3].ShouldBe(1);
        lca0[1, 4].ShouldBe(1); lca0[1, 5].ShouldBe(0); lca0[1, 6].ShouldBe(0); lca0[1, 7].ShouldBe(1);
        lca0[2, 0].ShouldBe(0); lca0[2, 1].ShouldBe(0); lca0[2, 2].ShouldBe(2); lca0[2, 3].ShouldBe(0);
        lca0[2, 4].ShouldBe(0); lca0[2, 5].ShouldBe(2); lca0[2, 6].ShouldBe(2); lca0[2, 7].ShouldBe(0);
        lca0[3, 0].ShouldBe(0); lca0[3, 1].ShouldBe(1); lca0[3, 2].ShouldBe(0); lca0[3, 3].ShouldBe(3);
        lca0[3, 4].ShouldBe(1); lca0[3, 5].ShouldBe(0); lca0[3, 6].ShouldBe(0); lca0[3, 7].ShouldBe(3);
        lca0[4, 0].ShouldBe(0); lca0[4, 1].ShouldBe(1); lca0[4, 2].ShouldBe(0); lca0[4, 3].ShouldBe(1);
        lca0[4, 4].ShouldBe(4); lca0[4, 5].ShouldBe(0); lca0[4, 6].ShouldBe(0); lca0[4, 7].ShouldBe(1);
        lca0[5, 0].ShouldBe(0); lca0[5, 1].ShouldBe(0); lca0[5, 2].ShouldBe(2); lca0[5, 3].ShouldBe(0);
        lca0[5, 4].ShouldBe(0); lca0[5, 5].ShouldBe(5); lca0[5, 6].ShouldBe(2); lca0[5, 7].ShouldBe(0);
        lca0[6, 0].ShouldBe(0); lca0[6, 1].ShouldBe(0); lca0[6, 2].ShouldBe(2); lca0[6, 3].ShouldBe(0);
        lca0[6, 4].ShouldBe(0); lca0[6, 5].ShouldBe(2); lca0[6, 6].ShouldBe(6); lca0[6, 7].ShouldBe(0);
        lca0[7, 0].ShouldBe(0); lca0[7, 1].ShouldBe(1); lca0[7, 2].ShouldBe(0); lca0[7, 3].ShouldBe(3);
        lca0[7, 4].ShouldBe(1); lca0[7, 5].ShouldBe(0); lca0[7, 6].ShouldBe(0); lca0[7, 7].ShouldBe(7);

        lca0.ChildOfLca(0, 0).ShouldBe((0, 0)); lca0.ChildOfLca(0, 1).ShouldBe((0, 1)); lca0.ChildOfLca(0, 2).ShouldBe((0, 2)); lca0.ChildOfLca(0, 3).ShouldBe((0, 1));
        lca0.ChildOfLca(0, 4).ShouldBe((0, 1)); lca0.ChildOfLca(0, 5).ShouldBe((0, 2)); lca0.ChildOfLca(0, 6).ShouldBe((0, 2)); lca0.ChildOfLca(0, 7).ShouldBe((0, 1));
        lca0.ChildOfLca(1, 0).ShouldBe((1, 0)); lca0.ChildOfLca(1, 1).ShouldBe((1, 1)); lca0.ChildOfLca(1, 2).ShouldBe((1, 2)); lca0.ChildOfLca(1, 3).ShouldBe((1, 3));
        lca0.ChildOfLca(1, 4).ShouldBe((1, 4)); lca0.ChildOfLca(1, 5).ShouldBe((1, 2)); lca0.ChildOfLca(1, 6).ShouldBe((1, 2)); lca0.ChildOfLca(1, 7).ShouldBe((1, 3));
        lca0.ChildOfLca(2, 0).ShouldBe((2, 0)); lca0.ChildOfLca(2, 1).ShouldBe((2, 1)); lca0.ChildOfLca(2, 2).ShouldBe((2, 2)); lca0.ChildOfLca(2, 3).ShouldBe((2, 1));
        lca0.ChildOfLca(2, 4).ShouldBe((2, 1)); lca0.ChildOfLca(2, 5).ShouldBe((2, 5)); lca0.ChildOfLca(2, 6).ShouldBe((2, 6)); lca0.ChildOfLca(2, 7).ShouldBe((2, 1));
        lca0.ChildOfLca(3, 0).ShouldBe((1, 0)); lca0.ChildOfLca(3, 1).ShouldBe((3, 1)); lca0.ChildOfLca(1, 2).ShouldBe((1, 2)); lca0.ChildOfLca(3, 3).ShouldBe((3, 3));
        lca0.ChildOfLca(3, 4).ShouldBe((3, 4)); lca0.ChildOfLca(3, 5).ShouldBe((1, 2)); lca0.ChildOfLca(3, 6).ShouldBe((1, 2)); lca0.ChildOfLca(3, 7).ShouldBe((3, 7));
        lca0.ChildOfLca(4, 0).ShouldBe((1, 0)); lca0.ChildOfLca(4, 1).ShouldBe((4, 1)); lca0.ChildOfLca(4, 2).ShouldBe((1, 2)); lca0.ChildOfLca(4, 3).ShouldBe((4, 3));
        lca0.ChildOfLca(4, 4).ShouldBe((4, 4)); lca0.ChildOfLca(4, 5).ShouldBe((1, 2)); lca0.ChildOfLca(4, 6).ShouldBe((1, 2)); lca0.ChildOfLca(4, 7).ShouldBe((4, 3));
        lca0.ChildOfLca(5, 0).ShouldBe((2, 0)); lca0.ChildOfLca(5, 1).ShouldBe((2, 1)); lca0.ChildOfLca(5, 2).ShouldBe((5, 2)); lca0.ChildOfLca(5, 3).ShouldBe((2, 1));
        lca0.ChildOfLca(5, 4).ShouldBe((2, 1)); lca0.ChildOfLca(5, 5).ShouldBe((5, 5)); lca0.ChildOfLca(5, 6).ShouldBe((5, 6)); lca0.ChildOfLca(5, 7).ShouldBe((2, 1));
        lca0.ChildOfLca(6, 0).ShouldBe((2, 0)); lca0.ChildOfLca(6, 1).ShouldBe((2, 1)); lca0.ChildOfLca(6, 2).ShouldBe((6, 2)); lca0.ChildOfLca(6, 3).ShouldBe((2, 1));
        lca0.ChildOfLca(6, 4).ShouldBe((2, 1)); lca0.ChildOfLca(6, 5).ShouldBe((6, 5)); lca0.ChildOfLca(6, 6).ShouldBe((6, 6)); lca0.ChildOfLca(6, 7).ShouldBe((2, 1));
        lca0.ChildOfLca(7, 0).ShouldBe((1, 0)); lca0.ChildOfLca(7, 1).ShouldBe((3, 1)); lca0.ChildOfLca(7, 2).ShouldBe((1, 2)); lca0.ChildOfLca(7, 3).ShouldBe((7, 3));
        lca0.ChildOfLca(7, 4).ShouldBe((3, 4)); lca0.ChildOfLca(7, 5).ShouldBe((1, 2)); lca0.ChildOfLca(7, 6).ShouldBe((1, 2)); lca0.ChildOfLca(7, 7).ShouldBe((7, 7));

        lca0.Ascend(0, 0).ShouldBe(0); lca0.Ascend(0, 1).ShouldBe(-1);
        lca0.Ascend(1, 0).ShouldBe(1); lca0.Ascend(1, 1).ShouldBe(0); lca0.Ascend(1, 2).ShouldBe(-1);
        lca0.Ascend(2, 0).ShouldBe(2); lca0.Ascend(2, 1).ShouldBe(0); lca0.Ascend(2, 2).ShouldBe(-1);
        lca0.Ascend(3, 0).ShouldBe(3); lca0.Ascend(3, 1).ShouldBe(1); lca0.Ascend(3, 2).ShouldBe(0); lca0.Ascend(3, 3).ShouldBe(-1);
        lca0.Ascend(4, 0).ShouldBe(4); lca0.Ascend(4, 1).ShouldBe(1); lca0.Ascend(4, 2).ShouldBe(0); lca0.Ascend(4, 3).ShouldBe(-1);
        lca0.Ascend(5, 0).ShouldBe(5); lca0.Ascend(5, 1).ShouldBe(2); lca0.Ascend(5, 2).ShouldBe(0); lca0.Ascend(5, 3).ShouldBe(-1);
        lca0.Ascend(6, 0).ShouldBe(6); lca0.Ascend(6, 1).ShouldBe(2); lca0.Ascend(6, 2).ShouldBe(0); lca0.Ascend(6, 3).ShouldBe(-1);
        lca0.Ascend(7, 0).ShouldBe(7); lca0.Ascend(7, 1).ShouldBe(3); lca0.Ascend(7, 2).ShouldBe(1); lca0.Ascend(7, 3).ShouldBe(0); lca0.Ascend(7, 4).ShouldBe(-1);
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
        var tree = gb.ToTree();
        var lca0 = tree.LowestCommonAncestorDoubling();
        lca0.Lca(0, 0).ShouldBe(0); lca0.Lca(0, 1).ShouldBe(0); lca0.Lca(0, 2).ShouldBe(0); lca0.Lca(0, 3).ShouldBe(0);
        lca0.Lca(0, 4).ShouldBe(0); lca0.Lca(0, 5).ShouldBe(0); lca0.Lca(0, 6).ShouldBe(0); lca0.Lca(0, 7).ShouldBe(0);
        lca0.Lca(1, 0).ShouldBe(0); lca0.Lca(1, 1).ShouldBe(1); lca0.Lca(1, 2).ShouldBe(0); lca0.Lca(1, 3).ShouldBe(1);
        lca0.Lca(1, 4).ShouldBe(1); lca0.Lca(1, 5).ShouldBe(0); lca0.Lca(1, 6).ShouldBe(0); lca0.Lca(1, 7).ShouldBe(1);
        lca0.Lca(2, 0).ShouldBe(0); lca0.Lca(2, 1).ShouldBe(0); lca0.Lca(2, 2).ShouldBe(2); lca0.Lca(2, 3).ShouldBe(0);
        lca0.Lca(2, 4).ShouldBe(0); lca0.Lca(2, 5).ShouldBe(2); lca0.Lca(2, 6).ShouldBe(2); lca0.Lca(2, 7).ShouldBe(0);
        lca0.Lca(3, 0).ShouldBe(0); lca0.Lca(3, 1).ShouldBe(1); lca0.Lca(3, 2).ShouldBe(0); lca0.Lca(3, 3).ShouldBe(3);
        lca0.Lca(3, 4).ShouldBe(1); lca0.Lca(3, 5).ShouldBe(0); lca0.Lca(3, 6).ShouldBe(0); lca0.Lca(3, 7).ShouldBe(3);
        lca0.Lca(4, 0).ShouldBe(0); lca0.Lca(4, 1).ShouldBe(1); lca0.Lca(4, 2).ShouldBe(0); lca0.Lca(4, 3).ShouldBe(1);
        lca0.Lca(4, 4).ShouldBe(4); lca0.Lca(4, 5).ShouldBe(0); lca0.Lca(4, 6).ShouldBe(0); lca0.Lca(4, 7).ShouldBe(1);
        lca0.Lca(5, 0).ShouldBe(0); lca0.Lca(5, 1).ShouldBe(0); lca0.Lca(5, 2).ShouldBe(2); lca0.Lca(5, 3).ShouldBe(0);
        lca0.Lca(5, 4).ShouldBe(0); lca0.Lca(5, 5).ShouldBe(5); lca0.Lca(5, 6).ShouldBe(2); lca0.Lca(5, 7).ShouldBe(0);
        lca0.Lca(6, 0).ShouldBe(0); lca0.Lca(6, 1).ShouldBe(0); lca0.Lca(6, 2).ShouldBe(2); lca0.Lca(6, 3).ShouldBe(0);
        lca0.Lca(6, 4).ShouldBe(0); lca0.Lca(6, 5).ShouldBe(2); lca0.Lca(6, 6).ShouldBe(6); lca0.Lca(6, 7).ShouldBe(0);
        lca0.Lca(7, 0).ShouldBe(0); lca0.Lca(7, 1).ShouldBe(1); lca0.Lca(7, 2).ShouldBe(0); lca0.Lca(7, 3).ShouldBe(3);
        lca0.Lca(7, 4).ShouldBe(1); lca0.Lca(7, 5).ShouldBe(0); lca0.Lca(7, 6).ShouldBe(0); lca0.Lca(7, 7).ShouldBe(7);

        lca0[0, 0].ShouldBe(0); lca0[0, 1].ShouldBe(0); lca0[0, 2].ShouldBe(0); lca0[0, 3].ShouldBe(0);
        lca0[0, 4].ShouldBe(0); lca0[0, 5].ShouldBe(0); lca0[0, 6].ShouldBe(0); lca0[0, 7].ShouldBe(0);
        lca0[1, 0].ShouldBe(0); lca0[1, 1].ShouldBe(1); lca0[1, 2].ShouldBe(0); lca0[1, 3].ShouldBe(1);
        lca0[1, 4].ShouldBe(1); lca0[1, 5].ShouldBe(0); lca0[1, 6].ShouldBe(0); lca0[1, 7].ShouldBe(1);
        lca0[2, 0].ShouldBe(0); lca0[2, 1].ShouldBe(0); lca0[2, 2].ShouldBe(2); lca0[2, 3].ShouldBe(0);
        lca0[2, 4].ShouldBe(0); lca0[2, 5].ShouldBe(2); lca0[2, 6].ShouldBe(2); lca0[2, 7].ShouldBe(0);
        lca0[3, 0].ShouldBe(0); lca0[3, 1].ShouldBe(1); lca0[3, 2].ShouldBe(0); lca0[3, 3].ShouldBe(3);
        lca0[3, 4].ShouldBe(1); lca0[3, 5].ShouldBe(0); lca0[3, 6].ShouldBe(0); lca0[3, 7].ShouldBe(3);
        lca0[4, 0].ShouldBe(0); lca0[4, 1].ShouldBe(1); lca0[4, 2].ShouldBe(0); lca0[4, 3].ShouldBe(1);
        lca0[4, 4].ShouldBe(4); lca0[4, 5].ShouldBe(0); lca0[4, 6].ShouldBe(0); lca0[4, 7].ShouldBe(1);
        lca0[5, 0].ShouldBe(0); lca0[5, 1].ShouldBe(0); lca0[5, 2].ShouldBe(2); lca0[5, 3].ShouldBe(0);
        lca0[5, 4].ShouldBe(0); lca0[5, 5].ShouldBe(5); lca0[5, 6].ShouldBe(2); lca0[5, 7].ShouldBe(0);
        lca0[6, 0].ShouldBe(0); lca0[6, 1].ShouldBe(0); lca0[6, 2].ShouldBe(2); lca0[6, 3].ShouldBe(0);
        lca0[6, 4].ShouldBe(0); lca0[6, 5].ShouldBe(2); lca0[6, 6].ShouldBe(6); lca0[6, 7].ShouldBe(0);
        lca0[7, 0].ShouldBe(0); lca0[7, 1].ShouldBe(1); lca0[7, 2].ShouldBe(0); lca0[7, 3].ShouldBe(3);
        lca0[7, 4].ShouldBe(1); lca0[7, 5].ShouldBe(0); lca0[7, 6].ShouldBe(0); lca0[7, 7].ShouldBe(7);

        lca0.ChildOfLca(0, 0).ShouldBe((0, 0)); lca0.ChildOfLca(0, 1).ShouldBe((0, 1)); lca0.ChildOfLca(0, 2).ShouldBe((0, 2)); lca0.ChildOfLca(0, 3).ShouldBe((0, 1));
        lca0.ChildOfLca(0, 4).ShouldBe((0, 1)); lca0.ChildOfLca(0, 5).ShouldBe((0, 2)); lca0.ChildOfLca(0, 6).ShouldBe((0, 2)); lca0.ChildOfLca(0, 7).ShouldBe((0, 1));
        lca0.ChildOfLca(1, 0).ShouldBe((1, 0)); lca0.ChildOfLca(1, 1).ShouldBe((1, 1)); lca0.ChildOfLca(1, 2).ShouldBe((1, 2)); lca0.ChildOfLca(1, 3).ShouldBe((1, 3));
        lca0.ChildOfLca(1, 4).ShouldBe((1, 4)); lca0.ChildOfLca(1, 5).ShouldBe((1, 2)); lca0.ChildOfLca(1, 6).ShouldBe((1, 2)); lca0.ChildOfLca(1, 7).ShouldBe((1, 3));
        lca0.ChildOfLca(2, 0).ShouldBe((2, 0)); lca0.ChildOfLca(2, 1).ShouldBe((2, 1)); lca0.ChildOfLca(2, 2).ShouldBe((2, 2)); lca0.ChildOfLca(2, 3).ShouldBe((2, 1));
        lca0.ChildOfLca(2, 4).ShouldBe((2, 1)); lca0.ChildOfLca(2, 5).ShouldBe((2, 5)); lca0.ChildOfLca(2, 6).ShouldBe((2, 6)); lca0.ChildOfLca(2, 7).ShouldBe((2, 1));
        lca0.ChildOfLca(3, 0).ShouldBe((1, 0)); lca0.ChildOfLca(3, 1).ShouldBe((3, 1)); lca0.ChildOfLca(1, 2).ShouldBe((1, 2)); lca0.ChildOfLca(3, 3).ShouldBe((3, 3));
        lca0.ChildOfLca(3, 4).ShouldBe((3, 4)); lca0.ChildOfLca(3, 5).ShouldBe((1, 2)); lca0.ChildOfLca(3, 6).ShouldBe((1, 2)); lca0.ChildOfLca(3, 7).ShouldBe((3, 7));
        lca0.ChildOfLca(4, 0).ShouldBe((1, 0)); lca0.ChildOfLca(4, 1).ShouldBe((4, 1)); lca0.ChildOfLca(4, 2).ShouldBe((1, 2)); lca0.ChildOfLca(4, 3).ShouldBe((4, 3));
        lca0.ChildOfLca(4, 4).ShouldBe((4, 4)); lca0.ChildOfLca(4, 5).ShouldBe((1, 2)); lca0.ChildOfLca(4, 6).ShouldBe((1, 2)); lca0.ChildOfLca(4, 7).ShouldBe((4, 3));
        lca0.ChildOfLca(5, 0).ShouldBe((2, 0)); lca0.ChildOfLca(5, 1).ShouldBe((2, 1)); lca0.ChildOfLca(5, 2).ShouldBe((5, 2)); lca0.ChildOfLca(5, 3).ShouldBe((2, 1));
        lca0.ChildOfLca(5, 4).ShouldBe((2, 1)); lca0.ChildOfLca(5, 5).ShouldBe((5, 5)); lca0.ChildOfLca(5, 6).ShouldBe((5, 6)); lca0.ChildOfLca(5, 7).ShouldBe((2, 1));
        lca0.ChildOfLca(6, 0).ShouldBe((2, 0)); lca0.ChildOfLca(6, 1).ShouldBe((2, 1)); lca0.ChildOfLca(6, 2).ShouldBe((6, 2)); lca0.ChildOfLca(6, 3).ShouldBe((2, 1));
        lca0.ChildOfLca(6, 4).ShouldBe((2, 1)); lca0.ChildOfLca(6, 5).ShouldBe((6, 5)); lca0.ChildOfLca(6, 6).ShouldBe((6, 6)); lca0.ChildOfLca(6, 7).ShouldBe((2, 1));
        lca0.ChildOfLca(7, 0).ShouldBe((1, 0)); lca0.ChildOfLca(7, 1).ShouldBe((3, 1)); lca0.ChildOfLca(7, 2).ShouldBe((1, 2)); lca0.ChildOfLca(7, 3).ShouldBe((7, 3));
        lca0.ChildOfLca(7, 4).ShouldBe((3, 4)); lca0.ChildOfLca(7, 5).ShouldBe((1, 2)); lca0.ChildOfLca(7, 6).ShouldBe((1, 2)); lca0.ChildOfLca(7, 7).ShouldBe((7, 7));

        lca0.Ascend(0, 0).ShouldBe(0); lca0.Ascend(0, 1).ShouldBe(-1);
        lca0.Ascend(1, 0).ShouldBe(1); lca0.Ascend(1, 1).ShouldBe(0); lca0.Ascend(1, 2).ShouldBe(-1);
        lca0.Ascend(2, 0).ShouldBe(2); lca0.Ascend(2, 1).ShouldBe(0); lca0.Ascend(2, 2).ShouldBe(-1);
        lca0.Ascend(3, 0).ShouldBe(3); lca0.Ascend(3, 1).ShouldBe(1); lca0.Ascend(3, 2).ShouldBe(0); lca0.Ascend(3, 3).ShouldBe(-1);
        lca0.Ascend(4, 0).ShouldBe(4); lca0.Ascend(4, 1).ShouldBe(1); lca0.Ascend(4, 2).ShouldBe(0); lca0.Ascend(4, 3).ShouldBe(-1);
        lca0.Ascend(5, 0).ShouldBe(5); lca0.Ascend(5, 1).ShouldBe(2); lca0.Ascend(5, 2).ShouldBe(0); lca0.Ascend(5, 3).ShouldBe(-1);
        lca0.Ascend(6, 0).ShouldBe(6); lca0.Ascend(6, 1).ShouldBe(2); lca0.Ascend(6, 2).ShouldBe(0); lca0.Ascend(6, 3).ShouldBe(-1);
        lca0.Ascend(7, 0).ShouldBe(7); lca0.Ascend(7, 1).ShouldBe(3); lca0.Ascend(7, 2).ShouldBe(1); lca0.Ascend(7, 3).ShouldBe(0); lca0.Ascend(7, 4).ShouldBe(-1);
    }

    [Fact]
    public void Random()
    {
        const int N = 20;
        var wgb = new WIntGraphBuilder(N, false);
        var gb = new GraphBuilder(N, false);
        var arr = Enumerable.Range(0, N).OrderBy(_ => Guid.NewGuid()).ToArray();
        for (int i = 1; i < arr.Length; i++)
        {
            gb.Add(arr[i - 1], arr[i]);
            wgb.Add(arr[i - 1], arr[i], i);
        }
        for (int r = 0; r < N; r++)
        {
            var tree = gb.ToTree(r);
            var wtree = wgb.ToTree(r);
            var lca = tree.LowestCommonAncestorDoubling();
            var wlca = wtree.LowestCommonAncestorDoubling();
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    var (expectedLca, expectedDist) = LcaDirect(tree.AsArray(), i, j);
                    tree.HlDecomposition.LowestCommonAncestor(i, j).ShouldBe(expectedLca);
                    lca.Lca(i, j).ShouldBe(expectedLca);
                    wlca.Lca(i, j).ShouldBe(expectedLca);
                    lca.Distance(i, j).ShouldBe(expectedDist);
                    wlca.Distance(i, j).ShouldBe(expectedDist);
                }
        }
    }
    static (int lca, int distance) LcaDirect(TreeNode<GraphEdge>[] tree, int i, int j)
    {
        int distance = 0;
        while (tree[i].Depth > tree[j].Depth)
        {
            i = tree[i].Parent;
            ++distance;
        }
        while (tree[i].Depth < tree[j].Depth)
        {
            j = tree[j].Parent;
            ++distance;
        }
        while (i != j)
        {
            i = tree[i].Parent;
            j = tree[j].Parent;
            distance += 2;
        }
        return (i, distance);
    }
}
