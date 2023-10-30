using AtCoder;
using AtCoder.Extension;

namespace Kzrnm.Competitive.Analyzer.Example;

public static class Example
{
    public static (long[], ulong[]) KZCOMPETITIVE0001(int v)
    {
        var u = (uint)v;
#pragma warning disable KZCOMPETITIVE0001 // 32bit整数の乗算演算子が64bit整数に代入されています
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
#pragma warning restore KZCOMPETITIVE0001 // 32bit整数の乗算演算子が64bit整数に代入されています

        return (toLong, toULong);
    }

#pragma warning disable KZCOMPETITIVE0003 // Operator method doesn't have MethodImpl attribute
    readonly struct KZCOMPETITIVE0003 : ISegtreeOperator<int>
    {
        public int Identity { get; }

        public int Operate(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
#pragma warning restore KZCOMPETITIVE0003 // Operator method doesn't have MethodImpl attribute

    public static object? KZCOMPETITIVE0004(int v)
    {
        //new Impl<Op>();
        
        //var seg = new Segtree<uint, Op>(v);
        
        //3.Other<Op>(2);

        return null;
    }
}

[IsOperator]
public interface IInterface
{
    static abstract int Merge(int a, int b);
}

record Impl<TOp> where TOp : IInterface;

static class Extension
{
    public static int Other<TOp>(this int num, short other) where TOp : IInterface => TOp.Merge(num, other);
}