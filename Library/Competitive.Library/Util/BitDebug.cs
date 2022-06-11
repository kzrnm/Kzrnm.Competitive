using System;
using System.Diagnostics;
using System.Numerics;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 配列のインデックスを 2 進数表記でデバッガのウォッチ式に表示します。
    /// </summary>
    public class BitDebug
    {
        public BitDebug(Array array)
        {
            Array = array;
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Array Array;


        [DebuggerDisplay("{" + nameof(value) + ",nq}", Name = "{" + nameof(key) + ",nq}")]
        public struct DebugItem
        {
            public DebugItem(int index, int len, object value)
            {
                this.index = index;
                key = $"{Convert.ToString(index, 2).PadLeft(len, '0')} [{index}]";
                this.value = value;
            }
            public readonly string key;
            public readonly int index;
            public readonly object value;
        }
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public DebugItem[] Items
        {
            get
            {
                var items = new DebugItem[Array.Length];
                var len = BitOperations.Log2((uint)Array.Length - 1) + 1;
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new DebugItem(i, len, Array.GetValue(i));
                }
                return items;
            }
        }
    }
}
