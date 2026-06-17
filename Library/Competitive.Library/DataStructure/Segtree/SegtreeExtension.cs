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
        public static T[] ToArray<T, TOp>(this Segtree<T, TOp> seg)
            where TOp : struct, ISegtreeOperator<T>
        {
            var data = seg.d;
            return data.AsSpan(data.Length / 2, seg.Length).ToArray();
        }
    }
}
