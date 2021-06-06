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
            => BinarySearch(ok, ng, new FuncOk<int>(okFunc));
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static int BinarySearch<TOp>(int ok, int ng) where TOp : IOk<int>
            => BinarySearch(ok, ng, default(TOp));
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static int BinarySearch<TOp>(int ok, int ng, TOp op) where TOp : IOk<int>
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
            => BinarySearch(ok, ng, new FuncOk<long>(okFunc));
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static long BinarySearch<TOp>(long ok, long ng) where TOp : IOk<long>
            => BinarySearch(ok, ng, default(TOp));
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static long BinarySearch<TOp>(long ok, long ng, TOp op) where TOp : IOk<long>
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
            => BinarySearch(ok, ng, new FuncOk<ulong>(okFunc));
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static ulong BinarySearch<TOp>(ulong ok, ulong ng) where TOp : IOk<ulong>
            => BinarySearch(ok, ng, default(TOp));
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static ulong BinarySearch<TOp>(ulong ok, ulong ng, TOp op) where TOp : IOk<ulong>
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
            => BinarySearch(ok, ng, new FuncOk<BigInteger>(okFunc));
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static BigInteger BinarySearch<TOp>(BigInteger ok, BigInteger ng) where TOp : IOk<BigInteger>
            => BinarySearch(ok, ng, default(TOp));
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static BigInteger BinarySearch<TOp>(BigInteger ok, BigInteger ng, TOp op) where TOp : IOk<BigInteger>
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
            => BinarySearchBig(ok, new FuncOk<long>(okFunc));
        /// <summary>
        /// <paramref name="ok"/> 以上で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も大きい値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |result|)</para>
        /// </remarks>
        public static long BinarySearchBig<TOp>(long ok) where TOp : IOk<long>
            => BinarySearchBig(ok, default(TOp));
        /// <summary>
        /// <paramref name="ok"/> 以上で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も大きい値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |result|)</para>
        /// </remarks>
        public static long BinarySearchBig<TOp>(long ok, TOp op) where TOp : IOk<long>
        {
            long plus = 1;
            var ng = ok + plus;
            while (op.Ok(ng))
            {
                plus <<= 1;
                ng += plus;
            }
            return BinarySearch(ok, ng, op);
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
        public static BigInteger BinarySearchBig<TOp>(BigInteger ok) where TOp : IOk<BigInteger>
            => BinarySearchBig(ok, default(TOp));
        /// <summary>
        /// <paramref name="ok"/> 以上で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も大きい値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |result|)</para>
        /// </remarks>
        public static BigInteger BinarySearchBig<TOp>(BigInteger ok, TOp op) where TOp : IOk<BigInteger>
        {
            BigInteger plus = 1;
            var ng = ok + plus;
            while (op.Ok(ng))
            {
                plus <<= 1;
                ng += plus;
            }
            return BinarySearch(ok, ng, op);
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
            => BinarySearch(ok, ng, new FuncOk<double>(okFunc), eps);
        /// <summary>
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <typeparamref name="TOp"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <typeparamref name="TOp"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<typeparamref name="TOp"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static double BinarySearch<TOp>(double ok, double ng, double eps = 1e-7) where TOp : IOk<double>
            => BinarySearch(ok, ng, default(TOp), eps);
        /// <summary>
        /// <paramref name="ok"/> と <paramref name="ng"/> の間で <paramref name="op"/>.Ok(i) == true を満たす最も <paramref name="ng"/> に近い値を取得します。
        /// </summary>
        /// <remarks>
        /// <para>制約: <paramref name="op"/>.Ok(<paramref name="ok"/>) &amp;&amp; !<paramref name="op"/>.Ok(<paramref name="ng"/>)</para>
        /// <para>計算量: O(log |<paramref name="ok"/> - <paramref name="ng"/>|)</para>
        /// </remarks>
        public static double BinarySearch<TOp>(double ok, double ng, TOp op, double eps = 1e-7) where TOp : IOk<double>
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
