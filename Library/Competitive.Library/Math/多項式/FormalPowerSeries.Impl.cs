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
        internal ref struct Impl
        {
            public StaticModInt<T>[] a;
            public int Length;
            public Span<StaticModInt<T>> AsSpan() => a.AsSpan(0, Length);

            private Impl Set(StaticModInt<T>[] value)
            {
                a = value;
                Length = value.Length;
                return this;
            }

            public Impl(FormalPowerSeries<T> fps) : this((StaticModInt<T>[])fps.Coefficients.Clone()) { }
            public Impl(StaticModInt<T>[] array)
            {
                a = array;
                Length = array.Length;
            }

            [凾(256)]
            public FormalPowerSeries<T> ToFps()
            {
                var arr = a;
                var shrinked = Shrink(AsSpan());
                if (arr.Length != shrinked.Length)
                    arr = shrinked.ToArray();
                return new FormalPowerSeries<T>(arr, false);
            }

            /// <remarks>
            /// <para>計算量: O(N)</para>
            /// </remarks>
            [凾(256)]
            public Impl Add(ReadOnlySpan<StaticModInt<T>> rhs)
            {
                ref var lp = ref a[0];
                ref var rp = ref MemoryMarshal.GetReference(rhs);
                var arr = new StaticModInt<T>[Math.Max(Length, rhs.Length)];
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
            public Impl Subtract(ReadOnlySpan<StaticModInt<T>> rhs)
            {
                ref var lp = ref a[0];
                ref var rp = ref MemoryMarshal.GetReference(rhs);
                var arr = new StaticModInt<T>[Math.Max(Length, rhs.Length)];
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
            public Impl Add(StaticModInt<T> v)
            {
                if (Length == 0)
                    Set(new StaticModInt<T>[] { v });
                else
                    a[0] += v;
                return this;
            }
            /// <remarks>
            /// <para>計算量: O(N)</para>
            /// </remarks>
            [凾(256)]
            public Impl Subtract(StaticModInt<T> v)
            {
                if (Length == 0)
                    Set(new StaticModInt<T>[] { v });
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
            public Impl Multiply(StaticModInt<T> m)
            {
                for (int i = Length - 1; i >= 0; i--)
                    a[i] *= m;
                return this;
            }
            /// <remarks>
            /// <para>計算量: O(N log N)</para>
            /// </remarks>
            [凾(256)]
            public Impl Multiply(ReadOnlySpan<StaticModInt<T>> rhs)
                => Set(ConvolutionAnyMod.Convolution(AsSpan(), rhs));
            /// <remarks>
            /// <para>計算量: O(N log N)</para>
            /// </remarks>
            [凾(256)] public Impl Multiply(Impl rhs) => Multiply(rhs.a);


            /// <remarks>
            /// <para>計算量: O(N log N)</para>
            /// </remarks>
            [凾(256)]
            public Impl Divide(ReadOnlySpan<StaticModInt<T>> rhs)
            {
                if (Length < rhs.Length)
                    return Set(Array.Empty<StaticModInt<T>>());

                if (rhs.Length <= 64)
                    return DivideNative(rhs);

                int n = Length - rhs.Length + 1;
                return new Impl(
                    ConvolutionAnyMod.Convolution(
                        new Impl(AsSpan().ToArray()).Reverse().Pre(n).AsSpan(),
                        new Impl(rhs.ToArray()).Reverse().Inv(n).AsSpan())
                    ).Pre(n).Reverse();
            }

            private Impl DivideNative(ReadOnlySpan<StaticModInt<T>> rhs)
            {
                var f = a;
                var g = rhs.ToArray();
                var coeff = g[^1].Inv();
                for (int i = 0; i < g.Length; i++)
                    g[i] *= coeff;
                int n = f.Length - g.Length + 1;

                var quotient = new StaticModInt<T>[n];
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
            public (StaticModInt<T>[] Quotient, StaticModInt<T>[] Remainder) DivRem(ReadOnlySpan<StaticModInt<T>> rhs)
            {
                var arr = AsSpan().ToArray();
                var qw = Divide(rhs);
                var quotient = qw.AsSpan().ToArray();
                var remainder = new Impl(arr).Subtract(ConvolutionAnyMod.Convolution(quotient, rhs)).AsSpan();
                return (quotient, remainder.ToArray());
            }

            /// <summary>
            /// 次数を <paramref name="sz"/> だけ小さくする。
            /// </summary>
            [凾(256)]
            public Impl RightShift(int sz)
                => Set(AsSpan()[Math.Min(sz, Length)..].ToArray());

            /// <summary>
            /// 次数を <paramref name="sz"/> だけ大きくする。
            /// </summary>
            [凾(256)]
            public Impl LeftShift(int sz)
            {
                var arr = new StaticModInt<T>[Length + sz];
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
                        a[i - 1] = a[i] * StaticModInt<T>.Raw(i);
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
                var res = new StaticModInt<T>[Length + 1];
                for (int i = 1; i < res.Length; i++)
                    res[i] = a[i - 1] / StaticModInt<T>.Raw(i);

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
                return NumberTheoreticTransform<T>.CanNtt() ? InvNtt(deg) : InvAnyMod(deg);

            }
            private Impl InvNtt(int deg)
            {
                var r = new StaticModInt<T>[deg];
                r[0] = a[0].Inv();
                for (int d = 1; d < deg; d <<= 1)
                {
                    var f = new StaticModInt<T>[2 * d];
                    var g = new StaticModInt<T>[2 * d];

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
                var r = new StaticModInt<T>[1] { a[0].Inv() };
                for (int i = 1; i < deg; i <<= 1)
                {
                    var rp = new StaticModInt<T>[r.Length];
                    for (int j = 0; j < r.Length; j++)
                        rp[j] = r[j] + r[j];

                    var rc = ConvolutionAnyMod.Convolution(ConvolutionAnyMod.Convolution(r, r), a.AsSpan(0, Math.Min(Length, i << 1)));

                    var r2 = new StaticModInt<T>[1 << i];
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
                return NumberTheoreticTransform<T>.CanNtt() ? ExpNtt(deg) : ExpAnyMod(deg);
            }

            [凾(256)]
            public Impl ExpAnyMod(int deg)
            {
                var one = StaticModInt<T>.Raw(1);
                var ret = new Impl(new StaticModInt<T>[] { one });
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
                StaticModInt<T>[] array;
                int f, t;
                public SimpleDeque(int size)
                {
                    array = new StaticModInt<T>[size * 2];
                    f = t = size;
                }
                public int Length => t - f;
                [凾(256)] public void Resize(int size) => t = f + size;
                [凾(256)] public void AddFirst(StaticModInt<T> v) => array[--f] = v;
                [凾(256)] public void AddLast(StaticModInt<T> v) => array[t++] = v;
                [凾(256)] public void PopFirst() => ++f;
                [凾(256)] public void PopLast() => --t;
                [凾(256)] public Span<StaticModInt<T>> AsSpan() => array.AsSpan(f, Length);
            }
            ref struct ExpNttInv
            {
                int Length;
                readonly StaticModInt<T>[] array;
                public ExpNttInv(int s)
                {
                    array = new StaticModInt<T>[s];
                    Length = 0;
                }
                [凾(256)] public void Add(StaticModInt<T> v) => array[Length++] = v;
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

                public void InplaceDerivative(SimpleDeque f)
                {
                    if (f.Length > 0)
                    {
                        f.PopFirst();
                        var fs = f.AsSpan();
                        for (int i = 0; i < fs.Length; i++)
                            fs[i] *= StaticModInt<T>.Raw(i + 1);
                    }
                }
            }

            [凾(256)]
            public Impl ExpNtt(int deg)
            {
                var inv = new ExpNttInv(3 * deg);
                inv.Add(StaticModInt<T>.Raw(0));
                inv.Add(StaticModInt<T>.Raw(1));

                var b = new SimpleDeque(3 * deg);
                b.AddLast(1);
                b.AddLast(1 < Length ? a[1] : 0);

                var c = new SimpleDeque(deg);
                c.AddLast(StaticModInt<T>.Raw(1));

                Span<StaticModInt<T>> z2 = stackalloc StaticModInt<T>[2];
                z2.Fill(StaticModInt<T>.Raw(1));

                for (int m = 2; m < deg; m *= 2)
                {
                    var y = new StaticModInt<T>[2 * m];
                    b.AsSpan().CopyTo(y);

                    NumberTheoreticTransform<T>.Ntt(y);

                    var z1 = z2;
                    var z = new StaticModInt<T>[m];
                    for (int i = 0; i < z.Length; i++) z[i] = y[i] * z1[i];
                    NumberTheoreticTransform<T>.INtt(z);
                    z.AsSpan(0, m / 2).Clear();
                    NumberTheoreticTransform<T>.Ntt(z);
                    for (int i = 0; i < z.Length; i++) z[i] *= -z1[i];
                    NumberTheoreticTransform<T>.INtt(z);

                    for (int i = m / 2; i < z.Length; i++) c.AddLast(z[i]);

                    z2 = new StaticModInt<T>[2 * m];
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

                var span = AsSpan();
                for (int i = 0; i < span.Length; i++)
                    if (span[i].Value != 0)
                    {
                        if (i * k > deg)
                            return Set(new StaticModInt<T>[deg]);
                        var rev = span[i].Inv();
                        var right = span[i].Pow(k);

                        return Multiply(rev).RightShift(i).Log(deg).Multiply(k).Exp(deg).Multiply(right)
                            .LeftShift((int)(i * k)).Pre(deg);
                    }
                return Set(new StaticModInt<T>[deg]);
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