using AtCoder.Internal;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.DataStructure;

public class SparseTableTests
{
    Random rnd = new(42);

    [Test, MultipleAssertions]
    public async Task Invalid()
    {
        await Assert.That(() => _ = new SparseTable<short, MinOp>([])).Throws<ContractAssertException>();

        var s = new SparseTable<short, MinOp>([1, 2, 3]);
        await Assert.That(() => s[-1..3]).Throws<ContractAssertException>();
        await Assert.That(() => s[0..4]).Throws<ContractAssertException>();
        for (var i = 0; i < 5; i++)
            await Assert.That(() => s[i..i]).Throws<ContractAssertException>();
        await Assert.That(() => s.Prod(-1, 3)).Throws<ContractAssertException>();
        await Assert.That(() => s.Prod(0, 4)).Throws<ContractAssertException>();

        for (var i = 0; i < 3; i++)
            for (var j = i + 1; j <= 3; j++)
            {
                await Assert.That(() => s[i..j]).ThrowsNothing();
                await Assert.That(() => s.Prod(i, j)).ThrowsNothing();
            }
    }

    [Test]
    public async Task Native()
    {
        for (int len = 1; len < 50; len++)
        {
            var arr = new short[len];
            rnd.NextBytes(MemoryMarshal.AsBytes(arr.AsSpan()));
            var native = new MinNative(arr);
            var st = new SparseTable<short, MinOp>(arr);

            for (var i = 0; i < len; i++)
                for (var j = i + 1; j <= len; j++)
                    using (Assert.Multiple())
                    {
                        var expected = native.Prod(i, j);
                        await st[i..j].Should().BeEqualTo(expected);
                        await st.Prod(i, j).Should().BeEqualTo(expected);
                    }
        }
    }

    readonly struct MinOp : ISparseTableOperator<short>
    {
        public short Operate(short x, short y) => Math.Min(x, y);
    }
    private class MinNative(short[] array)
    {

        public short Prod(int l, int r)
        {
            var min = array[l];
            for (int i = l + 1; i < r; i++)
                min = Math.Min(min, array[i]);
            return min;
        }
    }
}