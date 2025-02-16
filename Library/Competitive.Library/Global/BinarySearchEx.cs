using AtCoder;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __BinarySearchEx
    {
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <c>Ok</c>(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <c>Ok</c>(<paramref name="ok"/>) &amp;&amp; !<c>Ok</c>(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static int BinarySearch(int ok, int ng, Func<int, bool> okFunc)
                 => BinarySearch(new FuncOk<int>(okFunc), ok, ng);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <c>Ok</c>(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <c>Ok</c>(<paramref name="ok"/>) &amp;&amp; !<c>Ok</c>(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static int BinarySearch<TOp>(int ok, int ng) where TOp : struct, IOk<int>
               => BinarySearch(new TOp(), ok, ng);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static int BinarySearch<TOp>(this TOp op, int ok, int ng) where TOp : struct, IOk<int>
        {
            while (Math.Abs(ok - ng) > 1)
            {
                var m = ((ok - ng) >> 1) + ng;
                if (op.Ok(m)) ok = m;
                else ng = m;
            }
            return ok;
        }

        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <c>Ok</c>(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <c>Ok</c>(<paramref name="ok"/>) &amp;&amp; !<c>Ok</c>(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static long BinarySearch(long ok, long ng, Func<long, bool> okFunc)
            => BinarySearch(new FuncOk<long>(okFunc), ok, ng);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <c>Ok</c>(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <c>Ok</c>(<paramref name="ok"/>) &amp;&amp; !<c>Ok</c>(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static long BinarySearch<TOp>(long ok, long ng) where TOp : struct, IOk<long>
            => BinarySearch(new TOp(), ok, ng);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static long BinarySearch<TOp>(this TOp op, long ok, long ng) where TOp : struct, IOk<long>
        {
            while (Math.Abs(ok - ng) > 1)
            {
                var m = ((ok - ng) >> 1) + ng;
                if (op.Ok(m)) ok = m;
                else ng = m;
            }
            return ok;
        }

        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <c>Ok</c>(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <c>Ok</c>(<paramref name="ok"/>) &amp;&amp; !<c>Ok</c>(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static ulong BinarySearch(ulong ok, ulong ng, Func<ulong, bool> okFunc)
            => BinarySearch(new FuncOk<ulong>(okFunc), ok, ng);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <c>Ok</c>(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <c>Ok</c>(<paramref name="ok"/>) &amp;&amp; !<c>Ok</c>(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static ulong BinarySearch<TOp>(ulong ok, ulong ng) where TOp : struct, IOk<ulong>
            => BinarySearch(new TOp(), ok, ng);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static ulong BinarySearch<TOp>(this TOp op, ulong ok, ulong ng) where TOp : struct, IOk<ulong>
        {
            while ((ok > ng ? ok - ng : ng - ok) > 1)
            {
                var m = ok > ng ? ((ok - ng) >> 1) + ng : ((ng - ok) >> 1) + ok;
                if (op.Ok(m)) ok = m;
                else ng = m;
            }
            return ok;
        }

        /// <summary>
        /// <paramref name="ok"/> 以上で <c>Ok</c>(i) == true を満たす最も大きい値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <c>Ok</c>(<paramref name="ok"/>)</para>
        /// <para>計算量: O(log |result|)</para>
        /// </remarks>
        [凾(256)]
        public static long BinarySearchBig(long ok, Func<long, bool> okFunc)
            => BinarySearchBig(new FuncOk<long>(okFunc), ok);
        /// <summary>
        /// <paramref name="ok"/> 以上で <c>Ok</c>(i) == true を満たす最も大きい値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <c>Ok</c>(<paramref name="ok"/>)</para>
        /// <para>計算量: O(log |result|)</para>
        /// </remarks>
        [凾(256)]
        public static long BinarySearchBig<TOp>(long ok) where TOp : struct, IOk<long>
            => BinarySearchBig(new TOp(), ok);
        /// <summary>
        /// <paramref name="ok"/> 以上で <c>Ok</c>(i) == true を満たす最も大きい値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <c>Ok</c>(<paramref name="ok"/>)</para>
        /// <para>計算量: O(log |result|)</para>
        /// </remarks>
        [凾(256)]
        public static long BinarySearchBig<TOp>(this TOp op, long ok) where TOp : struct, IOk<long>
        {
            long plus = 1;
            var ng = ok + plus;
            while (op.Ok(ng))
            {
                plus <<= 1;
                ng += plus;
            }
            return BinarySearch(op, ok, ng);
        }

        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <c>Ok</c>(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <c>Ok</c>(<paramref name="ok"/>) &amp;&amp; !<c>Ok</c>(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static double BinarySearch(double ok, double ng, Func<double, bool> okFunc, double eps = 1e-7)
            => BinarySearch(new FuncOk<double>(okFunc), ok, ng, eps);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <c>Ok</c>(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <c>Ok</c>(<paramref name="ok"/>) &amp;&amp; !<c>Ok</c>(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static double BinarySearch<TOp>(double ok, double ng, double eps = 1e-7) where TOp : struct, IOk<double>
            => BinarySearch(new TOp(), ok, ng, eps);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static double BinarySearch<TOp>(this TOp op, double ok, double ng, double eps = 1e-7) where TOp : struct, IOk<double>
        {
            while (Math.Abs(ok - ng) > eps)
            {
                var m = (ok + ng) / 2;
                if (op.Ok(m)) ok = m;
                else ng = m;
            }
            return ok;
        }

        //#pragma warning disable CA1031
        //        private static bool NgOrThrow<T>(IOk<T> op, T val) { try { return !op.Ok(val); } catch { return true; } }
        //#pragma warning restore CA1031

        private readonly record struct FuncOk<T>(Func<T, bool> ok) : IOk<T>
        {
            [凾(256)]
            public bool Ok(T value) => ok(value);
        }

        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        [凾(256)]
        public static T BinarySearch<T, TOp>(T ok, T ng) where TOp : struct, IBinaryOk<T>
               => BinarySearch(new TOp(), ok, ng);
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
        private readonly record struct FuncBinaryOk<T>(Func<T, bool> ok, Func<T, T, T> mid, Func<T, T, bool> continueFunc) : IBinaryOk<T>
        {
            [凾(256)]
            public bool Ok(T value) => ok(value);
            [凾(256)]
            public T Mid(T ok, T ng) => mid(ok, ng);
            [凾(256)]
            public bool Continue(T ok, T ng) => continueFunc(ok, ng);
        }
    }
    [IsOperator]
    public interface IOk<in T>
    {
        bool Ok(T value);
    }

    [IsOperator]
    public interface IBinaryOk<T> : IOk<T>
    {
        T Mid(T ok, T ng);
        bool Continue(T ok, T ng);
    }
}
