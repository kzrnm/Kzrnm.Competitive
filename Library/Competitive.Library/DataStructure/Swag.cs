/*
 * Original
 * https://github.com/kmyk/library-checker-problems/blob/47ba6600eb6dc445ce742de511ca69cb6fc749b1/datastructure/queue_operate_all_composite/sol/correct.cpp
 * 
 * Apache License Version 2.0
 * https://github.com/yosupo06/library-checker-problems
 */
using AtCoder;
using AtCoder.Internal;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE スライド演算(SWAG: Slide Window Aggrigation)
    /// <summary>
    /// Slide Window Aggrigation: モノイドの範囲演算を移動しながら求める。計算量: O(N)
    /// </summary>
    public class Swag<T, TOp> where TOp : struct, ISegtreeOperator<T>
    {
        private static TOp op = default;
        private int frontSize = 0;
        private T back = op.Identity;
        private readonly Deque<T> data;
        /// <summary>
        /// Slide Window Aggrigation: モノイドの範囲演算を移動しながら求める。計算量: O(N)
        /// </summary>
        public Swag()
        {
            data = new Deque<T>();
        }
        /// <summary>
        /// Slide Window Aggrigation: モノイドの範囲演算を移動しながら求める。計算量: O(N)
        /// </summary>
        /// <param name="capacity">内部で保持する <see cref="Deque{T}"/> の初期サイズ</param>
        public Swag(int capacity)
        {
            data = new Deque<T>(capacity);
        }
        /// <summary>
        /// 末尾にモノイドを追加します。
        /// </summary>
        public void Push(T x)
        {
            data.AddLast(x);
            back = op.Operate(back, x);
        }

        /// <summary>
        /// 先頭のモノイドを削除します。
        /// </summary>
        public void Pop()
        {
            Contract.Assert(data.Count > 0, "data is empty.");
            data.PopFirst();
            if (--frontSize < 0)
            {
                for (int i = data.Count - 2; i >= 0; i--)
                {
                    data[i] = op.Operate(data[i], data[i + 1]);
                }
                frontSize = data.Count;
                back = op.Identity;
            }
        }
        /// <summary>
        /// モノイドをマージした結果を返します。
        /// </summary>
        public T AllProd { [凾(256)] get => frontSize > 0 ? op.Operate(data.First, back) : back; }

        /// <summary>
        /// 格納されたモノイドをすべて削除します
        /// </summary>
        [凾(256)]
        public void Clear()
        {
            frontSize = 0;
            back = op.Identity;
            data.Clear();
        }
    }
}
