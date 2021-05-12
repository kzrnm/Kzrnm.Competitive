using AtCoder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kzrnm.Competitive
{
    [IsOperator]
    public interface SlideGetOperator<T>
    {
        /// <summary>
        /// 最も xx な条件をみたす値が <paramref name="newValue"/> であるかを返す。<paramref name="oldValue"/> と等しいならば <see langword="true"/> を返すべし。
        /// </summary>
        public bool NeedUpdate(T oldValue, T newValue);
    }
    /// <summary>
    /// スライドする区間の X を求める。(スライド最小値など)
    /// </summary>
    public static class SlideGet
    {
        /// <summary>
        /// <para><paramref name="array"/>(i-<paramref name="k"/>...i] で最も <typeparamref name="TOp"/> な値を返す。</para>
        /// <para>計算量: O(<paramref name="array"/>.Length)</para>
        /// </summary>
        public static T[] Front<T, TOp>(T[] array, int k, TOp op = default)
            where TOp : struct, SlideGetOperator<T>
        {
            AtCoder.Internal.Contract.Assert(k > 0, "k must be positive");
            AtCoder.Internal.Contract.Assert(k <= array.Length, "k must not be greater than array.Length");
            var result = new T[array.Length];
            Deque<int> deq = new Deque<int>();
            for (int i = 0; i < array.Length; i++)
            {
                while (deq.Count > 0 && op.NeedUpdate(array[deq.Last], array[i]))
                    deq.PopLast();
                deq.AddLast(i);
                var first = deq.First;
                result[i] = array[first];
                if (first == i - k + 1) deq.PopFirst();
            }
            return result;
        }
        /// <summary>
        /// <para><paramref name="array"/>[i...i+<paramref name="k"/>) で最も <typeparamref name="TOp"/> な値を返す。</para>
        /// <para>計算量: O(<paramref name="array"/>.Length)</para>
        /// </summary>
        public static T[] Back<T, TOp>(T[] array, int k, TOp op = default)
            where TOp : struct, SlideGetOperator<T>
        {
            AtCoder.Internal.Contract.Assert(k > 0, "k must be positive");
            AtCoder.Internal.Contract.Assert(k <= array.Length, "k must not be greater than array.Length");
            var result = new T[array.Length];
            Deque<int> deq = new Deque<int>();
            for (int i = 0; i < array.Length; i++)
            {
                while (deq.Count > 0 && op.NeedUpdate(array[deq.Last], array[i]))
                    deq.PopLast();
                deq.AddLast(i);
                if (i - k + 1 >= 0)
                {
                    var first = deq.First;
                    result[i - k + 1] = array[first];
                    if (first == i - k + 1) deq.PopFirst();
                }
            }
            for (int i = array.Length; i - k + 1 < result.Length; i++)
            {
                var first = deq.First;
                result[i - k + 1] = array[first];
                if (first == i - k + 1) deq.PopFirst();
            }
            return result;
        }
    }
}
