using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AtCoder.Internal;

namespace Kzrnm.Competitive
{
    using static MethodImplOptions;

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
    [DebuggerTypeProxy(typeof(FenwickTreeRange.DebugView))]
    public class FenwickTreeRange
    {

        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly long[] data1;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly long[] data2;

        public int Length { get; }

        /// <summary>
        /// 長さ <paramref name="n"/> の配列aを持つ <see cref="FenwickTreeRange"/> クラスの新しいインスタンスを作ります。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        /// <param name="n">配列の長さ</param>
        public FenwickTreeRange(int n)
        {
            Length = n;
            data1 = new long[n + 1];
            data2 = new long[n + 1];
        }


        [MethodImpl(AggressiveInlining)]
        private static void Add(long[] data, int p, long w)
        {
            for (++p; p < data.Length; p += InternalBit.ExtractLowestSetBit(p))
                data[p] += w;
        }

        /// <summary>
        /// a[<paramref name="l"/>..<paramref name="r"/>) += <paramref name="x"/> を行います。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="l"/>&lt;n</para>
        /// <para>計算量: O(log n)</para>
        /// </remarks>
        [MethodImpl(AggressiveInlining)]
        public void Add(int l, int r, long x)
        {
            Add(data1, l, -x * l);
            Add(data1, r, x * r);
            Add(data2, l, x);
            Add(data2, r, -x);
        }

        private static long Sum(long[] data, int r)
        {
            long res = 0;
            for (; r > 0; r -= InternalBit.ExtractLowestSetBit(r))
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
        [MethodImpl(AggressiveInlining)]
        public long Sum(int l, int r)
        {
            Contract.Assert(0U <= (uint)l && (uint)l <= (uint)r && (uint)r <= (uint)Length, reason: $"IndexOutOfRange: 0 <= {nameof(l)} && {nameof(l)} <= {nameof(r)} && {nameof(r)} <= Length");
            return Sum(r) - Sum(l);
        }

        [MethodImpl(AggressiveInlining)]

        [EditorBrowsable(EditorBrowsableState.Never)]
        public long Sum(int r) => Sum(data1, r) + Sum(data2, r) * r;


        [MethodImpl(AggressiveInlining)]
        public long Slice(int l, int len) => Sum(l, l + len);

        [DebuggerDisplay("Value = {" + nameof(value) + "}, Sum = {" + nameof(sum) + "}")]
        internal struct DebugItem
        {
            public DebugItem(long value, long sum)
            {
                this.sum = sum;
                this.value = value;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public readonly long value;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public readonly long sum;
        }
        internal class DebugView
        {
            private readonly FenwickTreeRange fenwickTree;
            public DebugView(FenwickTreeRange fenwickTree)
            {
                this.fenwickTree = fenwickTree;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebugItem[] Items
            {
                get
                {
                    var items = new DebugItem[fenwickTree.Length];
                    long prev = 0;
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