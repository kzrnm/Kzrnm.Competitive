
namespace Kzrnm.Competitive.Testing.Graph;

public class DynamicConnectivityTests
{
    [Test, MultipleAssertions]
    public async Task Solve()
    {
        var dc = new DynamicConnectivity(5);
        async Task ShouldConnectEqual(bool[,] connect)
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    await dc.Same(i, j).Should().BeEqualTo(connect[i, j]);
        }

        await ShouldConnectEqual(new bool[5, 5]
        {
            {true,false,false,false,false },
            {false,true,false,false,false },
            {false,false,true,false,false },
            {false,false,false,true,false },
            {false,false,false,false,true },
        });

        dc.Link(0, 1);
        await ShouldConnectEqual(new bool[5, 5]
        {
            {true,true,false,false,false },
            {true,true,false,false,false },
            {false,false,true,false,false },
            {false,false,false,true,false },
            {false,false,false,false,true },
        });

        dc.Link(1, 2);
        await ShouldConnectEqual(new bool[5, 5]
        {
            {true,true,true,false,false },
            {true,true,true,false,false },
            {true,true,true,false,false },
            {false,false,false,true,false },
            {false,false,false,false,true },
        });

        dc.Link(2, 3);
        await ShouldConnectEqual(new bool[5, 5]
        {
            {true,true,true,true,false },
            {true,true,true,true,false },
            {true,true,true,true,false },
            {true,true,true,true,false },
            {false,false,false,false,true },
        });

        dc.Link(3, 4);
        await ShouldConnectEqual(new bool[5, 5]
        {
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
        });

        dc.Link(0, 4);
        await ShouldConnectEqual(new bool[5, 5]
        {
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
        });

        dc.Cut(1, 2);
        await ShouldConnectEqual(new bool[5, 5]
        {
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
        });

        dc.Cut(3, 4);
        await ShouldConnectEqual(new bool[5, 5]
        {
            {true,true,false,false,true },
            {true,true,false,false,true },
            {false,false,true,true,false },
            {false,false,true,true,false },
            {true,true,false,false,true },
        });

        dc.Link(1, 4);
        await ShouldConnectEqual(new bool[5, 5]
        {
            {true,true,false,false,true },
            {true,true,false,false,true },
            {false,false,true,true,false },
            {false,false,true,true,false },
            {true,true,false,false,true },
        });

        dc.Link(3, 4);
        await ShouldConnectEqual(new bool[5, 5]
        {
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
            {true,true,true,true,true },
        });
    }
}