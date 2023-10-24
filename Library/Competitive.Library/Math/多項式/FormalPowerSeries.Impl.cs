using AtCoder;
using AtCoder.Internal;
using System;
using System.Diagnostics;
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "気にしない")]
        internal ref struct Impl
        {
            public MontgomeryModInt<T>[] a;
            public int Length;
            public Span<MontgomeryModInt<T>> AsSpan() => a.AsSpan(0, Length);

            private Impl Set(MontgomeryModInt<T>[] value)
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

            /// <summary>
            /// 微分した多項式を返します。
            /// </summary>
            /// <remarks>
            /// <para>計算量: O(N)</para>
            /// </remarks>
            /// <returns></returns>
            [凾(256)]
            public Impl Derivative()
            {
                if (Length > 0)
                {
                    for (int i = 1; i < a.Length; i++)
                        a[i - 1] = a[i] * i;
                    --Length;
                }
                return this;
            }
            /// <summary>
            /// 積分した多項式を返します。
            /// </summary>
            /// <remarks>
            /// <para>計算量: O(N)</para>
            /// </remarks>
            /// <returns></returns>
            [凾(256)]
            public Impl Integrate()
            {
                var res = new MontgomeryModInt<T>[Length + 1];
                for (int i = 1; i < res.Length; i++)
                    res[i] = a[i - 1] / i;
                return Set(res);
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

            [凾(256)]
            public Impl Exp(int deg = -1)
            {
                Contract.Assert(Length == 0 || a[0].Value == 0);
                if (deg < 0) deg = Length;
                if (deg == 0)
                    return Set(new MontgomeryModInt<T>[] { MontgomeryModInt<T>.One });
                return deg <= NumberTheoreticTransform<T>.NttLength() ? ExpNtt(deg) : ExpAnyMod(deg);
            }

            [凾(256)]
            public Impl ExpAnyMod(int deg)
            {
                var one = MontgomeryModInt<T>.One;
                var ret = new Impl(new MontgomeryModInt<T>[] { one });
                for (int i = 1; i < deg; i <<= 1)
                {
                    var p = new Impl(AsSpan().ToArray());
                    var r2 = new Impl(ret.AsSpan().ToArray());
                    ret = ret.Multiply(p.Pre(i << 1).Add(one).Subtract(r2.Log(i << 1))).Pre(i << 1);
                }
                return this = ret.Pre(deg);
            }

            class SimpleDeque
            {
                MontgomeryModInt<T>[] array;
                int f, t;
                public SimpleDeque(int size)
                {
                    array = new MontgomeryModInt<T>[size * 2];
                    f = t = size;
                }
                public int Length => t - f;
                [凾(256)] public void Resize(int size) => t = f + size;
                [凾(256)] public void AddFirst(MontgomeryModInt<T> v) => array[--f] = v;
                [凾(256)] public void AddLast(MontgomeryModInt<T> v) => array[t++] = v;
                [凾(256)] public void PopFirst() => ++f;
                [凾(256)] public void PopLast() => --t;
                [凾(256)] public Span<MontgomeryModInt<T>> AsSpan() => array.AsSpan(f, Length);
            }
            ref struct ExpNttInv
            {
                int Length;
                readonly MontgomeryModInt<T>[] array;
                public ExpNttInv(int s)
                {
                    array = new MontgomeryModInt<T>[s];
                    Length = 0;
                }
                [凾(256)] public void Add(MontgomeryModInt<T> v) => array[Length++] = v;
                public void InplaceIntegrate(SimpleDeque f)
                {
                    var mod = (int)new T().Mod;
                    var n = f.Length;
                    for (ref int i = ref Length; i <= n;)
                        Add((-array[mod % i]) * (mod / i));
                    f.AddFirst(default);
                    var fs = f.AsSpan();
                    Debug.Assert(n + 1 == fs.Length);
                    for (int i = 1; i < fs.Length; i++) fs[i] *= array[i];
                }

#pragma warning disable CA1822 // メンバーを static に設定します
                public void InplaceDerivative(SimpleDeque f)
#pragma warning restore CA1822 // メンバーを static に設定します
                {
                    if (f.Length > 0)
                    {
                        f.PopFirst();
                        var fs = f.AsSpan();
                        for (int i = 0; i < fs.Length; i++)
                            fs[i] *= i + 1;
                    }
                }
            }

            [凾(256)]
            public Impl ExpNtt(int deg)
            {
                var inv = new ExpNttInv(3 * deg);
                inv.Add(MontgomeryModInt<T>.Zero);
                inv.Add(MontgomeryModInt<T>.One);

                var b = new SimpleDeque(3 * deg);
                b.AddLast(1);
                b.AddLast(1 < Length ? a[1] : 0);

                var c = new SimpleDeque(deg);
                c.AddLast(MontgomeryModInt<T>.One);

                Span<MontgomeryModInt<T>> z2 = stackalloc MontgomeryModInt<T>[2];
                z2.Fill(MontgomeryModInt<T>.One);

                for (int m = 2; m < deg; m *= 2)
                {
                    var y = new MontgomeryModInt<T>[2 * m];
                    b.AsSpan().CopyTo(y);

                    NumberTheoreticTransform<T>.Ntt(y);

                    var z1 = z2;
                    var z = new MontgomeryModInt<T>[m];
                    for (int i = 0; i < z.Length; i++) z[i] = y[i] * z1[i];
                    NumberTheoreticTransform<T>.INtt(z);
                    z.AsSpan(0, m / 2).Clear();
                    NumberTheoreticTransform<T>.Ntt(z);
                    for (int i = 0; i < z.Length; i++) z[i] *= -z1[i];
                    NumberTheoreticTransform<T>.INtt(z);

                    for (int i = m / 2; i < z.Length; i++) c.AddLast(z[i]);

                    z2 = new MontgomeryModInt<T>[2 * m];
                    c.AsSpan().CopyTo(z2);
                    NumberTheoreticTransform<T>.Ntt(z2);

                    var x = new SimpleDeque(3 * m);
                    foreach (var v in a.AsSpan(0, Math.Min(Length, m)))
                        x.AddLast(v);
                    x.Resize(m);

                    inv.InplaceDerivative(x);
                    x.AddLast(default);
                    {
                        var xs = x.AsSpan();
                        NumberTheoreticTransform<T>.Ntt(xs);
                        for (int i = 0; i < m; ++i) xs[i] *= y[i];
                        NumberTheoreticTransform<T>.INtt(xs);

                        var bd = new Impl(b.AsSpan().ToArray()).Derivative().AsSpan();
                        for (int i = 0; i < bd.Length; i++) xs[i] -= bd[i];
                    }

                    x.Resize(2 * m);
                    {
                        var xs = x.AsSpan();

                        for (int i = 0; i + 1 < m; ++i)
                        {
                            xs[m + i] = xs[i];
                            xs[i] = default;
                        }
                        NumberTheoreticTransform<T>.Ntt(xs);
                        for (int i = 0; i < xs.Length; ++i) xs[i] *= z2[i];
                        NumberTheoreticTransform<T>.INtt(xs);
                    }

                    x.PopLast();
                    inv.InplaceIntegrate(x);
                    {
                        var xs = x.AsSpan();
                        Debug.Assert(2 * m == xs.Length);
                        for (int i = 0; i < Math.Min(Length, xs.Length); ++i) xs[i] += a[i];
                        xs[..m].Clear();

                        NumberTheoreticTransform<T>.Ntt(xs);
                        for (int i = 0; i < xs.Length; ++i) xs[i] *= y[i];
                        NumberTheoreticTransform<T>.INtt(xs);

                        foreach (var v in xs[m..])
                            b.AddLast(v);
                    }
                }
                return Set(b.AsSpan()[..deg].ToArray());
            }

            [凾(256)]
            public Impl Log(int deg = -1)
            {
                Contract.Assert(a[0].Value == 1);
                if (deg < 0) deg = Length;
                var inv = new Impl(AsSpan().ToArray()).Inv(deg);
                return Derivative().Multiply(inv).Pre(deg - 1).Integrate();
            }

            [凾(256)]
            public Impl Pow(long k, int deg = -1)
            {
                if (deg < 0) deg = Length;
                if (k == 0)
                    return new Impl(new[] { MontgomeryModInt<T>.One });

                var span = AsSpan();
                for (int i = 0; i < span.Length; i++)
                {
                    if (span[i].Value != 0)
                    {
                        var rev = span[i].Inv();
                        var right = span[i].Pow(k);

                        return Multiply(rev).RightShift(i).Log(deg).Multiply(k).Exp(deg).Multiply(right)
                            .LeftShift((int)(i * k)).Pre(deg);
                    }
                    if (Math.Max((i + 1) * k, k) > deg)
                        return Set(new MontgomeryModInt<T>[deg]);
                }
                return Set(new MontgomeryModInt<T>[deg]);
            }


            [凾(256)]
            public Impl Ntt()
            {
                NumberTheoreticTransform<T>.Ntt(AsSpan());
                return this;
            }
            [凾(256)]
            public Impl INtt()
            {
                NumberTheoreticTransform<T>.INtt(AsSpan());
                return this;
            }
            [凾(256)]
            public Impl NttDoubling()
            {
                Set(NumberTheoreticTransform<T>.NttDoubling(AsSpan()));
                return this;
            }
        }
    }
}