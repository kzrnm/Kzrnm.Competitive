using AtCoder;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.Testing.Graph;

public class DynamicConnectivityOpTests
{
    [Test, MultipleAssertions]
    public async Task Solve()
    {
        var dc = new DynamicConnectivity<long, SolveOp>(5);
        async Task ShouldConnectEqual(bool[,] connect)
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    await dc.Same(i, j).Should().BeEqualTo(connect[i, j]);
        }
        dc[0] = 1;
        dc[1] = 10;
        dc[2] = 100;
        dc[3] = 1000;
        dc[4] = 10000;
        await dc[0].Should().BeEqualTo(1);
        await dc[1].Should().BeEqualTo(10);
        await dc[2].Should().BeEqualTo(100);
        await dc[3].Should().BeEqualTo(1000);
        await dc[4].Should().BeEqualTo(10000);
        await dc.Prod(0).Should().BeEqualTo(1);
        await dc.Prod(1).Should().BeEqualTo(10);
        await dc.Prod(2).Should().BeEqualTo(100);
        await dc.Prod(3).Should().BeEqualTo(1000);
        await dc.Prod(4).Should().BeEqualTo(10000);
        await ShouldConnectEqual(new bool[5, 5]
           {
            {true,false,false,false,false },
            {false,true,false,false,false },
            {false,false,true,false,false },
            {false,false,false,true,false },
            {false,false,false,false,true },
           });

        dc.Link(0, 1);
        await dc[0].Should().BeEqualTo(1);
        await dc[1].Should().BeEqualTo(10);
        await dc[2].Should().BeEqualTo(100);
        await dc[3].Should().BeEqualTo(1000);
        await dc[4].Should().BeEqualTo(10000);
        await dc.Prod(0).Should().BeEqualTo(11);
        await dc.Prod(1).Should().BeEqualTo(11);
        await dc.Prod(2).Should().BeEqualTo(100);
        await dc.Prod(3).Should().BeEqualTo(1000);
        await dc.Prod(4).Should().BeEqualTo(10000);
        await ShouldConnectEqual(new bool[5, 5]
           {
            {true,true,false,false,false },
            {true,true,false,false,false },
            {false,false,true,false,false },
            {false,false,false,true,false },
            {false,false,false,false,true },
           });

        dc.Link(1, 2);
        await dc[0].Should().BeEqualTo(1);
        await dc[1].Should().BeEqualTo(10);
        await dc[2].Should().BeEqualTo(100);
        await dc[3].Should().BeEqualTo(1000);
        await dc[4].Should().BeEqualTo(10000);
        await dc.Prod(0).Should().BeEqualTo(111);
        await dc.Prod(1).Should().BeEqualTo(111);
        await dc.Prod(2).Should().BeEqualTo(111);
        await dc.Prod(3).Should().BeEqualTo(1000);
        await dc.Prod(4).Should().BeEqualTo(10000);
        await ShouldConnectEqual(new bool[5, 5]
           {
            {true,true,true,false,false },
            {true,true,true,false,false },
            {true,true,true,false,false },
            {false,false,false,true,false },
            {false,false,false,false,true },
           });

        dc.Link(2, 3);
        await dc[0].Should().BeEqualTo(1);
        await dc[1].Should().BeEqualTo(10);
        await dc[2].Should().BeEqualTo(100);
        await dc[3].Should().BeEqualTo(1000);
        await dc[4].Should().BeEqualTo(10000);
        await dc.Prod(0).Should().BeEqualTo(1111);
        await dc.Prod(1).Should().BeEqualTo(1111);
        await dc.Prod(2).Should().BeEqualTo(1111);
        await dc.Prod(3).Should().BeEqualTo(1111);
        await dc.Prod(4).Should().BeEqualTo(10000);
        await ShouldConnectEqual(new bool[5, 5]
           {
            {true,true,true,true,false },
            {true,true,true,true,false },
            {true,true,true,true,false },
            {true,true,true,true,false },
            {false,false,false,false,true },
           });

        dc.Link(3, 4);
        await dc[0].Should().BeEqualTo(1);
        await dc[1].Should().BeEqualTo(10);
        await dc[2].Should().BeEqualTo(100);
        await dc[3].Should().BeEqualTo(1000);
        await dc[4].Should().BeEqualTo(10000);
        await dc.Prod(0).Should().BeEqualTo(11111);
        await dc.Prod(1).Should().BeEqualTo(11111);
        await dc.Prod(2).Should().BeEqualTo(11111);
        await dc.Prod(3).Should().BeEqualTo(11111);
        await dc.Prod(4).Should().BeEqualTo(11111);
        await ShouldConnectEqual(new bool[5, 5]
           {
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
           });

