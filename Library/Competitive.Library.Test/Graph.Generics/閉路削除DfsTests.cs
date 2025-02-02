
using System.Linq;

namespace Kzrnm.Competitive.Testing.Graph;

public class 閉路削除DfsTests
{
    [Fact]
    public void 重みなしグラフ()
    {
        var gb = new GraphBuilder(8, true);
        gb.Add(0, 1);
        gb.Add(1, 2);
        gb.Add(2, 3);
        gb.Add(3, 5);
        gb.Add(3, 4);
        gb.Add(4, 5);
        gb.Add(5, 6);
        gb.Add(4, 7);
        gb.Add(7, 3);
        var es = gb.ToGraph().RemoveCycle();
        es.Order().ShouldBe(new (int, GraphEdge)[] {
            (4, new(5)),
            (3, new(5)),
            (5, new(6)),
            (2, new(3)),
            (1, new(2)),
            (0, new(1)),
        }.Order());
    }
    [Fact]
    public void 重み付きグラフ()
    {
        var gb = new WIntGraphBuilder(8, true);
        gb.Add(0, 1, 1);
        gb.Add(1, 2, 2);
        gb.Add(2, 3, 3);
        gb.Add(3, 5, 5);
        gb.Add(3, 4, 4);
        gb.Add(4, 5, 5);
        gb.Add(5, 6, 6);
        gb.Add(4, 7, 7);
        gb.Add(7, 3, 8);
        var es = gb.ToGraph().RemoveCycle();
        es.Order().ShouldBe(new (int, WEdge<int>)[] {
            (4, new(5, 5)),
            (3, new(5, 5)),
            (5, new(6, 6)),
            (2, new(3, 3)),
            (1, new(2, 2)),
            (0, new(1, 1)),
        }.Order());
    }
    [Fact]
    public void 無向グラフ()
    {
        var gb = new GraphBuilder(8, false);
        gb.Add(0, 1);
        gb.Add(1, 2);
        gb.Add(2, 3);
        gb.Add(3, 5);
        gb.Add(3, 4);
        gb.Add(4, 5);
        gb.Add(5, 6);
        gb.Add(4, 7);
        var es = gb.ToGraph().RemoveCycle();
        es.Order().ShouldBe(new (int, GraphEdge)[] {
            (4, new(7)),
            (5, new(6)),
            (2, new(3)),
            (1, new(2)),
            (0, new(1)),
        }.Order());
    }
}
