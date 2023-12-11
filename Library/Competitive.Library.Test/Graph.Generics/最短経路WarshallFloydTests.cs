
namespace Kzrnm.Competitive.Testing.Graph
{
    public class 最短経路WarshallFloydTests
    {
        [Fact]
        public void Int()
        {
            var gb = new WIntGraphBuilder(5, true);
            gb.Add(0, 1, 1);
            gb.Add(1, 1, 1);
            gb.Add(0, 2, 10);
            gb.Add(0, 3, 30);
            gb.Add(0, 4, 40);
            gb.Add(1, 2, 5);
            gb.Add(2, 3, 605);
            gb.Add(2, 4, 6);
            gb.Add(4, 3, 6);
            gb.Add(4, 0, 1);
            var res = gb.ToGraph().WarshallFloyd();
            res[0].Should().Equal([0, 1, 6, 18, 12]);
            res[1].Should().Equal([12, 0, 5, 17, 11]);
            res[2].Should().Equal([7, 8, 0, 12, 6]);
            res[3].Should().Equal([1073741823, 1073741823, 1073741823, 0, 1073741823]);
            res[4].Should().Equal([1, 2, 7, 6, 0]);
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
            var res = gb.ToGraph().WarshallFloyd();
            res[0].Should().Equal([0, 1, 6, 18, 12]);
            res[1].Should().Equal([12, 0, 5, 17, 11]);
            res[2].Should().Equal([7, 8, 0, 12, 6]);
            res[3].Should().Equal([4611686018427387903, 4611686018427387903, 4611686018427387903, 0, 4611686018427387903]);
            res[4].Should().Equal([1, 2, 7, 6, 0]);
        }
    }
}
