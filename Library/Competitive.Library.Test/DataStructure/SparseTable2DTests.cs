using AtCoder.Internal;
using System;
using System.Runtime.InteropServices;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class SparseTable2DTests
    {
        Random rnd = new(42);

        [Fact]
        public void Invalid()
        {
            ((Action)(() => _ = new SparseTable2D<short, MinOp>([]))).ShouldThrow<ContractAssertException>();
            ((Action)(() => _ = new SparseTable2D<short, MinOp>([[]]))).ShouldThrow<ContractAssertException>();

            var s = new SparseTable2D<short, MinOp>(Grid.Create(new[]
            {
                new short[]{ 1,2,3 },
                [4,5,6],
                [7,8,9],
                [10,11,12],
            }));
            Should.Throw<ContractAssertException>(() => s[-1..3][0..1]);
            Should.Throw<ContractAssertException>(() => s[0..5][0..1]);
            Should.Throw<ContractAssertException>(() => s[0..1][-1..]);
            Should.Throw<ContractAssertException>(() => s[0..1][..5]);
            for (var i = 0; i < 5; i++)
            {
                Should.Throw<ContractAssertException>(() => s[i..i][0..1]);
                Should.Throw<ContractAssertException>(() => s[0..1][i..i]);
            }


            Should.Throw<ContractAssertException>(() => s.Prod(-1, 3, 0, 1));
            Should.Throw<ContractAssertException>(() => s.Prod(0, 5, 0, 1));
            Should.Throw<ContractAssertException>(() => s.Prod(0, 1, -1, 1));
            Should.Throw<ContractAssertException>(() => s.Prod(0, 1, 0, 5));
            for (var i = 0; i < 5; i++)
            {
                Should.Throw<ContractAssertException>(() => s.Prod(i, i, 0, 1));
                Should.Throw<ContractAssertException>(() => s.Prod(0, 1, i, i));
            }
        }

        [Fact]
        public void Native()
        {
            for (int lenH = 1; lenH < 10; lenH++)
                for (int lenW = 1; lenW < 10; lenW++)
                {
                    var arr = new short[lenH][];
                    for (int i = 0; i < arr.Length; i++)
                    {
                        arr[i] = new short[lenW];
                        rnd.NextBytes(MemoryMarshal.Cast<short, byte>(arr[i]));
                    }
                    var native = new MinNative(arr);
                    var st = new SparseTable2D<short, MinOp>(arr);

                    for (var lh = 0; lh < lenH; lh++)
                        for (var rh = lh + 1; rh <= lenH; rh++)
                            for (var lw = 0; lw < lenW; lw++)
                                for (var rw = lw + 1; rw <= lenW; rw++)
                                {
                                    var expected = native.Prod(lh, rh, lw, rw);
                                    st[lh..rh][lw..rw].ShouldBe(expected);
                                    st.Prod(lh, rh, lw, rw).ShouldBe(expected);
                                }
                }
        }

        readonly struct MinOp : ISparseTableOperator<short>
        {
            public short Operate(short x, short y) => Math.Min(x, y);
        }
        private class MinNative(short[][] array)
        {

            public short Prod(int lh, int rh, int lw, int rw)
            {
                var min = array[lh][lw];
                for (int i = lh; i < rh; i++)
                    for (int j = lw; j < rw; j++)
                        min = Math.Min(min, array[i][j]);
                return min;
            }
        }
    }
}
