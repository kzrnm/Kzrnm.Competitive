using AtCoder;
using System;
using System.Diagnostics;
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
        public readonly MontgomeryModInt<T>[] Coefficients;

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
            : this(Shrink(polynomial.Select(n => new MontgomeryModInt<T>((ulong)n)))) { }

        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="polynomial"/>[i]&lt;mod</para>
        /// </remarks>
        /// <param name="polynomial"><paramref name="polynomial"/>[i] がi次の係数となる多項式</param>
        public FormalPowerSeries(ReadOnlySpan<int> polynomial)
            : this(Shrink(polynomial.Select(n => new MontgomeryModInt<T>(n)))) { }

        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        /// <param name="polynomial"><paramref name="polynomial"/>[i] がi次の係数となる多項式</param>
        public FormalPowerSeries(ReadOnlySpan<StaticModInt<T>> polynomial)
            : this(Shrink(polynomial.Select(n => new MontgomeryModInt<T>((ulong)n.Value)))) { }

        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        /// <param name="polynomial"><paramref name="polynomial"/>[i] がi次の係数となる多項式</param>
        public FormalPowerSeries(ReadOnlySpan<MontgomeryModInt<T>> polynomial)
            : this(Shrink(polynomial).ToArray()) { }

        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        private FormalPowerSeries(MontgomeryModInt<T>[] polynomial)
        {
            Coefficients = polynomial;
        }
        public override string ToString() => string.Join(", ", Coefficients);
        private static ReadOnlySpan<MontgomeryModInt<T>> Shrink(ReadOnlySpan<MontgomeryModInt<T>> polynomial)
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
            return lhs.ToImpl().Add(rhs.Coefficients).ToFps();
        }

        /// <summary>
        /// <paramref name="lhs"/> + <paramref name="rhs"/>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public static FormalPowerSeries<T> operator +(FormalPowerSeries<T> lhs, ReadOnlySpan<MontgomeryModInt<T>> rhs)
        {
            if (rhs.Length == 0) return lhs;
            if (lhs.Coefficients.Length == 0) return new FormalPowerSeries<T>(rhs);
            return lhs.ToImpl().Add(rhs).ToFps();
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
            return lhs.ToImpl().Subtract(rhs.Coefficients).ToFps();
        }

        /// <summary>
        /// <paramref name="lhs"/> - <paramref name="rhs"/>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public static FormalPowerSeries<T> operator -(FormalPowerSeries<T> lhs, ReadOnlySpan<MontgomeryModInt<T>> rhs)
        {
            if (rhs.Length == 0) return lhs;
            if (lhs.Coefficients.Length == 0) return new Impl(rhs.ToArray()).Minus().ToFps();
            return lhs.ToImpl().Subtract(rhs).ToFps();
        }

        /// <summary>
        /// -<paramref name="v"/>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        public static FormalPowerSeries<T> operator -(FormalPowerSeries<T> v)
            => v.ToImpl().Minus().ToFps();
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
            => lhs.ToImpl().Multiply(rhs.Coefficients).ToFps();

        [凾(256)]
        public static FormalPowerSeries<T> operator *(MontgomeryModInt<T> lhs, FormalPowerSeries<T> rhs)
            => rhs.ToImpl().Multiply(lhs).ToFps();
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
            => lhs.ToImpl().Divide(rhs.Coefficients).ToFps();
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
            var (q, r) = ToImpl().DivRem(other.Coefficients);
            return (new FormalPowerSeries<T>(q), new FormalPowerSeries<T>(r));
        }
        #endregion Divide

        /// <summary>
        /// 次数を <paramref name="sz"/> だけ小さくする。
        /// </summary>
        [凾(256)]
        public static FormalPowerSeries<T> operator >>(FormalPowerSeries<T> v, int sz)
            => v.ToImpl().RightShift(sz).ToFps();

        /// <summary>
        /// 次数を <paramref name="sz"/> だけ大きくする。
        /// </summary>
        [凾(256)]
        public static FormalPowerSeries<T> operator <<(FormalPowerSeries<T> v, int sz)
            => v.ToImpl().LeftShift(sz).ToFps();


        /// <summary>
        /// 多項式に <paramref name="x"/> を代入した値を返します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public MontgomeryModInt<T> Eval(MontgomeryModInt<T> x)
        {
            var x_n = MontgomeryModInt<T>.One;
            MontgomeryModInt<T> res = 0;
            foreach (var c in Coefficients)
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
        public FormalPowerSeries<T> Inv(int deg = -1) => ToImpl().Inv(deg).ToFps();
    }
}