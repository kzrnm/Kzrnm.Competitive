using AtCoder.Internal;
using System;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class SparseTableTests
    {
        Random rnd = new(42);

        [Fact]
        public void Invalid()
        {
            ((Action)(() => _ = new SparseTable<short, MinOp>([]))).Should().Throw<ContractAssertException>();

            var s = new SparseTable<short, MinOp>([1, 2, 3]);
            s.Invoking(s => s[-1..3]).Should().Throw<ContractAssertException>();
            s.Invoking(s => s[0..4]).Should().Throw<ContractAssertException>();
            for (var i = 0; i < 5; i++)
                s.Invoking(s => s[i..i]).Should().Throw<ContractAssertException>();
            s.Invoking(s => s.Prod(-1, 3)).Should().Throw<ContractAssertException>();
            s.Invoking(s => s.Prod(0, 4)).Should().Throw<ContractAssertException>();

            for (var i = 0; i < 3; i++)
                for (var j = i + 1; j <= 3; j++)
                {
                    s.Invoking(s => s[i..j]).Should().NotThrow();
                    s.Invoking(s => s.Prod(i, j)).Should().NotThrow();
                }
        }

        [Fact]
        public void Native()
        {
            for (int len = 1; len < 50; len++)
            {
                var arr = new short[len];
                rnd.NextBytes(MemoryMarshal.Cast<short, byte>(arr));
                var native = new MinNative(arr);
                var st = new SparseTable<short, MinOp>(arr);

                for (var i = 0; i < len; i++)
                    for (var j = i + 1; j <= len; j++)
                    {
                        var expected = native.Prod(i, j);
                        st[i..j].Should().Be(expected);
                        st.Prod(i, j).Should().Be(expected);
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
}
