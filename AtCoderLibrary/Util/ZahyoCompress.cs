using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AtCoder
{
    /// <summary>
    /// 座標圧縮を行う
    /// </summary>
    public static class ZahyoCompress
    {
        /// <summary>
        /// 座標圧縮を行う
        /// </summary>
        public static ZahyoCompress<T> Create<T>(IEnumerable<T> orig) => new ZahyoCompress<T>(orig).Compress();

        /// <summary>
        /// 座標圧縮を行う
        /// </summary>
        public static ZahyoCompress<T> Create<T>(ReadOnlySpan<T> orig) => new ZahyoCompress<T>(orig).Compress();
    }
    /// <summary>
    /// 座標圧縮を行う
    /// </summary>
    public class ZahyoCompress<T>
    {
        public ZahyoCompress() { data = new HashSet<T>(); }
        public ZahyoCompress(IEnumerable<T> collection) { data = new HashSet<T>(collection); }
        public ZahyoCompress(ReadOnlySpan<T> span)
        {
            data = new HashSet<T>(span.Length);
            foreach (var item in span)
                data.Add(item);
        }

        private readonly HashSet<T> data;
        private int version;
        private int lastCompressVesion = -1;
        private IComparer<T> lastComparer;
        public void Add(T item)
        {
            if (data.Add(item))
                ++version;
        }

        /// <summary>
        /// 登録されている値から座標圧縮する。前に呼び出されたときと異なるならば作り直す。
        /// </summary>
        /// <returns></returns>
        public ZahyoCompress<T> Compress() => Compress(Comparer<T>.Default);

        /// <summary>
        /// 登録されている値から座標圧縮する。前に呼び出されたときと異なるならば作り直す。
        /// </summary>
        /// <returns></returns>
        public ZahyoCompress<T> Compress(IComparer<T> comparer)
        {
            if (lastCompressVesion == version && comparer.Equals(lastComparer))
                return this;
            lastCompressVesion = version;
            lastComparer = comparer;

            var ox = data.ToArray();
            Array.Sort(ox, comparer);
            var zip = new Dictionary<T, int>();
            for (int i = 0; i < ox.Length; i++)
                zip[ox[i]] = i;

            NewTable = zip;
            Original = ox;
            return this;
        }

        /// <summary>
        /// 座標圧縮後のインデックスを取得する
        /// </summary>
        public Dictionary<T, int> NewTable { private set; get; }

        /// <summary>
        /// インデックスに対応する元の値を保持する
        /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
        public T[] Original { private set; get; }
#pragma warning restore CA1819 // Properties should not return arrays

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out Dictionary<T, int> newTable, out T[] original)
        {
            newTable = NewTable;
            original = Original;
        }

        /// <summary>
        /// 座標圧縮後の値に置換した配列を取得する
        /// </summary>
        public int[] Replace(T[] orig) => Replace((ReadOnlySpan<T>)orig);

        /// <summary>
        /// 座標圧縮後の値に置換した配列を取得する
        /// </summary>
        public int[] Replace(ReadOnlySpan<T> orig)
        {
            if (lastCompressVesion != version)
                Compress();
            var res = new int[orig.Length];
            var zip = NewTable;
            for (int i = 0; i < res.Length; i++)
                res[i] = zip[orig[i]];
            return res;
        }

        /// <summary>
        /// 座標圧縮後の値に置換した配列を取得する
        /// </summary>
        public static int[] CompressedArray(ReadOnlySpan<T> orig) => new ZahyoCompress<T>(orig).Replace(orig);
    }
}
