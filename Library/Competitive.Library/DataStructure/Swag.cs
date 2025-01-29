/*
 * Original
 * https://github.com/yosupo06/library-checker-problems/blob/b2d2c050026820706dea6c9f18b8275a0fb0cada/datastructure/queue_operate_all_composite/sol/correct.cpp
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
    public class Swag<T, TOp> where TOp : struct, ISegtreeOperator<T>
    {
        /*
         * frontSize = 4 のときの構造
         * ---------frontSize---------
         * | d[0] | d[1] | d[2] | d[3] |---d[4..]---|
         *                                 back1
         * ------d[0]の保持する総積------
         *         --d[1]の保持する総積--
         * 
         *  i < frontSize: d[i] は d[i..frontSize] の総積を保持する
         * frontSize <= i: d[i] は元の値を保持する。
         * 
         * back は d[frontSize..] の総積
         */
        static TOp op => default;
        int frontSize = 0;
        T back = op.Identity;
        readonly Deque<T> d;
        /// <summary>
        /// Slide Window Aggrigation: モノイドの範囲演算を移動しながら求める。計算量: O(N)
        /// </summary>
        public Swag()
        {
            d = new Deque<T>();
        }
        /// <summary>
        /// Slide Window Aggrigation: モノイドの範囲演算を移動しながら求める。計算量: O(N)
        /// </summary>
        /// <param name="capacity">内部で保持する <see cref="Deque{T}"/> の初期サイズ</param>
        public Swag(int capacity)
        {
            d = new Deque<T>(capacity);
        }

        /// <summary>
        /// Slide Window Aggrigation: モノイドの範囲演算を移動しながら求める。計算量: O(N)
        /// </summary>
        /// <param name="initial">あらかじめ追加されているモノイド</param>
        public Swag(ReadOnlySpan<T> initial)
        {
            d = new Deque<T>(initial.Length) { tail = initial.Length };
            initial.CopyTo(d.data);
            foreach (var x in initial)
                back = op.Operate(back, x);
        }

        /// <summary>
        /// 保持しているモノイドの数を返します。
        /// </summary>
        public int Count => d.Count;

        /// <summary>
        /// 末尾にモノイドを追加します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        public void Push(T x)
        {
            d.AddLast(x);
            back = op.Operate(back, x);
        }

        /// <summary>
        /// 先頭のモノイドを削除します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        public void Pop()
        {
            Contract.Assert(d.Count > 0, "data is empty.");
            d.PopFirst();
            if (--frontSize < 0)
            {
                for (int i = d.Count - 2; i >= 0; i--)
                {
                    d[i] = op.Operate(d[i], d[i + 1]);
                }
                frontSize = d.Count;
                back = op.Identity;
            }
        }
        /// <summary>
        /// モノイドをマージした結果を返します。
        /// </summary>
        /// <remarks>
        /// <para>op(d[0], op(d[1], op(...)))</para>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        public T AllProd => frontSize > 0 ? op.Operate(d.First, back) : back;

        /// <summary>
        /// 格納されたモノイドをすべて削除します
        /// </summary>
        [凾(256)]
        public void Clear()
        {
            frontSize = 0;
            back = op.Identity;
            d.Clear();
        }
    }
}
