using AtCoder;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

// https://nyaannyaan.github.io/library/fps/formal-power-series.hpp
namespace Kzrnm.Competitive
{
    /// <summary>
    /// 多項式/形式的冪級数
    /// </summary>
    public partial class FormalPowerSeries<T>
        where T : struct, IStaticMod
    {
        /// <summary>
        /// 多項式の係数
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public readonly StaticModInt<T>[] Coefficients;

        /// <summary>
        /// 多項式の次数
        /// </summary>
        public int Count => Coefficients.Length;

        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="polynomial"/>[i]&lt;mod</para>
        /// </remarks>
        /// <param name="polynomial"><paramref name="polynomial"/>[i] がi次の係数となる多項式</param>
        public FormalPowerSeries(ReadOnlySpan<uint> polynomial)
            : this(MemoryMarshal.Cast<uint, StaticModInt<T>>(polynomial)) { }

        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="polynomial"/>[i]&lt;mod</para>
        /// </remarks>
        /// <param name="polynomial"><paramref name="polynomial"/>[i] がi次の係数となる多項式</param>
        public FormalPowerSeries(ReadOnlySpan<int> polynomial)
            : this(MemoryMarshal.Cast<int, StaticModInt<T>>(polynomial)) { }

        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        /// <param name="polynomial"><paramref name="polynomial"/>[i] がi次の係数となる多項式</param>
        public FormalPowerSeries(ReadOnlySpan<StaticModInt<T>> polynomial)
            : this(Shrink(polynomial).ToArray(), true) { }
        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        public FormalPowerSeries(StaticModInt<T>[] polynomial, bool newArray)
        {
            if (newArray)
                Coefficients = (StaticModInt<T>[])polynomial.Clone();
            else
                Coefficients = polynomial;
        }
        public override string ToString() => string.Join(", ", Coefficients);
        private static ReadOnlySpan<StaticModInt<T>> Shrink(ReadOnlySpan<StaticModInt<T>> polynomial)
        {
            while (polynomial.Length > 0 && polynomial[^1].Value == 0)
                polynomial = polynomial[..^1];
            return polynomial;

        }

        #region Add
        /// <summary>
        /// <paramref name="lhs"/> + <paramref name="rhs"/>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public static FormalPowerSeries<T> operator +(FormalPowerSeries<T> lhs, FormalPowerSeries<T> rhs)
        {
            if (rhs.Coefficients.Length == 0) return lhs;
            if (lhs.Coefficients.Length == 0) return rhs;
            return new Impl(lhs).Add(rhs.Coefficients).ToFps();
        }

        /// <summary>
        /// <paramref name="lhs"/> + <paramref name="rhs"/>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public static FormalPowerSeries<T> operator +(FormalPowerSeries<T> lhs, ReadOnlySpan<StaticModInt<T>> rhs)
        {
            if (rhs.Length == 0) return lhs;
            if (lhs.Coefficients.Length == 0) return new FormalPowerSeries<T>(rhs);
            return new Impl(lhs).Add(rhs).ToFps();
        }
        #endregion Add

        #region Subtract
        /// <summary>
        /// <paramref name="lhs"/> - <paramref name="rhs"/>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public static FormalPowerSeries<T> operator -(FormalPowerSeries<T> lhs, FormalPowerSeries<T> rhs)
        {
            if (rhs.Coefficients.Length == 0) return lhs;
            if (lhs.Coefficients.Length == 0) return -rhs;
            return new Impl(lhs).Subtract(rhs.Coefficients).ToFps();
        }

        /// <summary>
        /// <paramref name="lhs"/> - <paramref name="rhs"/>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public static FormalPowerSeries<T> operator -(FormalPowerSeries<T> lhs, ReadOnlySpan<StaticModInt<T>> rhs)
        {
            if (rhs.Length == 0) return lhs;
            if (lhs.Coefficients.Length == 0) return new Impl(rhs.ToArray()).Minus().ToFps();
            return new Impl(lhs).Subtract(rhs).ToFps();
        }

        /// <summary>
        /// -<paramref name="v"/>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        public static FormalPowerSeries<T> operator -(FormalPowerSeries<T> v)
            => new Impl(v).Minus().ToFps();
        #endregion Subtract

        #region Multiply
        /// <summary>
        /// <paramref name="lhs"/> * <paramref name="rhs"/>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O( L log L ) ただし L = len(<paramref name="lhs"/>) + len(<paramref name="rhs"/>) </para>
        /// </remarks>
        [凾(256)]
        public static FormalPowerSeries<T> operator *(FormalPowerSeries<T> lhs, FormalPowerSeries<T> rhs)
            => new Impl(lhs).Multiply(rhs.Coefficients).ToFps();

        [凾(256)]
        public static FormalPowerSeries<T> operator *(StaticModInt<T> lhs, FormalPowerSeries<T> rhs)
            => new Impl(rhs).Multiply(lhs).ToFps();
        #endregion Multiply

        #region Divide
        /// <summary>
        /// <paramref name="lhs"/> / <paramref name="rhs"/>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O( N log N )</para>
        /// </remarks>
        [凾(256)]
        public static FormalPowerSeries<T> operator /(FormalPowerSeries<T> lhs, FormalPowerSeries<T> rhs)
            => new Impl(lhs).Divide(rhs.Coefficients).ToFps();
        /// <summary>
        /// <paramref name="lhs"/> / <paramref name="rhs"/>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O( N log N )</para>
        /// </remarks>
        [凾(256)]
        public static FormalPowerSeries<T> operator %(FormalPowerSeries<T> lhs, FormalPowerSeries<T> rhs)
            => lhs.DivRem(rhs).Remainder;

        /// <summary>
        /// <see langword="this"/> / <paramref name="other"/> の商とあまり
        /// </summary>
        /// <remarks>
        /// <para>計算量: O( N log N )</para>
        /// </remarks>
        /// <param name="other">割る多項式</param>
        /// <returns>(商, あまり) のタプル</returns>
        [凾(256)]
        public (FormalPowerSeries<T> Quotient, FormalPowerSeries<T> Remainder) DivRem(FormalPowerSeries<T> other)
        {
            var (q, r) = new Impl(this).DivRem(other.Coefficients);
            return (new FormalPowerSeries<T>(q), new FormalPowerSeries<T>(r));
        }
        #endregion Divide

        /// <summary>
        /// 次数を <paramref name="sz"/> だけ小さくする。
        /// </summary>
        [凾(256)]
        public static FormalPowerSeries<T> operator >>(FormalPowerSeries<T> v, int sz)
            => new Impl(v).RightShift(sz).ToFps();

        /// <summary>
        /// 次数を <paramref name="sz"/> だけ大きくする。
        /// </summary>
        [凾(256)]
        public static FormalPowerSeries<T> operator <<(FormalPowerSeries<T> v, int sz)
            => new Impl(v).LeftShift(sz).ToFps();

        /// <summary>
        /// 微分した多項式を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public FormalPowerSeries<T> Derivative() => new Impl(this).Derivative().ToFps();
        /// <summary>
        /// 積分した多項式を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public FormalPowerSeries<T> Integrate() => new Impl(this).Integrate().ToFps();

        /// <summary>
        /// 多項式に <paramref name="x"/> を代入した値を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public StaticModInt<T> Eval(StaticModInt<T> x)
        {
            var x_n = x;
            var res = Coefficients[0];
            foreach (var c in Coefficients.AsSpan(1))
            {
                res += c * x_n;
                x_n *= x;
            }
            return res;
        }

        /// <summary>
        /// 1/f(x) となる多項式の先頭 N 項。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        /// <example>https://judge.yosupo.jp/problem/inv_of_formal_power_series</example>
        [凾(256)]
        public FormalPowerSeries<T> Inv(int deg = -1) => new Impl(this).Inv(deg).ToFps();

        /// <summary>
        /// exp(f(x)) となる多項式の先頭 N 項。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        /// <example>https://judge.yosupo.jp/problem/exp_of_formal_power_series</example>
        [凾(256)]
        public FormalPowerSeries<T> Exp(int deg = -1) => new Impl(this).Exp(deg).ToFps();

        /// <summary>
        /// log(f(x)) となる多項式の先頭 N 項。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        /// <example>https://judge.yosupo.jp/problem/log_of_formal_power_series</example>
        [凾(256)]
        public FormalPowerSeries<T> Log(int deg = -1) => new Impl(this).Log(deg).ToFps();

        /// <summary>
        /// (f(x))^<paramref name="k"/> となる多項式の先頭 N 項。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        /// <example>https://judge.yosupo.jp/problem/pow_of_formal_power_series</example>
        [凾(256)]
        public FormalPowerSeries<T> Pow(long k, int deg = -1) => new Impl(this).Pow(k, deg).ToFps();
    }
}