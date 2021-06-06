using AtCoder;
using System;
using System.Numerics;

namespace Kzrnm.Competitive
{
    public static class __BinarySearchEx
    {
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static int BinarySearch(int ok, int ng, Func<int, bool> okFunc)
            => BinarySearch(new FuncOk<int>(okFunc), ok, ng);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static int BinarySearch<TOp>(int ok, int ng) where TOp : struct, IOk<int>
            => BinarySearch(default(TOp), ok, ng);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
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
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static long BinarySearch(long ok, long ng, Func<long, bool> okFunc)
            => BinarySearch(new FuncOk<long>(okFunc), ok, ng);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static long BinarySearch<TOp>(long ok, long ng) where TOp : struct, IOk<long>
            => BinarySearch(default(TOp), ok, ng);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
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
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static ulong BinarySearch(ulong ok, ulong ng, Func<ulong, bool> okFunc)
            => BinarySearch(new FuncOk<ulong>(okFunc), ok, ng);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static ulong BinarySearch<TOp>(ulong ok, ulong ng) where TOp : struct, IOk<ulong>
            => BinarySearch(default(TOp), ok, ng);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
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
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static BigInteger BinarySearch(BigInteger ok, BigInteger ng, Func<BigInteger, bool> okFunc)
            => BinarySearch(new FuncOk<BigInteger>(okFunc), ok, ng);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static BigInteger BinarySearch<TOp>(BigInteger ok, BigInteger ng) where TOp : struct, IOk<BigInteger>
            => BinarySearch(default(TOp), ok, ng);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static BigInteger BinarySearch<TOp>(this TOp op, BigInteger ok, BigInteger ng) where TOp : struct, IOk<BigInteger>
        {
            while (BigInteger.Abs(ok - ng) > 1)
            {
                var m = ((ok - ng) >> 1) + ng;
                if (op.Ok(m)) ok = m;
                else ng = m;
            }
            return ok;
        }

        /// <summary>
        /// <paramref name="ok"/> 以上で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も大きい値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |result|)</para>
        /// </remarks>
        public static long BinarySearchBig(long ok, Func<long, bool> okFunc)
            => BinarySearchBig(new FuncOk<long>(okFunc), ok);
        /// <summary>
        /// <paramref name="ok"/> 以上で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も大きい値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |result|)</para>
        /// </remarks>
        public static long BinarySearchBig<TOp>(long ok) where TOp : struct, IOk<long>
            => BinarySearchBig(default(TOp), ok);
        /// <summary>
        /// <paramref name="ok"/> 以上で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も大きい値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |result|)</para>
        /// </remarks>
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
        /// <paramref name="ok"/> 以上で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も大きい値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |result|)</para>
        /// </remarks>
        public static BigInteger BinarySearchBig(BigInteger ok, Func<BigInteger, bool> okFunc)
            => BinarySearchBig(ok, new FuncOk<BigInteger>(okFunc));
        /// <summary>
        /// <paramref name="ok"/> 以上で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も大きい値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |result|)</para>
        /// </remarks>
        public static BigInteger BinarySearchBig<TOp>(BigInteger ok) where TOp : struct, IOk<BigInteger>
            => BinarySearchBig(ok, default(TOp));
        /// <summary>
        /// <paramref name="ok"/> 以上で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も大きい値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |result|)</para>
        /// </remarks>
        public static BigInteger BinarySearchBig<TOp>(BigInteger ok, TOp op) where TOp : struct, IOk<BigInteger>
        {
            BigInteger plus = 1;
            var ng = ok + plus;
            while (op.Ok(ng))
            {
                plus <<= 1;
                ng += plus;
            }
            return BinarySearch(op, ok, ng);
        }

        /// <summary>
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static double BinarySearch(double ok, double ng, Func<double, bool> okFunc, double eps = 1e-7)
            => BinarySearch(new FuncOk<double>(okFunc), ok, ng, eps);
        /// <summary>
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static double BinarySearch<TOp>(double ok, double ng, double eps = 1e-7) where TOp : struct, IOk<double>
            => BinarySearch(default(TOp), ok, ng, eps);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
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

        private readonly struct FuncOk<T> : IOk<T>
        {
            readonly Func<T, bool> ok;
            public FuncOk(Func<T, bool> ok)
            {
                this.ok = ok;
            }
            public bool Ok(T value) => ok(value);
        }
    }
    [IsOperator]
    public interface IOk<in T>
    {
        bool Ok(T value);
    }
}
