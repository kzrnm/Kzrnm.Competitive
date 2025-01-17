
namespace Kzrnm.Competitive.Testing.Graph
{
    public class DynamicConnectivityTests
    {
        [Fact]
        public void Solve()
        {
            var dc = new DynamicConnectivity(5);
            void ShouldConnectEqual(bool[,] connect)
            {
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 5; j++)
                        dc.Same(i, j).ShouldBe(connect[i, j], $"connect[{i}, {j}] should be {connect[i, j]}");
            }

            ShouldConnectEqual(new bool[5, 5]
            {
                {true,false,false,false,false },
                {false,true,false,false,false },
                {false,false,true,false,false },
                {false,false,false,true,false },
                {false,false,false,false,true },
            });

            dc.Link(0, 1);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,false,false,false },
                {true,true,false,false,false },
                {false,false,true,false,false },
                {false,false,false,true,false },
                {false,false,false,false,true },
            });

            dc.Link(1, 2);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,true,false,false },
                {true,true,true,false,false },
                {true,true,true,false,false },
                {false,false,false,true,false },
                {false,false,false,false,true },
            });

            dc.Link(2, 3);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,true,true,false },
                {true,true,true,true,false },
                {true,true,true,true,false },
                {true,true,true,true,false },
                {false,false,false,false,true },
            });

            dc.Link(3, 4);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
            });

            dc.Link(0, 4);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
            });

            dc.Cut(1, 2);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
            });

            dc.Cut(3, 4);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,false,false,true },
                {true,true,false,false,true },
                {false,false,true,true,false },
                {false,false,true,true,false },
                {true,true,false,false,true },
            });

            dc.Link(1, 4);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,false,false,true },
                {true,true,false,false,true },
                {false,false,true,true,false },
                {false,false,true,true,false },
                {true,true,false,false,true },
            });

            dc.Link(3, 4);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
            });
        }
    }
}