        dc.Link(0, 4);
        await dc[0].Should().BeEqualTo(1);
        await dc[1].Should().BeEqualTo(10);
        await dc[2].Should().BeEqualTo(100);
        await dc[3].Should().BeEqualTo(1000);
        await dc[4].Should().BeEqualTo(10000);
        await dc.Prod(0).Should().BeEqualTo(11111);
        await dc.Prod(1).Should().BeEqualTo(11111);
        await dc.Prod(2).Should().BeEqualTo(11111);
        await dc.Prod(3).Should().BeEqualTo(11111);
        await dc.Prod(4).Should().BeEqualTo(11111);
        await ShouldConnectEqual(new bool[5, 5]
           {
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
           });

        dc.Cut(1, 2);
        await dc[0].Should().BeEqualTo(1);
        await dc[1].Should().BeEqualTo(10);
        await dc[2].Should().BeEqualTo(100);
        await dc[3].Should().BeEqualTo(1000);
        await dc[4].Should().BeEqualTo(10000);
        await dc.Prod(0).Should().BeEqualTo(11111);
        await dc.Prod(1).Should().BeEqualTo(11111);
        await dc.Prod(2).Should().BeEqualTo(11111);
        await dc.Prod(3).Should().BeEqualTo(11111);
        await dc.Prod(4).Should().BeEqualTo(11111);
        await ShouldConnectEqual(new bool[5, 5]
           {
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
           });

        dc.Cut(3, 4);
        await dc[0].Should().BeEqualTo(1);
        await dc[1].Should().BeEqualTo(10);
        await dc[2].Should().BeEqualTo(100);
        await dc[3].Should().BeEqualTo(1000);
        await dc[4].Should().BeEqualTo(10000);
        await dc.Prod(0).Should().BeEqualTo(10011);
        await dc.Prod(1).Should().BeEqualTo(10011);
        await dc.Prod(2).Should().BeEqualTo(1100);
        await dc.Prod(3).Should().BeEqualTo(1100);
        await dc.Prod(4).Should().BeEqualTo(10011);
        await ShouldConnectEqual(new bool[5, 5]
           {
            {true,true,false,false,true },
            {true,true,false,false,true },
            {false,false,true,true,false },
            {false,false,true,true,false },
            {true,true,false,false,true },
           });

        dc.Apply(1, 100000);
        await dc[0].Should().BeEqualTo(1);
        await dc[1].Should().BeEqualTo(100010);
        await dc[2].Should().BeEqualTo(100);
        await dc[3].Should().BeEqualTo(1000);
        await dc[4].Should().BeEqualTo(10000);
        await dc.Prod(0).Should().BeEqualTo(110011);
        await dc.Prod(1).Should().BeEqualTo(110011);
        await dc.Prod(2).Should().BeEqualTo(1100);
        await dc.Prod(3).Should().BeEqualTo(1100);
        await dc.Prod(4).Should().BeEqualTo(110011);
        await ShouldConnectEqual(new bool[5, 5]
           {
            {true,true,false,false,true },
            {true,true,false,false,true },
            {false,false,true,true,false },
            {false,false,true,true,false },
            {true,true,false,false,true },
           });

        dc.Link(1, 4);
        await dc[0].Should().BeEqualTo(1);
        await dc[1].Should().BeEqualTo(100010);
        await dc[2].Should().BeEqualTo(100);
        await dc[3].Should().BeEqualTo(1000);
        await dc[4].Should().BeEqualTo(10000);
        await dc.Prod(0).Should().BeEqualTo(110011);
        await dc.Prod(1).Should().BeEqualTo(110011);
        await dc.Prod(2).Should().BeEqualTo(1100);
        await dc.Prod(3).Should().BeEqualTo(1100);
        await dc.Prod(4).Should().BeEqualTo(110011);
        await ShouldConnectEqual(new bool[5, 5]
           {
            {true,true,false,false,true },
            {true,true,false,false,true },
            {false,false,true,true,false },
            {false,false,true,true,false },
            {true,true,false,false,true },
           });

        dc.Link(3, 4);
        await dc[0].Should().BeEqualTo(1);
        await dc[1].Should().BeEqualTo(100010);
        await dc[2].Should().BeEqualTo(100);
        await dc[3].Should().BeEqualTo(1000);
        await dc[4].Should().BeEqualTo(10000);
        await dc.Prod(0).Should().BeEqualTo(111111);
        await dc.Prod(1).Should().BeEqualTo(111111);
        await dc.Prod(2).Should().BeEqualTo(111111);
        await dc.Prod(3).Should().BeEqualTo(111111);
        await dc.Prod(4).Should().BeEqualTo(111111);
        await ShouldConnectEqual(new bool[5, 5]
           {
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
           });
    }
    readonly struct SolveOp : ISegtreeOperator<long>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long Operate(long x, long y) => x + y;
        public long Identity => default;
    }
}