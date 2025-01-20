using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 座標圧縮を行う
    /// </summary>
    public static class ZahyoCompress
    {
        /// <summary>
        /// 座標圧縮を行う
        /// </summary>
        [凾(256)]
        public static ZahyoCompress<T> Create<T>(IEnumerable<T> orig) => new ZahyoCompress<T>(orig).Compress();

        /// <summary>
        /// 座標圧縮を行う
        /// </summary>
        [凾(256)]
        public static ZahyoCompress<T> Create<T>(ReadOnlySpan<T> orig) => new ZahyoCompress<T>(orig).Compress();
        /// <summary>
        /// 座標圧縮を行う
        /// </summary>
        [凾(256)]
        public static ZahyoCompress<T> Create<T>(Span<T> orig) => new ZahyoCompress<T>(orig).Compress();
        /// <summary>
        /// 座標圧縮を行う
        /// </summary>
        [凾(256)]
        public static ZahyoCompress<T> Create<T>(T[] orig) => new ZahyoCompress<T>((ReadOnlySpan<T>)orig).Compress();

        /// <summary>
        /// 座標圧縮後の値に置換した配列を取得する
        /// </summary>
        [凾(256)]
        public static int[] CompressedArray<T>(ReadOnlySpan<T> orig) => new ZahyoCompress<T>(orig).Replace(orig);

        /// <summary>
        /// 座標圧縮後の値に置換した配列を取得する
        /// </summary>
        [凾(256)]
        public static int[] CompressedArray<T>(Span<T> orig) => CompressedArray((ReadOnlySpan<T>)orig);
        /// <summary>
        /// 座標圧縮後の値に置換した配列を取得する
        /// </summary>
        [凾(256)]
        public static int[] CompressedArray<T>(T[] orig) => CompressedArray((ReadOnlySpan<T>)orig);
    }
    /// <summary>
    /// 座標圧縮を行う
    /// </summary>
    public class ZahyoCompress<T>
    {
        public ZahyoCompress() { data = new(); }
        public ZahyoCompress(T[] arr) : this(arr.AsEnumerable()) { }
        public ZahyoCompress(IEnumerable<T> collection) { data = new SortedSet<T>(collection); }
        public ZahyoCompress(ReadOnlySpan<T> span) : this(span.ToArray()) { }

        private readonly SortedSet<T> data;
        private int version;
        private int lastCompressVesion = -1;
        private IComparer<T> lastComparer;
        [凾(256)]
        public void Add(T item)
        {
            if (data.Add(item))
                ++version;
        }

        /// <summary>
        /// 登録されている値から座標圧縮する。前に呼び出されたときと異なるならば作り直す。
        /// </summary>
        /// <returns></returns>
        [凾(256)]
        public ZahyoCompress<T> Compress() => Compress(null);

        /// <summary>
        /// 登録されている値から座標圧縮する。前に呼び出されたときと異なるならば作り直す。
        /// </summary>
        /// <returns></returns>
        [凾(256)]
        public ZahyoCompress<T> Compress(IComparer<T> comparer)
        {
            if (typeof(T) == typeof(string))
                comparer ??= (IComparer<T>)(object)StringComparer.Ordinal;
            if (lastCompressVesion == version
                && (comparer == lastComparer || comparer.Equals(lastComparer)))
                return this;
            lastCompressVesion = version;
            lastComparer = comparer;

            var ox = data.ToArray();
            Array.Sort(ox, comparer);
            var zip = new SortedDictionary<T, int>(comparer);
            for (int i = 0; i < ox.Length; i++)
                zip[ox[i]] = i;

            NewTable = zip;
            Original = ox;
            return this;
        }

        /// <summary>
        /// 座標圧縮後の要素数
        /// </summary>
        public int Count => Original.Length;

        /// <summary>
        /// 座標圧縮後のインデックス
        /// </summary>
        public SortedDictionary<T, int> NewTable { private set; get; }

        /// <summary>
        /// インデックスに対応する元の値
        /// </summary>
        public T[] Original { private set; get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [凾(256)]
        public void Deconstruct(out SortedDictionary<T, int> newTable, out T[] original)
        {
            newTable = NewTable;
            original = Original;
        }

        /// <summary>
        /// 座標圧縮後の値に置換した配列を返します
        /// </summary>
        [凾(256)]
        public int[] Replace(T[] orig) => Replace((ReadOnlySpan<T>)orig);

        /// <summary>
        /// 座標圧縮後の値に置換した配列を返します
        /// </summary>
        [凾(256)]
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
    }
}
