using AtCoder;
using FluentAssertions;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.Testing.Graph
{
    public class DynamicConnectivityOpTests
    {
        [Fact]
        public void Solve()
        {
            var dc = new DynamicConnectivity<long, SolveOp>(5);
            void ShouldConnectEqual(bool[,] connect)
            {
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 5; j++)
                        dc.Same(i, j).Should().Be(connect[i, j], "connect[{0}, {1}] should be ", i, j, connect[i, j]);
            }
            dc[0] = 1;
            dc[1] = 10;
            dc[2] = 100;
            dc[3] = 1000;
            dc[4] = 10000;
            dc[0].Should().Be(1);
            dc[1].Should().Be(10);
            dc[2].Should().Be(100);
            dc[3].Should().Be(1000);
            dc[4].Should().Be(10000);
            dc.Prod(0).Should().Be(1);
            dc.Prod(1).Should().Be(10);
            dc.Prod(2).Should().Be(100);
            dc.Prod(3).Should().Be(1000);
            dc.Prod(4).Should().Be(10000);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,false,false,false,false },
                {false,true,false,false,false },
                {false,false,true,false,false },
                {false,false,false,true,false },
                {false,false,false,false,true },
            });

            dc.Link(0, 1);
            dc[0].Should().Be(1);
            dc[1].Should().Be(10);
            dc[2].Should().Be(100);
            dc[3].Should().Be(1000);
            dc[4].Should().Be(10000);
            dc.Prod(0).Should().Be(11);
            dc.Prod(1).Should().Be(11);
            dc.Prod(2).Should().Be(100);
            dc.Prod(3).Should().Be(1000);
            dc.Prod(4).Should().Be(10000);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,false,false,false },
                {true,true,false,false,false },
                {false,false,true,false,false },
                {false,false,false,true,false },
                {false,false,false,false,true },
            });

            dc.Link(1, 2);
            dc[0].Should().Be(1);
            dc[1].Should().Be(10);
            dc[2].Should().Be(100);
            dc[3].Should().Be(1000);
            dc[4].Should().Be(10000);
            dc.Prod(0).Should().Be(111);
            dc.Prod(1).Should().Be(111);
            dc.Prod(2).Should().Be(111);
            dc.Prod(3).Should().Be(1000);
            dc.Prod(4).Should().Be(10000);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,true,false,false },
                {true,true,true,false,false },
                {true,true,true,false,false },
                {false,false,false,true,false },
                {false,false,false,false,true },
            });

            dc.Link(2, 3);
            dc[0].Should().Be(1);
            dc[1].Should().Be(10);
            dc[2].Should().Be(100);
            dc[3].Should().Be(1000);
            dc[4].Should().Be(10000);
            dc.Prod(0).Should().Be(1111);
            dc.Prod(1).Should().Be(1111);
            dc.Prod(2).Should().Be(1111);
            dc.Prod(3).Should().Be(1111);
            dc.Prod(4).Should().Be(10000);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,true,true,false },
                {true,true,true,true,false },
                {true,true,true,true,false },
                {true,true,true,true,false },
                {false,false,false,false,true },
            });

            dc.Link(3, 4);
            dc[0].Should().Be(1);
            dc[1].Should().Be(10);
            dc[2].Should().Be(100);
            dc[3].Should().Be(1000);
            dc[4].Should().Be(10000);
            dc.Prod(0).Should().Be(11111);
            dc.Prod(1).Should().Be(11111);
            dc.Prod(2).Should().Be(11111);
            dc.Prod(3).Should().Be(11111);
            dc.Prod(4).Should().Be(11111);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
            });

            dc.Link(0, 4);
            dc[0].Should().Be(1);
            dc[1].Should().Be(10);
            dc[2].Should().Be(100);
            dc[3].Should().Be(1000);
            dc[4].Should().Be(10000);
            dc.Prod(0).Should().Be(11111);
            dc.Prod(1).Should().Be(11111);
            dc.Prod(2).Should().Be(11111);
            dc.Prod(3).Should().Be(11111);
            dc.Prod(4).Should().Be(11111);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
            });

            dc.Cut(1, 2);
            dc[0].Should().Be(1);
            dc[1].Should().Be(10);
            dc[2].Should().Be(100);
            dc[3].Should().Be(1000);
            dc[4].Should().Be(10000);
            dc.Prod(0).Should().Be(11111);
            dc.Prod(1).Should().Be(11111);
            dc.Prod(2).Should().Be(11111);
            dc.Prod(3).Should().Be(11111);
            dc.Prod(4).Should().Be(11111);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
            });

            dc.Cut(3, 4);
            dc[0].Should().Be(1);
            dc[1].Should().Be(10);
            dc[2].Should().Be(100);
            dc[3].Should().Be(1000);
            dc[4].Should().Be(10000);
            dc.Prod(0).Should().Be(10011);
            dc.Prod(1).Should().Be(10011);
            dc.Prod(2).Should().Be(1100);
            dc.Prod(3).Should().Be(1100);
            dc.Prod(4).Should().Be(10011);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,false,false,true },
                {true,true,false,false,true },
                {false,false,true,true,false },
                {false,false,true,true,false },
                {true,true,false,false,true },
            });

            dc.Apply(1, 100000);
            dc[0].Should().Be(1);
            dc[1].Should().Be(100010);
            dc[2].Should().Be(100);
            dc[3].Should().Be(1000);
            dc[4].Should().Be(10000);
            dc.Prod(0).Should().Be(110011);
            dc.Prod(1).Should().Be(110011);
            dc.Prod(2).Should().Be(1100);
            dc.Prod(3).Should().Be(1100);
            dc.Prod(4).Should().Be(110011);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,false,false,true },
                {true,true,false,false,true },
                {false,false,true,true,false },
                {false,false,true,true,false },
                {true,true,false,false,true },
            });

            dc.Link(1, 4);
            dc[0].Should().Be(1);
            dc[1].Should().Be(100010);
            dc[2].Should().Be(100);
            dc[3].Should().Be(1000);
            dc[4].Should().Be(10000);
            dc.Prod(0).Should().Be(110011);
            dc.Prod(1).Should().Be(110011);
            dc.Prod(2).Should().Be(1100);
            dc.Prod(3).Should().Be(1100);
            dc.Prod(4).Should().Be(110011);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,false,false,true },
                {true,true,false,false,true },
                {false,false,true,true,false },
                {false,false,true,true,false },
                {true,true,false,false,true },
            });

            dc.Link(3, 4);
            dc[0].Should().Be(1);
            dc[1].Should().Be(100010);
            dc[2].Should().Be(100);
            dc[3].Should().Be(1000);
            dc[4].Should().Be(10000);
            dc.Prod(0).Should().Be(111111);
            dc.Prod(1).Should().Be(111111);
            dc.Prod(2).Should().Be(111111);
            dc.Prod(3).Should().Be(111111);
            dc.Prod(4).Should().Be(111111);
            ShouldConnectEqual(new bool[5, 5]
            {
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
                {true,true,true,true,true },
            });
        }
        struct SolveOp : ISegtreeOperator<long>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public long Operate(long x, long y) => x + y;
            public long Identity => default;
        }
    }
}
