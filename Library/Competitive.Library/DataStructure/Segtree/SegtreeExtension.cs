using AtCoder;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class SegtreeExtension
    {
        /// <summary>
        /// 現在のセグ木の中身を配列にコピーして返します。
        /// </summary>
        [凾(256)]
        public static TValue[] ToArray<TValue, TOp>(this Segtree<TValue, TOp> seg)
            where TOp : struct, ISegtreeOperator<TValue>
        {
            var data = seg.d;
            return data.AsSpan(data.Length / 2, seg.Length).ToArray();
        }
    }
}
