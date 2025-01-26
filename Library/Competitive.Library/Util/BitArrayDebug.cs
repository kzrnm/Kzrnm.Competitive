using System;
using System.Diagnostics;
using BitArray = System.Collections.BitArray;

namespace Kzrnm.Competitive
{
    using static DebuggerBrowsableState;
    /// <summary>
    /// <see cref="BitArray"/> や <see cref="BitArray"/>[] を 2 進数表記でデバッガのウォッチ式に表示します。
    /// </summary>
    public class BitArrayDebug
    {
        public BitArrayDebug(object bits)
        {
            this.bits = bits switch
            {
                BitArray single => new BitArrayDebugImpl(single),
                BitArray[] arr => new BitArrayArrayDebug(arr),
                _ => throw new NotSupportedException(),
            };
        }
        [DebuggerBrowsable(RootHidden)]
        public object bits;

        public override string ToString()
        {
            return bits switch
            {
                BitArrayDebugImpl d => d.Value.ToString(),
                BitArrayArrayDebug arr => $"System.Collections.BitArray[{arr.bits.Length}]",
                _ => "Unknown",
            };
        }

        public static object ToDebugString(BitArray bits)
        {
            var brr = new bool[bits.Length];
            bits.CopyTo(brr, 0);
            var chrs = new char[brr.Length];
            for (int i = 0; i < chrs.Length; i++)
            {
                chrs[i] = brr[i] ? '1' : '0';
            }
            return new DebugTuple(brr, new string(chrs));
        }

        private class BitArrayDebugImpl
        {
            public BitArrayDebugImpl(BitArray b) { bits = b; }
            [DebuggerBrowsable(Never)]
            readonly BitArray bits;
            public override string ToString() => Value.ToString();

            [DebuggerBrowsable(RootHidden)]
            public object Value => ToDebugString(bits);
        }
        private class BitArrayArrayDebug
        {
            public BitArrayArrayDebug(BitArray[] b) { bits = b; }
            [DebuggerBrowsable(Never)]
            public readonly BitArray[] bits;

            [DebuggerDisplay("{" + nameof(value) + ",nq}", Name = "[{" + nameof(index) + ",nq}]")]
            public struct DebugItem
            {
                public DebugItem(int index, bool[] value)
                {
                    var chrs = new char[value.Length];
                    for (int i = 0; i < chrs.Length; i++)
                    {
                        chrs[i] = value[i] ? '1' : '0';
                    }
                    this.index = index;
                    this.value = new DebugTuple(value, new string(chrs));
                }
                [DebuggerBrowsable(Never)]
                internal int index;
                [DebuggerBrowsable(RootHidden)]
                internal DebugTuple value;
            }
            [DebuggerBrowsable(RootHidden)]
            public DebugItem[] Items
            {
                get
                {
                    var items = new DebugItem[bits.Length];
                    for (int i = 0; i < items.Length; i++)
                    {
                        var brr = new bool[bits[i].Length];
                        bits[i].CopyTo(brr, 0);
                        items[i] = new DebugItem(i, brr);
                    }
                    return items;
                }
            }
        }
        public readonly record struct DebugTuple(bool[] Array, string Bits)
        {
            public override string ToString() => $"Length = {Array.Length}, {Bits}";
        }
    }
}