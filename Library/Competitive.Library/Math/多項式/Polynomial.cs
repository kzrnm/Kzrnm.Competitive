using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 多項式
    /// </summary>
    public class Polynomial<T> where T : INumberBase<T>
    {
        /// <summary>
        /// 多項式の係数
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public readonly T[] Coefficients;
        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        /// <param name="polynomial"><paramref name="polynomial"/>[i] がi次の係数となる多項式</param>
        public Polynomial(ReadOnlySpan<T> polynomial)
        {
            var span = polynomial;
            while (span.Length > 1 && EqualityComparer<T>.Default.Equals(span[^1], default))
                span = span[..^1];
            Coefficients = span.ToArray();
        }

        [凾(256)] public static implicit operator Polynomial<T>(T v) => new(new[] { v });

        public override string ToString() => string.Join(", ", Coefficients);

        [凾(256)]
        public static Polynomial<T> operator +(Polynomial<T> lhs, Polynomial<T> rhs)
        {
            var ll = lhs.Coefficients.Length;
            var rl = rhs.Coefficients.Length;
            if (ll == 0) return rhs;
            if (rl == 0) return lhs;
            ref var lp = ref lhs.Coefficients[0];
            ref var rp = ref rhs.Coefficients[0];
            var arr = new T[Math.Max(ll, rl)];
            for (int i = 0; i < arr.Length; i++)
            {
                var lv = i < ll ? Unsafe.Add(ref lp, i) : default;
                var rv = i < rl ? Unsafe.Add(ref rp, i) : default;
                arr[i] = lv + rv;
            }
            return new Polynomial<T>(arr);
        }
        [凾(256)]
        public static Polynomial<T> operator -(Polynomial<T> lhs, Polynomial<T> rhs)
        {
            var ll = lhs.Coefficients.Length;
            var rl = rhs.Coefficients.Length;
            if (ll == 0) return -rhs;
            if (rl == 0) return lhs;
            ref var lp = ref lhs.Coefficients[0];
            ref var rp = ref rhs.Coefficients[0];
            var arr = new T[Math.Max(ll, rl)];
            for (int i = 0; i < arr.Length; i++)
            {
                var lv = i < ll ? Unsafe.Add(ref lp, i) : default;
                var rv = i < rl ? Unsafe.Add(ref rp, i) : default;
                arr[i] = lv - rv;
            }
            return new Polynomial<T>(arr);
        }
        [凾(256)]
        public static Polynomial<T> operator -(Polynomial<T> p)
        {
            var arr = p.Coefficients.Select(v => -v).ToArray();
            return new Polynomial<T>(arr);
        }
        [凾(256)]
        public static Polynomial<T> operator *(Polynomial<T> lhs, Polynomial<T> rhs)
        {
            var lp = lhs.Coefficients;
            var rp = rhs.Coefficients;
            var arr = new T[lp.Length + rp.Length - 1];

            for (int l = 0; l < lp.Length; l++)
                for (int r = 0; r < rp.Length; r++)
                    arr[l + r] += lp[l] * rp[r];

            return new Polynomial<T>(arr);
        }
        [凾(256)]
        public static Polynomial<T> operator *(T lhs, Polynomial<T> rhs)
        {
            var rp = rhs.Coefficients;
            var arr = new T[rp.Length];
            for (int r = 0; r < rp.Length; r++)
                arr[r] = lhs * rp[r];

            return new Polynomial<T>(arr);
        }

        [凾(256)]
        public static Polynomial<T> operator /(Polynomial<T> lhs, Polynomial<T> rhs)
                => lhs.DivRem(rhs).Quotient;
        [凾(256)]
        public static Polynomial<T> operator %(Polynomial<T> lhs, Polynomial<T> rhs)
                => lhs.DivRem(rhs).Remainder;

        /// <summary>
        /// 多項式の割り算
        /// </summary>
        [凾(256)]
        public (Polynomial<T> Quotient, Polynomial<T> Remainder) DivRem(Polynomial<T> rhs)
        {
            if (Coefficients.Length < rhs.Coefficients.Length)
                return (new Polynomial<T>(Array.Empty<T>()), this);

            var lp = (T[])Coefficients.Clone();
            var rp = rhs.Coefficients;

            var res = new T[lp.Length - rp.Length + 1];
            for (int i = res.Length - 1; i >= 0; i--)
            {
                var current = lp.AsSpan(i, rp.Length);
                res[i] = current[^1] / rp[^1];
                for (int j = 0; j < current.Length; j++)
                    current[j] -= res[i] * rp[j];
            }

            var remainder = new Polynomial<T>(lp.AsSpan(0, rp.Length - 1).ToArray());
            return (new Polynomial<T>(res), remainder);
        }

        /// <summary>
        /// 微分した多項式を返します。
        /// </summary>
        /// <returns></returns>
        [凾(256)]
        public Polynomial<T> Derivative()
        {
            var arr = new T[Coefficients.Length - 1];
            T a = default;
            for (int i = 0; i < arr.Length; i++)
                arr[i] = ++a * Coefficients[i + 1];

            return new Polynomial<T>(arr);
        }
        /// <summary>
        /// 積分した多項式を返します。
        /// </summary>
        /// <returns></returns>
        [凾(256)]
        public Polynomial<T> Integrate()
        {
            var arr = new T[Coefficients.Length + 1];
            T a = default;
            for (int i = 1; i < arr.Length; i++)
                arr[i] = Coefficients[i - 1] / ++a;

            return new Polynomial<T>(arr);
        }

        /// <summary>
        /// 多項式に <paramref name="x"/> を代入した値を返します。
        /// </summary>
        [凾(256)]
        public T Eval(T x)
        {
            T x_n = x;
            T res = Coefficients[0];
            foreach (var a in Coefficients.AsSpan(1))
            {
                res += a * x_n;
                x_n *= x;
            }
            return res;
        }

        /// <summary>
        /// <para>ラグランジュ補間</para>
        /// <para>(x0, y0), ..., (xn, yn)を満たす n 次多項式を返します。</para>
        /// <para>制約: xは全て異なる</para>
        /// </summary>
        [凾(256)]
        public static Polynomial<T> LagrangeInterpolation((T x, T y)[] plots) => LagrangeInterpolation((ReadOnlySpan<(T, T)>)plots);

        /// <summary>
        /// <para>ラグランジュ補間</para>
        /// <para>(x0, y0), ..., (xn, yn)を満たす n 次多項式を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>制約:</para>
        /// <para>- xは全て異なる</para>
        /// <para>計算量: O(n^2)</para>
        /// </remarks>
        [凾(256)]
        public static Polynomial<T> LagrangeInterpolation(ReadOnlySpan<(T x, T y)> plots)
        {
            // y = ∑ (y_i * (Π_k!=i (x - x_k)) / (Π_k!=i (x_i - x_k)) )
            var res = new Polynomial<T>(new T[plots.Length]);
            var pall = PAll(plots);
            for (int i = 0; i < plots.Length; i++)
                res += ConstantCoefficient(plots, i) * Pk(plots, i, pall);
            return res;
        }
        // y_i / (Π_k!=i (x_i - x_k)) )
        [凾(256)]
        static T ConstantCoefficient(ReadOnlySpan<(T x, T y)> plots, int i)
        {
            var (xi, yi) = plots[i];
            var d = T.MultiplicativeIdentity;
            for (int k = 0; k < plots.Length; k++)
                if (i != k)
                    d *= xi - plots[k].x;
            return yi / d;
        }

        // Π_k (x - x_k) となる n+1 次多項式の係数
        [凾(256)]
        static T[] PAll(ReadOnlySpan<(T x, T y)> plots)
        {
            var res = new T[plots.Length + 1];
            res[^1] = T.MultiplicativeIdentity;
            for (int k = plots.Length - 1; k >= 0; k--)
            {
                var xk = -plots[k].x;
                for (int j = k; j + 1 < res.Length; j++)
                    res[j] += xk * res[j + 1];
            }
            return res;
        }

        // Π_k!=i (x - x_k)
        [凾(256)]
        static Polynomial<T> Pk(ReadOnlySpan<(T x, T y)> plots, int i, T[] pall)
        {
            var xi = plots[i].x;
            var res = new T[plots.Length];
            res[^1] = T.MultiplicativeIdentity;
            for (int j = plots.Length - 2; j >= 0; j--)
                res[j] = xi * res[j + 1] + pall[j + 1];
            return new Polynomial<T>(res);
        }
    }
}
