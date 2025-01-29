using AtCoder.Internal;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 長さ N の配列に対し、
    /// <list type="bullet">
    /// <item>
    /// <description>要素の区間変更</description>
    /// </item>
    /// <item>
    /// <description>区間の要素の総和</description>
    /// </item>
    /// </list>
    /// <para>を O(log⁡N) で求めることが出来るデータ構造です。</para>
    /// </summary>
    [DebuggerTypeProxy(typeof(FenwickTreeRange<>.DebugView))]
    public class FenwickTreeRange<T>
        where T : INumberBase<T>
    {
        readonly T[] data1, data2;

        public int Length { get; }

        /// <summary>
        /// 長さ <paramref name="n"/> の配列aを持つ <see cref="FenwickTreeRange{T}"/> クラスの新しいインスタンスを作ります。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        /// <param name="n">配列の長さ</param>
        public FenwickTreeRange(int n)
        {
            Length = n;
            data1 = new T[n + 1];
            data2 = new T[n + 1];
        }

        [凾(256)]
        static void Add(T[] data, int p, T w)
        {
            for (++p; p < data.Length; p += (int)InternalBit.ExtractLowestSetBit(p))
                data[p] += w;
        }

        /// <summary>
        /// a[<paramref name="l"/>..<paramref name="r"/>) += <paramref name="x"/> を行います。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="l"/>&lt;n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [凾(256)]
        public void Add(int l, int r, T x)
        {
            Add(data1, l, -x * T.CreateChecked(l));
            Add(data1, r, x * T.CreateChecked(r));
            Add(data2, l, x);
            Add(data2, r, -x);
        }
        [凾(256)]
        static T Sum(T[] data, int r)
        {
            T res = default;
            for (; r > 0; r &= r - 1)
                res += data[r];
            return res;
        }
        /// <summary>
        /// a[<paramref name="l"/>] + a[<paramref name="l"/> - 1] + ... + a[<paramref name="r"/> - 1] を返します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="l"/>≤<paramref name="r"/>≤n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        /// <returns>a[<paramref name="l"/>] + a[<paramref name="l"/> - 1] + ... + a[<paramref name="r"/> - 1]</returns>
        [凾(256)]
        public T Sum(int l, int r)
        {
            Contract.Assert(0U <= (uint)l && (uint)l <= (uint)r && (uint)r <= (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(l)} && {nameof(l)} <= {nameof(r)} && {nameof(r)} <= Length");
            return Sum(r) - Sum(l);
        }

        [凾(256)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public T Sum(int r) => Sum(data1, r) + (Sum(data2, r) * T.CreateChecked(r));


        [凾(256)]
        public T Slice(int l, int len) => Sum(l, l + len);

        [DebuggerDisplay("Value = {" + nameof(value) + "}, Sum = {" + nameof(sum) + "}")]
        internal readonly struct DebugItem
        {
            public DebugItem(T value, T sum)
            {
                this.sum = sum;
                this.value = value;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public readonly T value;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public readonly T sum;
        }
        internal class DebugView
        {
            readonly FenwickTreeRange<T> fenwickTree;
            public DebugView(FenwickTreeRange<T> fenwickTree)
            {
                this.fenwickTree = fenwickTree;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebugItem[] Items
            {
                get
                {
                    var items = new DebugItem[fenwickTree.Length];
                    T prev = default;
                    for (int i = 0; i < items.Length; i++)
                    {
                        var sum = fenwickTree.Sum(i + 1);
                        items[i] = new DebugItem(sum - prev, sum);
                        prev = sum;
                    }
                    return items;
                }
            }
        }
    }
}