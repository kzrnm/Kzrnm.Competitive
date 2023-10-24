using AtCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        /// 多項式の係数(Coefficients)
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal MontgomeryModInt<T>[] _cs;

        public MontgomeryModInt<T> this[int i] { [凾(256)] get => _cs[i]; }

        /// <summary>
        /// 係数を <see cref="ReadOnlySpan{T}"/> として取得します。
        /// </summary>
        /// <param name="deg">先頭の <paramref name="deg"/> 項を取得します。負数のときは元の FPS と同じ長さで取得します。</param>
        [凾(256)]
        public ReadOnlySpan<MontgomeryModInt<T>> Coefficients(int deg = -1)
        {
            if (deg < 0)
                return _cs;
            if ((uint)deg < (uint)_cs.Length)
                return _cs.AsSpan()[..deg];
            var rt = new MontgomeryModInt<T>[deg];
            _cs.AsSpan().CopyTo(rt);
            return rt;
        }
        [凾(256)] public ReadOnlySpan<MontgomeryModInt<T>> AsSpan() => _cs.AsSpan();
        [凾(256)] public static implicit operator ReadOnlySpan<MontgomeryModInt<T>>(FormalPowerSeries<T> f) => f._cs.AsSpan();


        /// <summary>
        /// 多項式の次数
        /// </summary>
        public int Count => _cs.Length;

        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="polynomial"/>[i]&lt;mod</para>
        /// </remarks>
        /// <param name="polynomial"><paramref name="polynomial"/>[i] がi次の係数となる多項式</param>
        public FormalPowerSeries(ReadOnlySpan<uint> polynomial)
            : this(Shrink(polynomial).Select(n => (MontgomeryModInt<T>)n)) { }

        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="polynomial"/>[i]&lt;mod</para>
        /// </remarks>
        /// <param name="polynomial"><paramref name="polynomial"/>[i] がi次の係数となる多項式</param>
        public FormalPowerSeries(ReadOnlySpan<int> polynomial)
            : this(Shrink(polynomial).Select(n => (MontgomeryModInt<T>)n)) { }

        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        /// <param name="polynomial"><paramref name="polynomial"/>[i] がi次の係数となる多項式</param>
        public FormalPowerSeries(ReadOnlySpan<MontgomeryModInt<T>> polynomial)
            : this(Shrink(polynomial).ToArray()) { }

        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        public FormalPowerSeries(MontgomeryModInt<T>[] polynomial)
        {
            _cs = polynomial.Length == 0 || polynomial[^1].Value != 0
                ? polynomial
                : Shrink((ReadOnlySpan<MontgomeryModInt<T>>)polynomial).ToArray();
        }
        static ReadOnlySpan<R> Shrink<R>(ReadOnlySpan<R> s)
        {
            while (s.Length > 0 && EqualityComparer<R>.Default.Equals(s[^1], default))
                s = s[..^1];
            return s;
        }
        public override string ToString() => string.Join(", ", _cs);
        [凾(256)] internal FpsImpl<T> ToImpl(bool breaking = false) => new FpsImpl<T>(breaking ? _cs : _cs.ToArray());

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
            if (rhs._cs.Length == 0) return lhs;
            if (lhs._cs.Length == 0) return rhs;
            return lhs.ToImpl().Add(rhs._cs).ToFps();
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
            if (lhs._cs.Length == 0) return new FormalPowerSeries<T>(rhs);
            return lhs.ToImpl().Add(rhs).ToFps();
        }

        /// <summary>
        /// <paramref name="rhs"/> を破壊的に加算します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public FormalPowerSeries<T> AddSelf(FormalPowerSeries<T> rhs)
        {
            if (rhs._cs.Length == 0) return this;
            _cs = ToImpl(true).Add(rhs._cs).a;
            return this;
        }

        /// <summary>
        /// <paramref name="rhs"/> を破壊的に加算します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public FormalPowerSeries<T> AddSelf(ReadOnlySpan<MontgomeryModInt<T>> rhs)
        {
            if (rhs.Length == 0) return this;
            _cs = ToImpl(true).Add(rhs).a;
            return this;
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
            if (rhs._cs.Length == 0) return lhs;
            if (lhs._cs.Length == 0) return -rhs;
            return lhs.ToImpl().Subtract(rhs._cs).ToFps();
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
            if (lhs._cs.Length == 0) return new FpsImpl<T>(rhs.ToArray()).Minus().ToFps();
            return lhs.ToImpl().Subtract(rhs).ToFps();
        }

        /// <summary>
        /// <paramref name="rhs"/> を破壊的に減算します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public FormalPowerSeries<T> SubtractSelf(FormalPowerSeries<T> rhs)
        {
            if (rhs._cs.Length == 0) return this;
            _cs = ToImpl(true).Subtract(rhs._cs).a;
            return this;
        }

        /// <summary>
        /// <paramref name="rhs"/> を破壊的に減算します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public FormalPowerSeries<T> SubtractSelf(ReadOnlySpan<MontgomeryModInt<T>> rhs)
        {
            if (rhs.Length == 0) return this;
            _cs = ToImpl(true).Subtract(rhs).a;
            return this;
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
            => lhs.ToImpl().Multiply(rhs._cs).ToFps();

        /// <summary>
        /// <paramref name="lhs"/> * <paramref name="rhs"/>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O( L log L ) ただし L = len(<paramref name="lhs"/>) + len(<paramref name="rhs"/>) </para>
        /// </remarks>
        [凾(256)]
        public static FormalPowerSeries<T> operator *(FormalPowerSeries<T> lhs, ReadOnlySpan<MontgomeryModInt<T>> rhs)
            => lhs.ToImpl().Multiply(rhs).ToFps();

        /// <summary>
        /// <paramref name="rhs"/> を破壊的に乗算します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O( L log L ) ただし L = len(<see cref="_cs"/>) + len(<paramref name="rhs"/>) </para>
        /// </remarks>
        [凾(256)]
        public FormalPowerSeries<T> MultiplySelf(FormalPowerSeries<T> rhs)
        {
            _cs = ToImpl(true).Multiply(rhs._cs).a;
            return this;
        }

        /// <summary>
        /// <paramref name="rhs"/> を破壊的に乗算します。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O( L log L ) ただし L = len(<see cref="_cs"/>) + len(<paramref name="rhs"/>) </para>
        /// </remarks>
        [凾(256)]
        public FormalPowerSeries<T> MultiplySelf(ReadOnlySpan<MontgomeryModInt<T>> rhs)
        {
            _cs = ToImpl(true).Multiply(rhs).a;
            return this;
        }

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
            => lhs.ToImpl().Divide(rhs._cs).ToFps();
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
            var (q, r) = ToImpl().DivRem(other._cs);
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
            foreach (var c in _cs)
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
        /// <param name="deg">先頭の <paramref name="deg"/> 項を取得します。負数のときは元の FPS と同じ長さで取得します。</param>
        /// <example>https://judge.yosupo.jp/problem/inv_of_formal_power_series</example>
        [凾(256)]
        public FormalPowerSeries<T> Inv(int deg = -1) => ToImpl().Inv(deg).ToFps();
    }
}