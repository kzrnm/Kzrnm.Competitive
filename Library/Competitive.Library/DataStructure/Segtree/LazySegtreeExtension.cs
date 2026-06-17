using AtCoder;
using System;

namespace Kzrnm.Competitive
{
    public static class LazySegtreeExtension
    {
        /// <summary>
        /// 現在のセグ木の中身を配列にコピーして返します。
        /// </summary>
        public static T[] ToArray<T, F, TOp>(this LazySegtree<T, F, TOp> seg)
            where TOp : struct, ILazySegtreeOperator<T, F>
        {
            var data = seg.d;
            var p = data.Length / 2;
            for (int i = 0; i < p; i++)
                seg.Push(i);
            return data.AsSpan(p, seg.Length).ToArray();
        }
    }
}
