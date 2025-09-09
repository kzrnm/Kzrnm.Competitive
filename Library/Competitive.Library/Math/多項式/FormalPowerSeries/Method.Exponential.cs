using AtCoder;
using AtCoder.Internal;
using System;
using System.Diagnostics;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 形式的冪級数の指数関数
    /// </summary>
    public static class __FormalPowerSeries__Exponential
    {
        /// <summary>
        /// exp(f(x)) となる多項式の先頭 N 項。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        /// <param name="f">形式的冪級数</param>
        /// <param name="deg">先頭の <paramref name="deg"/> 項を取得します。負数のときは元の FPS と同じ長さで取得します。</param>
        /// <example>https://judge.yosupo.jp/problem/exp_of_formal_power_series</example>
        [凾(256)]
        public static FormalPowerSeries<T> Exp<T>(this FormalPowerSeries<T> f, int deg = -1) where T : struct, IStaticMod
        {
            var a = f._cs;
            Contract.Assert(a.Length == 0 || a[0].Value == 0);
            if (deg < 0) deg = a.Length;
            if (deg == 0)
            {
                return new FormalPowerSeries<T>(new MontgomeryModInt<T>[] { MontgomeryModInt<T>.One });
            }
            var t = f.ToImpl();
            return (deg <= NumberTheoreticTransform<T>.NttLength() ? t.ExpNtt(deg) : t.ExpAnyMod(deg)).ToFps();
        }

        /// <summary>
        /// exp(f(x)) となる多項式の先頭 N 項。
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        /// <param name="t">形式的冪級数</param>
        /// <param name="deg">先頭の <paramref name="deg"/> 項を取得します。負数のときは元の FPS と同じ長さで取得します。</param>
        /// <example>https://judge.yosupo.jp/problem/exp_of_formal_power_series</example>
        [凾(256)]
        internal static FpsImpl<T> Exp<T>(this FpsImpl<T> t, int deg = -1) where T : struct, IStaticMod
        {
            var a = t.a;
            Contract.Assert(a.Length == 0 || a[0].Value == 0);
            if (deg < 0) deg = a.Length;
            if (deg == 0)
                return t.Set(new MontgomeryModInt<T>[] { MontgomeryModInt<T>.One });
            return deg <= NumberTheoreticTransform<T>.NttLength() ? t.ExpNtt(deg) : t.ExpAnyMod(deg);
        }

        class SimpleDeque<T>
        {
            T[] array;
            int f, t;
            public SimpleDeque(int size)
            {
                array = new T[size * 2];
                f = t = size;
            }
            public int Length => t - f;
            [凾(256)] public void Resize(int size) => t = f + size;
            [凾(256)] public void AddFirst(T v) => array[--f] = v;
            [凾(256)] public void AddLast(T v) => array[t++] = v;
            [凾(256)] public void PopFirst() => ++f;
            [凾(256)] public void PopLast() => --t;
            [凾(256)] public Span<T> AsSpan() => array.AsSpan(f, Length);
        }

        struct ExpNttInv<T> where T : struct, IStaticMod
        {
            int Length;
            readonly MontgomeryModInt<T>[] array;
            public ExpNttInv(int s)
            {
                array = new MontgomeryModInt<T>[s];
                Length = 0;
            }
            [凾(256)] public void Add(MontgomeryModInt<T> v) => array[Length++] = v;
            public void InplaceIntegrate(SimpleDeque<MontgomeryModInt<T>> f)
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
        }

        [凾(256)]
        static FpsImpl<T> ExpAnyMod<T>(this FpsImpl<T> t, int deg) where T : struct, IStaticMod
        {
            var a = t.a;
            var one = MontgomeryModInt<T>.One;
            t.Set(new MontgomeryModInt<T>[] { one });
            for (int i = 1; i < deg; i <<= 1)
            {
                var p = new FpsImpl<T>(a.AsSpan().ToArray());
                var r2 = new FpsImpl<T>(t.AsSpan().ToArray());
                t = t.Multiply(p.Pre(i << 1).Add(one).Subtract(r2.Log(i << 1))).Pre(i << 1);
            }
            return t.Pre(deg);
        }

        [凾(256)]
        static FpsImpl<T> ExpNtt<T>(this FpsImpl<T> t, int deg) where T : struct, IStaticMod
        {
            static SimpleDeque<MontgomeryModInt<T>> GetDeque(int s) => new SimpleDeque<MontgomeryModInt<T>>(s);
            var inv = new ExpNttInv<T>(3 * deg);
            inv.Add(MontgomeryModInt<T>.Zero);
            inv.Add(MontgomeryModInt<T>.One);

            var a = t.a;
            var b = GetDeque(3 * deg);
            b.AddLast(1);
            b.AddLast(1 < a.Length ? a[1] : 0);

            var c = GetDeque(deg);
            c.AddLast(MontgomeryModInt<T>.One);

#if NET8_0_OR_GREATER
            Span<MontgomeryModInt<T>> z2 = [MontgomeryModInt<T>.One, MontgomeryModInt<T>.One];
#else
            Span<MontgomeryModInt<T>> z2 = stackalloc MontgomeryModInt<T>[] { MontgomeryModInt<T>.One, MontgomeryModInt<T>.One };
#endif

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

                var x = GetDeque(3 * m);
                foreach (var v in a.AsSpan(0, Math.Min(a.Length, m)))
                    x.AddLast(v);
                x.Resize(m);

                InplaceDerivative(x);
                x.AddLast(default);
                {
                    var xs = x.AsSpan();
                    NumberTheoreticTransform<T>.Ntt(xs);
                    for (int i = 0; i < m; ++i) xs[i] *= y[i];
                    NumberTheoreticTransform<T>.INtt(xs);

                    var bd = new FpsImpl<T>(b.AsSpan().ToArray()).Derivative().AsSpan();
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
                    for (int i = 0; i < Math.Min(a.Length, xs.Length); ++i) xs[i] += a[i];
                    xs[..m].Clear();

                    NumberTheoreticTransform<T>.Ntt(xs);
                    for (int i = 0; i < xs.Length; ++i) xs[i] *= y[i];
                    NumberTheoreticTransform<T>.INtt(xs);

                    foreach (var v in xs[m..])
                        b.AddLast(v);
                }
            }
            return t.Set(b.AsSpan()[..deg].ToArray());


            static void InplaceDerivative(SimpleDeque<MontgomeryModInt<T>> f)
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
    }
}
