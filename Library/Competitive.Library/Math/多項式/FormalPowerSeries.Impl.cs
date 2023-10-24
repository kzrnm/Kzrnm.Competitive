using AtCoder;
using AtCoder.Internal;
using System;
using System.Runtime.CompilerServices;
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
        [凾(256)] internal Impl ToImpl() => new Impl(this);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "気にしない")]
        internal ref struct Impl
        {
            public MontgomeryModInt<T>[] a;
            public int Length;
            public Span<MontgomeryModInt<T>> AsSpan() => a.AsSpan(0, Length);

            public Impl Set(MontgomeryModInt<T>[] value)
            {
                a = value;
                Length = value.Length;
                return this;
            }

            public Impl(FormalPowerSeries<T> fps) : this((MontgomeryModInt<T>[])fps.Coefficients.Clone()) { }
            public Impl(MontgomeryModInt<T>[] array)
            {
                a = array;
                Length = array.Length;
            }

            [凾(256)]
            public FormalPowerSeries<T> ToFps()
            {
                var s = AsSpan();
                if (s.Length == a.Length && a.Length > 0 && a[^1].Value != 0)
                    return new FormalPowerSeries<T>(a);
                return new FormalPowerSeries<T>(s);
            }

            /// <remarks>
            /// <para>計算量: O(N)</para>
            /// </remarks>
            [凾(256)]
            public Impl Add(ReadOnlySpan<MontgomeryModInt<T>> rhs)
            {
                ref var lp = ref a[0];
                ref var rp = ref MemoryMarshal.GetReference(rhs);
                var arr = new MontgomeryModInt<T>[Math.Max(Length, rhs.Length)];
                for (int i = 0; i < arr.Length; i++)
                {
                    var lv = i < Length ? Unsafe.Add(ref lp, i) : default;
                    var rv = i < rhs.Length ? Unsafe.Add(ref rp, i) : default;
                    arr[i] = lv + rv;
                }
                return Set(arr);
            }
            /// <remarks>
            /// <para>計算量: O(N)</para>
            /// </remarks>
            [凾(256)] public Impl Add(Impl rhs) => Add(rhs.a);
            /// <remarks>
            /// <para>計算量: O(N)</para>
            /// </remarks>
            [凾(256)]
            public Impl Add(MontgomeryModInt<T> v)
            {
                if (Length == 0)
                    Set(new MontgomeryModInt<T>[] { v });
                else
                    a[0] += v;
                return this;
            }
            /// <remarks>
            /// <para>計算量: O(N)</para>
            /// </remarks>
            [凾(256)]
            public Impl Subtract(ReadOnlySpan<MontgomeryModInt<T>> rhs)
            {
                if (Length == 0)
                {
                    return Set(rhs.ToArray()).Minus();
                }
                ref var lp = ref a[0];
                ref var rp = ref MemoryMarshal.GetReference(rhs);
                var arr = new MontgomeryModInt<T>[Math.Max(Length, rhs.Length)];
                for (int i = 0; i < arr.Length; i++)
                {
                    var lv = i < Length ? Unsafe.Add(ref lp, i) : default;
                    var rv = i < rhs.Length ? Unsafe.Add(ref rp, i) : default;
                    arr[i] = lv - rv;
                }
                return Set(arr);
            }
            /// <remarks>
            /// <para>計算量: O(N)</para>
            /// </remarks>
            [凾(256)]
            public Impl Subtract(MontgomeryModInt<T> v)
            {
                if (Length == 0)
                    Set(new MontgomeryModInt<T>[] { v });
                else
                    a[0] -= v;
                return this;
            }
            /// <remarks>
            /// <para>計算量: O(N)</para>
            /// </remarks>
            [凾(256)] public Impl Subtract(Impl rhs) => Subtract(rhs.a);

            /// <remarks>
            /// <para>計算量: O(N)</para>
            /// </remarks>
            [凾(256)]
            public Impl Minus()
            {
                if (Length > 0)
                {
                    for (int i = 0; i < a.Length; i++)
                        a[i] = -a[i];
                }
                return this;
            }

            /// <remarks>
            /// <para>計算量: O(N log N)</para>
            /// </remarks>
            [凾(256)]
            public Impl Multiply(MontgomeryModInt<T> m)
            {
                for (int i = Length - 1; i >= 0; i--)
                    a[i] *= m;
                return this;
            }
            /// <remarks>
            /// <para>計算量: O(N log N)</para>
            /// </remarks>
            [凾(256)]
            public Impl Multiply(ReadOnlySpan<MontgomeryModInt<T>> rhs)
                => Set(NumberTheoreticTransform.Convolution(AsSpan(), rhs));
            /// <remarks>
            /// <para>計算量: O(N log N)</para>
            /// </remarks>
            [凾(256)] public Impl Multiply(Impl rhs) => Multiply(rhs.a);


            /// <remarks>
            /// <para>計算量: O(N log N)</para>
            /// </remarks>
            [凾(256)]
            public Impl Divide(ReadOnlySpan<MontgomeryModInt<T>> rhs)
            {
                if (Length < rhs.Length)
                    return Set(Array.Empty<MontgomeryModInt<T>>());

                if (rhs.Length <= 64)
                    return DivideNative(rhs);

                int n = Length - rhs.Length + 1;
                var a = new Impl(AsSpan().ToArray()).Reverse().Pre(n).AsSpan();
                var b = new Impl(rhs.ToArray()).Reverse().Inv(n).AsSpan();

                var m = NumberTheoreticTransform.Convolution(a, b);
                return Set(m).Pre(n).Reverse();
            }

            private Impl DivideNative(ReadOnlySpan<MontgomeryModInt<T>> rhs)
            {
                var f = a;
                var g = rhs.ToArray();
                var coeff = g[^1].Inv();
                for (int i = 0; i < g.Length; i++)
                    g[i] *= coeff;
                int n = f.Length - g.Length + 1;

                var quotient = new MontgomeryModInt<T>[n];
                for (int i = quotient.Length - 1; i >= 0; i--)
                {
                    quotient[i] = f[i + g.Length - 1];
                    for (int j = 0; j < g.Length; j++)
                        f[i + j] -= quotient[i] * g[j];
                }

                for (int i = 0; i < quotient.Length; i++)
                    quotient[i] *= coeff;

                return Set(quotient);
            }

            /// <summary>
            /// 商・あまりを求める。this は商を保持。
            /// </summary>
            /// <param name="rhs"></param>
            /// <returns></returns>
            public (MontgomeryModInt<T>[] Quotient, MontgomeryModInt<T>[] Remainder) DivRem(ReadOnlySpan<MontgomeryModInt<T>> rhs)
            {
                var arr = AsSpan().ToArray();
                var qw = Divide(rhs);
                var quotient = qw.AsSpan().ToArray();
                var remainder = new Impl(arr).Subtract(NumberTheoreticTransform.Convolution(quotient, rhs)).AsSpan();
                return (quotient, remainder.ToArray());
            }

            /// <summary>
            /// 次数を <paramref name="sz"/> だけ小さくする。
            /// </summary>
            [凾(256)]
            public Impl RightShift(int sz)
                => sz > 0 ? Set(AsSpan()[sz..].ToArray()) : this;


            /// <summary>
            /// 次数を <paramref name="sz"/> だけ大きくする。
            /// </summary>
            [凾(256)]
            public Impl LeftShift(int sz)
            {
                if (sz == 0)
                    return this;
                var arr = new MontgomeryModInt<T>[Length + sz];
                AsSpan().CopyTo(arr.AsSpan(sz));
                return Set(arr);
            }

            [凾(256)]
            public Impl Reverse()
            {
                AsSpan().Reverse();
                return this;
            }
            [凾(256)]
            public Impl Pre(int sz)
            {
                if (sz < Length)
                    Length = sz;
                return this;
            }
            [凾(256)]
            public Impl Inv(int deg = -1)
            {
                Contract.Assert(a[0].Value != 0);
                if (deg < 0) deg = Length;
                return deg <= NumberTheoreticTransform<T>.NttLength() ? InvNtt(deg) : InvAnyMod(deg);

            }
            private Impl InvNtt(int deg)
            {
                var r = new MontgomeryModInt<T>[deg];
                r[0] = a[0].Inv();
                for (int d = 1; d < deg; d <<= 1)
                {
                    var f = new MontgomeryModInt<T>[2 * d];
                    var g = new MontgomeryModInt<T>[2 * d];

                    a.AsSpan(0, Math.Min(f.Length, Length)).CopyTo(f);
                    r.AsSpan(0, d).CopyTo(g);

                    NumberTheoreticTransform<T>.Ntt(f);
                    NumberTheoreticTransform<T>.Ntt(g);

                    for (int j = 0; j < 2 * d; j++) f[j] *= g[j];
                    NumberTheoreticTransform<T>.INtt(f);
                    for (int j = 0; j < d; j++) f[j] = 0;
                    NumberTheoreticTransform<T>.Ntt(f);
                    for (int j = 0; j < 2 * d; j++) f[j] *= g[j];
                    NumberTheoreticTransform<T>.INtt(f);

                    for (int j = Math.Min(r.Length, f.Length) - 1; j >= d; j--)
                        r[j] = -f[j];
                }
                return Set(r).Pre(deg);
            }
            private Impl InvAnyMod(int deg)
            {
                var r = new MontgomeryModInt<T>[1] { a[0].Inv() };
                for (int i = 1; i < deg; i <<= 1)
                {
                    var rp = new MontgomeryModInt<T>[r.Length];
                    for (int j = 0; j < r.Length; j++)
                        rp[j] = r[j] + r[j];

                    var rc = NumberTheoreticTransform.Convolution(NumberTheoreticTransform.Convolution(r, r), a.AsSpan(0, Math.Min(Length, i << 1)));

                    var r2 = new MontgomeryModInt<T>[1 << i];
                    for (int j = 0; j < r2.Length; j++)
                    {
                        r2[j] = (uint)j < (uint)r.Length ? r[j] + r[j] : default;
                        if ((uint)j < (uint)rc.Length)
                            r2[j] -= rc[j];
                    }
                    r = r2;
                }
                return Set(r).Pre(deg);
            }
        }
    }
}