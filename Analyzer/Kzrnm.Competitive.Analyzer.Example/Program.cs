namespace Kzrnm.Competitive.Analyzer.Example;

public static class Example
{
    public static (long[], ulong[]) KZCOMPETITIVE0001(int v)
    {
        var u = (uint)v;
        var toLong = new long[]
        {
            2 * 4 * v,
            1 << v,
            1 >> v,
            v * 2 * 5,
            v * v,
            v << 5,
            v >> 5,
        };

        var toULong = new ulong[]
        {
            2 * 4 * u,
            1u << v,
            1u >> v,
            u * 2 * 5,
            u * u,
            u << 5,
            u >> 5,
        };

        return (toLong, toULong);
    }
}