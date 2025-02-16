using System;
using System.Diagnostics;
using System.Numerics;

namespace Kzrnm.Competitive
{
    using static DebuggerBrowsableState;
    /// <summary>
    /// 配列のインデックスを 2 進数表記でデバッガのウォッチ式に表示します。
    /// </summary>
    public class BitDebug
    {
        public BitDebug(Array array)
        {
            a = array;
        }
        [DebuggerBrowsable(Never)]
        Array a;


        [DebuggerDisplay("{" + nameof(Value) + ",nq}", Name = "{" + nameof(Key) + ",nq}")]
        public record struct DebugItem(
            [property: DebuggerBrowsable(Never)] string Key,
            [property: DebuggerBrowsable(RootHidden)] object Value)
        {
            public DebugItem(int index, int len, object value) : this($"{Convert.ToString(index, 2).PadLeft(len, '0')} [{index}]", value) { }
        }
        [DebuggerBrowsable(RootHidden)]
        public DebugItem[] Items
        {
            get
            {
                var items = new DebugItem[a.Length];
                var len = BitOperations.Log2((uint)a.Length - 1) + 1;
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new DebugItem(i, len, a.GetValue(i));
                }
                return items;
            }
        }
    }
}
