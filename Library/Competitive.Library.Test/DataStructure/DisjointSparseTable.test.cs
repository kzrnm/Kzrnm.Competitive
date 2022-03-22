using AtCoder;
using AtCoder.Internal;
using FluentAssertions;
using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class DisjointSparseTableTests
    {
        Random rnd = new(42);

        [Fact]
        public void Invalid()
        {
            ((Action)(() => new DisjointSparseTable<short, MinOp>(Array.Empty<short>()))).Should().Throw<ContractAssertException>();

            var s = new DisjointSparseTable<short, MinOp>(new short[] { 1, 2, 3 });
            s.Invoking(s => s[-1..3]).Should().Throw<ContractAssertException>();
            s.Invoking(s => s[0..4]).Should().Throw<ContractAssertException>();
            for (var i = 0; i < 5; i++)
                s.Invoking(s => s[0..0]).Should().Throw<ContractAssertException>();
            s.Invoking(s => s.Prod(-1, 3)).Should().Throw<ContractAssertException>();
            s.Invoking(s => s.Prod(0, 4)).Should().Throw<ContractAssertException>();
            for (var i = 0; i < 5; i++)
                s.Invoking(s => s.Prod(0, 0)).Should().Throw<ContractAssertException>();


            for (var i = 0; i < 3; i++)
                for (var j = i + 1; j <= 3; j++)
                {
                    s.Invoking(s => s[i..j]).Should().NotThrow();
                    s.Invoking(s => s.Prod(i, j)).Should().NotThrow();
                }
        }

        [Fact]
        public void NativeMin()
        {
            for (int len = 1; len < 260; len++)
            {
                var arr = new short[len];
                rnd.NextBytes(MemoryMarshal.Cast<short, byte>(arr));
                var native = new MinNative(arr);
                var st = new DisjointSparseTable<short, MinOp>(arr);

                for (var i = 0; i < len; i++)
                    for (var j = i + 1; j <= len; j++)
                    {
                        var expected = native.Prod(i, j);
                        st[i..j].Should().Be(expected);
                        st.Prod(i, j).Should().Be(expected);
                    }
            }
        }

        struct MinOp : ISparseTableOperator<short>
        {
            public short Operate(short x, short y) => Math.Min(x, y);
        }
        private class MinNative
        {
            private readonly short[] array;
            public MinNative(short[] array)
            {
                this.array = array;
            }

            public short Prod(int l, int r)
            {
                var min = array[l];
                for (int i = l + 1; i < r; i++)
                    min = Math.Min(min, array[i]);
                return min;
            }
        }


        [Fact]
        public void NativeSum()
        {
            for (int len = 1; len < 260; len++)
            {
                var arr = new uint[len];
                rnd.NextBytes(MemoryMarshal.Cast<uint, byte>(arr));
                for (int i = 0; i < arr.Length; i++)
                    arr[i] &= (1 << 16) - 1;
                var sums = new Sums<uint, UIntOperator>(arr);
                var st = new DisjointSparseTable<uint, SumOp>(arr);

                for (var i = 0; i < len; i++)
                    for (var j = i + 1; j <= len; j++)
                    {
                        var expected = sums[i..j];
                        st[i..j].Should().Be(expected);
                        st.Prod(i, j).Should().Be(expected);
                    }
            }
        }
        struct SumOp : ISparseTableOperator<uint>
        {
            public uint Operate(uint x, uint y) => x + y;
        }
    }
}
