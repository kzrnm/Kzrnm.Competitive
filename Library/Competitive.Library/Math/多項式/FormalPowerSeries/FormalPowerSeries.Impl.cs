using AtCoder;
using AtCoder.Internal;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

// https://nyaannyaan.github.io/library/fps/formal-power-series.hpp
namespace Kzrnm.Competitive
{
    /// <summary>
    /// 多項式/形式的冪級数
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0251:メンバーを 'readonly' にする", Justification = "気にしない")]
    internal ref partial struct FpsImpl<T> where T : struct, IStaticMod
    {
        public MontgomeryModInt<T>[] a;
        public int Length;
        public FpsImpl(FormalPowerSeries<T> f) : this(f._cs) { }
        public FpsImpl(MontgomeryModInt<T>[] c)
        {
            a = c;
            Length = a.Length;
        }
        [凾(256)]
        public FpsImpl<T> Set(MontgomeryModInt<T>[] c)
        {
            a = c;
            Length = c.Length;
            return this;
        }

        [凾(256)]
        public Span<MontgomeryModInt<T>> AsSpan() => a.AsSpan(0, Length);



        [凾(256)]
        public FormalPowerSeries<T> ToFps()
        {
            var s = AsSpan();
            if (s.Length == a.Length)
                return new FormalPowerSeries<T>(a);
            return new FormalPowerSeries<T>(s);
        }
        [凾(256)]
        void Grow(int length)
        {
            Array.Resize(ref a, length);
            Length = length;
        }

        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public FpsImpl<T> Add(ReadOnlySpan<MontgomeryModInt<T>> rhs)
        {
            if (rhs.Length == 0)
                return this;
            if (a.Length < rhs.Length)
                Grow(rhs.Length);
            ref var ap = ref a[0];
            for (int i = 0; i < rhs.Length; i++)
                Unsafe.Add(ref ap, i) += rhs[i];
            return this;
        }
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)] public FpsImpl<T> Add(FpsImpl<T> rhs) => Add(rhs.a);
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public FpsImpl<T> Add(MontgomeryModInt<T> v)
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
        public FpsImpl<T> Subtract(ReadOnlySpan<MontgomeryModInt<T>> rhs)
        {
            if (Length == 0)
                return Set(rhs.ToArray()).Minus();
            if (rhs.Length == 0)
                return this;
            if (a.Length < rhs.Length)
                Grow(rhs.Length);
            ref var ap = ref a[0];
            for (int i = 0; i < rhs.Length; i++)
                Unsafe.Add(ref ap, i) -= rhs[i];
            return this;
        }
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public FpsImpl<T> Subtract(MontgomeryModInt<T> v)
        {
            if (Length == 0)
                Set(new MontgomeryModInt<T>[] { -v });
            else
                a[0] -= v;
            return this;
        }
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)] public FpsImpl<T> Subtract(FpsImpl<T> rhs) => Subtract(rhs.a);

        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public FpsImpl<T> Minus()
        {
            for (int i = Length - 1; i >= 0; i--)
                a[i] = -a[i];
            return this;
        }

        /// <remarks>
        /// <para>計算量: O(N log N)</para>
        /// </remarks>
        [凾(256)]
        public FpsImpl<T> Multiply(MontgomeryModInt<T> m)
        {
            for (int i = Length - 1; i >= 0; i--)
                a[i] *= m;
            return this;
        }
        /// <remarks>
        /// <para>計算量: O(N log N)</para>
        /// </remarks>
        [凾(256)]
        public FpsImpl<T> Multiply(ReadOnlySpan<MontgomeryModInt<T>> rhs)
            => Set(NumberTheoreticTransform.Convolution(AsSpan(), rhs));
        /// <remarks>
        /// <para>計算量: O(N log N)</para>
        /// </remarks>
        [凾(256)] public FpsImpl<T> Multiply(FpsImpl<T> rhs) => Multiply(rhs.a);


        /// <remarks>
        /// <para>計算量: O(N log N)</para>
        /// </remarks>
        [凾(256)]
        public FpsImpl<T> Divide(ReadOnlySpan<MontgomeryModInt<T>> rhs)
        {
            if (Length < rhs.Length)
                return Set(Array.Empty<MontgomeryModInt<T>>());

            if (rhs.Length <= 64)
                return DivideNative(rhs);

            int n = Length - rhs.Length + 1;
            var a = new FpsImpl<T>(AsSpan().ToArray()).Reverse().Pre(n).AsSpan();
            var b = new FpsImpl<T>(rhs.ToArray()).Reverse().Inv(n).AsSpan();

            var m = NumberTheoreticTransform.Convolution(a, b);
            return Set(m).Pre(n).Reverse();
        }

        private FpsImpl<T> DivideNative(ReadOnlySpan<MontgomeryModInt<T>> rhs)
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
            var remainder = new FpsImpl<T>(arr).Subtract(NumberTheoreticTransform.Convolution(quotient, rhs)).AsSpan();
            return (quotient, remainder.ToArray());
        }

        /// <summary>
        /// 次数を <paramref name="sz"/> だけ小さくする。
        /// </summary>
        [凾(256)]
        public FpsImpl<T> RightShift(int sz)
            => sz > 0 ? Set(AsSpan()[sz..].ToArray()) : this;


        /// <summary>
        /// 次数を <paramref name="sz"/> だけ大きくする。
        /// </summary>
        [凾(256)]
        public FpsImpl<T> LeftShift(int sz)
        {
            if (sz == 0)
                return this;
            var arr = new MontgomeryModInt<T>[Length + sz];
            AsSpan().CopyTo(arr.AsSpan(sz));
            return Set(arr);
        }

        [凾(256)]
        public FpsImpl<T> Reverse()
        {
            AsSpan().Reverse();
            return this;
        }
        [凾(256)]
        public FpsImpl<T> Pre(int sz)
        {
            if (sz < Length)
                Length = sz;
            return this;
        }
        [凾(256)]
        public FpsImpl<T> Inv(int deg = -1)
        {
            Contract.Assert(a[0].Value != 0);
            if (deg < 0) deg = Length;
            return deg <= NumberTheoreticTransform<T>.NttLength() ? InvNtt(deg) : InvAnyMod(deg);

        }
        private FpsImpl<T> InvNtt(int deg)
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
        private FpsImpl<T> InvAnyMod(int deg)
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