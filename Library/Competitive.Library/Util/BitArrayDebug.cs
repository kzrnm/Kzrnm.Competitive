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
                BitArray single => new[] { single },
                BitArray[] arr => arr,
                _ => throw new NotSupportedException(),
            };
        }
        [DebuggerBrowsable(Never)]
        BitArray[] bits;


        [DebuggerDisplay("{" + nameof(value) + ",nq}", Name = "{" + nameof(key) + ",nq}")]
        public struct DebugItem
        {
            public DebugItem(bool[] value)
            {
                var chrs = new char[value.Length];
                for (int i = 0; i < chrs.Length; i++)
                {
                    chrs[i] = value[i] ? '1' : '0';
                }
                key = new string(chrs);
                this.value = (key, value);
            }
            [DebuggerBrowsable(Never)]
            internal string key;
            [DebuggerBrowsable(RootHidden)]
            internal (string Bits, bool[] Array) value;
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
                    items[i] = new DebugItem(brr);
                }
                return items;
            }
        }
    }
}
