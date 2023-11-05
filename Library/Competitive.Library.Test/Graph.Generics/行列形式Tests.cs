namespace Kzrnm.Competitive.Testing.Graph
{
    public class 行列形式Tests
    {
        [Fact]
        public void Adjacency()
        {
            var gb = new WIntGraphBuilder(5, false);
            gb.Add(0, 1, 1);
            gb.Add(0, 2, 10);
            gb.Add(0, 3, 30);
            gb.Add(0, 4, 40);
            gb.Add(1, 2, 5);
            gb.Add(2, 3, 605);
            gb.Add(2, 4, 6);
            gb.Add(4, 3, 6);
            gb.Add(4, 0, 1);

            var g = gb.ToGraph();
            g.Adjacency().Should().Be(new ArrayMatrix<int>(new int[5, 5]
            {
                {  0, 1,  10,  30, 1 },
                {  1, 0,   5,   0, 0 },
                { 10, 5,   0, 605, 6 },
                { 30, 0, 605,   0, 6 },
                {  1, 0,   6,   6, 0 },
            }));

            g.AsUnweighted().Adjacency().Should().Be(new ArrayMatrix<int>(new int[5, 5]
            {
                { 0, 1, 1, 1, 1 },
                { 1, 0, 1, 0, 0 },
                { 1, 1, 0, 1, 1 },
                { 1, 0, 1, 0, 1 },
                { 1, 0, 1, 1, 0 },
            }));
        }

        [Fact]
        public void Laplacian()
        {
            var gb = new WIntGraphBuilder(5, false);
            gb.Add(0, 1, 1);
            gb.Add(0, 2, 10);
            gb.Add(0, 3, 30);
            gb.Add(0, 4, 40);
            gb.Add(1, 2, 5);
            gb.Add(2, 3, 605);
            gb.Add(2, 4, 6);
            gb.Add(4, 3, 6);

            var g = gb.ToGraph();
            g.Laplacian().Should().Be(new ArrayMatrix<int>(new int[5, 5]
            {
                { 4, -1, -1, -1, -1 },
                { -1, 2, -1, 0, 0 },
                { -1, -1, 4, -1, -1 },
                { -1, 0, -1, 3, -1 },
                { -1, 0, -1, -1, 3 },
            }));
        }
    }
}