using AtCoder;
using AtCoder.Internal;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// モノイドの範囲演算を移動しながら求める。計算量: O(N)
    /// </summary>
    public class SWAG<T, TOp> where TOp : struct, ISegtreeOperator<T>
    {
        private static TOp op = default;
        private readonly T[] array;
        private int left, right;
        private T front;
        private SimpleList<T> back;
        public SWAG(T[] array)
        {
            this.array = array;
            front = op.Identity;
            back = new SimpleList<T>();
        }
        /// <summary>
        /// <para>[<paramref name="l"/>..<paramref name="r"/>) の範囲クエリの結果を返す。</para>
        /// <para>計算量: O(N)</para>
        /// </summary>
        [凾(256)]
        public T Slide(int l, int r)
        {
            Contract.Assert(l <= r, "Range: l <= r");
            Contract.Assert(left <= l, "左端は使用済みです");
            Contract.Assert(right <= r, "右端は使用済みです");
            Contract.Assert(l < array.Length, "左端が配列の範囲外です");
            Contract.Assert(r <= array.Length, "右端が配列の範囲外です");
            foreach (var v in array.AsSpan()[right..r])
                front = op.Operate(front, v);
            right = r;
            while (left < l)
            {
                if (back.Count == 0)
                {
                    var tmp = op.Identity;
                    for (int u = right - 1; u >= left; --u)
                        back.Add(tmp = op.Operate(array[u], tmp));
                    front = op.Identity;
                }
                back.RemoveLast();
                ++left;
            }
            if (back.Count == 0) return front;
            return op.Operate(back[^1], front);
        }
    }
}
