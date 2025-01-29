/*
 * Original
 * https://github.com/yosupo06/library-checker-problems/blob/b2d2c050026820706dea6c9f18b8275a0fb0cada/datastructure/deque_operate_all_composite/sol/correct.cpp
 * 
 * Apache License Version 2.0
 * https://github.com/yosupo06/library-checker-problems
 */
using AtCoder;
using AtCoder.Internal;
using System;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// Slide Window Aggrigation: モノイドの範囲演算を移動しながら求める。計算量: O(N)
    /// </summary>
    [DebuggerDisplay("AllProd = {" + nameof(AllProd) + "}")]
    public class SwagDeque<T, TOp> where TOp : struct, ISegtreeOperator<T>
    {
        /*
         * c.Count = 4, d.Count = 3 のときの構造
         * | data[0] | data[1] | data[2] | data[3] |--| data[4] | data[5] | data[6] |
         * |    c[0] |    c[1] |    c[2] |    c[3] |--|   d[^3] |   d[^2] |   d[^1] |
         *                               
         * ------------c[0]の保持する総積------------    ------d[^1]の保持する総積------
         *             ------c[1]の保持する総積------    -d[^2]の保持する総積-
         * 
         * c[i] は data[i..c.Count] の総積を保持する
         * d[i] は data[^d.Count..i+1] の総積を保持する
         * 
         * c, d のどちらかが空になったら半分ずつに分ける
         * 
         */

        static TOp op => default;
        readonly Deque<T> a, c, d;
        /// <summary>
        /// Slide Window Aggrigation: モノイドの範囲演算を移動しながら求める。計算量: O(N)
        /// </summary>
        public SwagDeque()
        {
            a = new Deque<T>();
            c = new Deque<T>();
            d = new Deque<T>();
        }

        /// <summary>
        /// Slide Window Aggrigation: モノイドの範囲演算を移動しながら求める。計算量: O(N)
        /// </summary>
        /// <param name="capacity">内部で保持する <see cref="Deque{T}"/> の初期サイズ</param>
        public SwagDeque(int capacity)
        {
            a = new Deque<T>(capacity);
            c = new Deque<T>(capacity);
            d = new Deque<T>(capacity);
        }

        /// <summary>
        /// Slide Window Aggrigation: モノイドの範囲演算を移動しながら求める。計算量: O(N)
        /// </summary>
        /// <param name="initial">あらかじめ追加されているモノイド</param>
        public SwagDeque(ReadOnlySpan<T> initial)
        {
            a = new Deque<T>(initial.Length) { tail = initial.Length };
            c = new Deque<T>(initial.Length);
            d = new Deque<T>(initial.Length);
            initial.CopyTo(a.data);
            Balance();
        }

        /// <summary>
        /// 保持しているモノイドの数を返します。
        /// </summary>
        public int Count => a.Count;

        /// <summary>
        /// 先頭にモノイドを追加します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        public void AddFirst(T x)
        {
            Debug.Assert(c.Count + d.Count == a.Count);
            a.AddFirst(x);
            c.AddFirst(c.Count == 0 ? x : op.Operate(x, c.First));
        }

        /// <summary>
        /// 末尾にモノイドを追加します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        public void AddLast(T x)
        {
            Debug.Assert(c.Count + d.Count == a.Count);
            a.AddLast(x);
            d.AddLast(d.Count == 0 ? x : op.Operate(d.Last, x));
        }

        /// <summary>
        /// 先頭のモノイドを削除します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: 償却 O(1)</para>
        /// </remarks>
        public void PopFirst()
        {
            Debug.Assert(c.Count + d.Count == a.Count);
            Contract.Assert(Count > 0, "data is empty.");
            if (c.Count == 0)
            {
                if (d.Count == 1)
                {
                    a.Clear();
                    d.Clear();
                    return;
                }
                Balance();
            }
            a.PopFirst();
            c.PopFirst();
        }

        /// <summary>
        /// 末尾のモノイドを削除します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: 償却 O(1)</para>
        /// </remarks>
        public void PopLast()
        {
            Debug.Assert(c.Count + d.Count == a.Count);
            Contract.Assert(Count > 0, "data is empty.");
            if (d.Count == 0) Balance();

            a.PopLast();
            d.PopLast();
        }
        /// <summary>
        /// モノイドをマージした結果を返します。
        /// </summary>
        /// <remarks>
        /// <para>op(d[0], op(d[1], op(...)))</para>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        public T AllProd => (c.Count, d.Count) switch
        {
            (0, 0) => op.Identity,
            (_, 0) => c.First,
            (0, _) => d.Last,
            _ => op.Operate(c.First, d.Last),
        };

        /// <summary>
        /// 格納されたモノイドをすべて削除します
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        public void Clear()
        {
            a.Clear();
            c.Clear();
            d.Clear();
        }

        /// <summary>
        /// c, d のどちらかが空になったときに半分ずつにします。奇数個のときには右が1個多くなります。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        void Balance()
        {
            Debug.Assert(c.Count == 0 || d.Count == 0);
            c.Clear();
            d.Clear();
            var size = a.Count;

            if (size <= 1)
            {
                if (size == 1)
                    d.AddLast(a[0]);
                return;
            }
            var half = size >> 1;
            c.AddLast(a[half - 1]);
            d.AddFirst(a[half]);

            for (int i = half - 2; i >= 0; i--)
                c.AddFirst(op.Operate(a[i], c.First));
            for (int i = half + 1; i < size; i++)
                d.AddLast(op.Operate(d.Last, a[i]));
            Debug.Assert(c.Count + d.Count == a.Count);
        }
    }
}
