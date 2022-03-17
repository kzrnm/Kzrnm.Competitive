using AtCoder;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __BinarySearchGenericEx
    {
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static T BinarySearch<T, TOp>(T ok, T ng) where TOp : struct, IBinaryOk<T>
               => BinarySearch(default(TOp), ok, ng);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static T BinarySearch<T, TOp>(TOp op, T ok, T ng) where TOp : struct, IBinaryOk<T>
        {
            while (op.Continue(ok, ng))
            {
                var m = op.Mid(ok, ng);
                if (op.Ok(m)) ok = m;
                else ng = m;
            }
            return ok;
        }
        private readonly struct FuncBinaryOk<T> : IBinaryOk<T>
        {
            readonly Func<T, bool> ok;
            readonly Func<T, T, T> mid;
            readonly Func<T, T, bool> continueFunc;
            public FuncBinaryOk(Func<T, bool> ok, Func<T, T, T> mid, Func<T, T, bool> continueFunc)
            {
                this.ok = ok;
                this.mid = mid;
                this.continueFunc = continueFunc;
            }
            [凾(256)]
            public bool Ok(T value) => ok(value);
            [凾(256)]
            public T Mid(T ok, T ng) => mid(ok, ng);
            [凾(256)]
            public bool Continue(T ok, T ng) => continueFunc(ok, ng);
        }
    }
    [IsOperator]
    public interface IBinaryOk<T> : IOk<T>
    {
        T Mid(T ok, T ng);
        bool Continue(T ok, T ng);
    }
}
