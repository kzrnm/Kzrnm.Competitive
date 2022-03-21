using AtCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 多項式
    /// </summary>
    public class PolynomialModInt<T>
        where T : struct, IStaticMod
    {
        private static readonly StaticModIntOperator<T> op = default;
        /// <summary>
        /// 多項式の係数
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public readonly StaticModInt<T>[] Coefficients;
        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        /// <param name="polynomial"><paramref name="polynomial"/>[i] がi次の係数となる多項式</param>
        public PolynomialModInt(StaticModInt<T>[] polynomial)
        {
            var span = polynomial.AsSpan();
            while (span.Length > 1 && EqualityComparer<StaticModInt<T>>.Default.Equals(span[^1], default))
                span = span[..^1];
            Coefficients = span.ToArray();
        }

        [凾(256)]
        public static PolynomialModInt<T> operator +(PolynomialModInt<T> lhs, PolynomialModInt<T> rhs)
        {
            var ll = lhs.Coefficients.Length;
            var rl = rhs.Coefficients.Length;
            ref var lp = ref lhs.Coefficients[0];
            ref var rp = ref rhs.Coefficients[0];
            var arr = new StaticModInt<T>[Math.Max(ll, rl)];
            for (int i = 0; i < arr.Length; i++)
            {
                var lv = i < ll ? Unsafe.Add(ref lp, i) : default;
                var rv = i < rl ? Unsafe.Add(ref rp, i) : default;
                arr[i] = op.Add(lv, rv);
            }
            return new PolynomialModInt<T>(arr);
        }
        [凾(256)]
        public static PolynomialModInt<T> operator -(PolynomialModInt<T> lhs, PolynomialModInt<T> rhs)
        {
            var ll = lhs.Coefficients.Length;
            var rl = rhs.Coefficients.Length;
            ref var lp = ref lhs.Coefficients[0];
            ref var rp = ref rhs.Coefficients[0];
            var arr = new StaticModInt<T>[Math.Max(ll, rl)];
            for (int i = 0; i < arr.Length; i++)
            {
                var lv = i < ll ? Unsafe.Add(ref lp, i) : default;
                var rv = i < rl ? Unsafe.Add(ref rp, i) : default;
                arr[i] = op.Subtract(lv, rv);
            }
            return new PolynomialModInt<T>(arr);
        }
        [凾(256)]
        public static PolynomialModInt<T> operator -(PolynomialModInt<T> p)
        {
            var arr = p.Coefficients.Select(op.Minus).ToArray();
            return new PolynomialModInt<T>(arr);
        }
        [凾(256)]
        public static PolynomialModInt<T> operator *(PolynomialModInt<T> lhs, PolynomialModInt<T> rhs)
        {
            var arr = ConvolutionAnyMod.Convolution<T>(lhs.Coefficients, rhs.Coefficients);
            return new PolynomialModInt<T>(arr);
        }
        [凾(256)]
        public static PolynomialModInt<T> operator *(StaticModInt<T> lhs, PolynomialModInt<T> rhs)
        {
            var rp = rhs.Coefficients;
            var arr = new StaticModInt<T>[rp.Length];
            for (int r = 0; r < rp.Length; r++)
                arr[r] = op.Multiply(lhs, rp[r]);

            return new PolynomialModInt<T>(arr);
        }

        [凾(256)]
        public static PolynomialModInt<T> operator /(PolynomialModInt<T> lhs, PolynomialModInt<T> rhs)
                => lhs.DivRem(rhs).Quotient;


        /// <summary>
        /// 多項式の割り算
        /// </summary>
        [凾(256)]
        public (PolynomialModInt<T> Quotient, PolynomialModInt<T> Remainder) DivRem(PolynomialModInt<T> rhs)
        {
            var lp = (StaticModInt<T>[])Coefficients.Clone();
            ReadOnlySpan<StaticModInt<T>> rp = rhs.Coefficients;

            if (lp.Length < rp.Length)
                return (new PolynomialModInt<T>(Array.Empty<StaticModInt<T>>()), this);

            var res = new StaticModInt<T>[lp.Length - rp.Length + 1];

            for (int i = res.Length - 1; i >= 0; i--)
            {
                var current = lp.AsSpan(i, rp.Length);
                res[i] = op.Divide(current[^1], rp[^1]);
                for (int j = 0; j < current.Length; j++)
                {
                    current[j] = op.Subtract(current[j], op.Multiply(res[i], rp[j]));
                }
            }

            var remainder = new PolynomialModInt<T>(lp.AsSpan(0, rp.Length - 1).ToArray());
            return (new PolynomialModInt<T>(res), remainder);
        }

        /// <summary>
        /// 微分した多項式を返します。
        /// </summary>
        /// <returns></returns>
        [凾(256)]
        public PolynomialModInt<T> Derivative()
        {
            var arr = new StaticModInt<T>[Coefficients.Length - 1];
            StaticModInt<T> a = default;
            for (int i = 0; i < arr.Length; i++)
                arr[i] = op.Multiply(a = op.Increment(a), Coefficients[i + 1]);

            return new PolynomialModInt<T>(arr);
        }
        /// <summary>
        /// 積分した多項式を返します。
        /// </summary>
        /// <returns></returns>
        [凾(256)]
        public PolynomialModInt<T> Integrate()
        {
            var arr = new StaticModInt<T>[Coefficients.Length + 1];
            StaticModInt<T> a = default;
            for (int i = 1; i < arr.Length; i++)
                arr[i] = op.Divide(Coefficients[i - 1], a = op.Increment(a));

            return new PolynomialModInt<T>(arr);
        }

        /// <summary>
        /// 多項式に <paramref name="x"/> を代入した値を返します。
        /// </summary>
        [凾(256)]
        public StaticModInt<T> Calc(StaticModInt<T> x)
        {
            StaticModInt<T> x_n = x;
            StaticModInt<T> res = Coefficients[0];
            foreach (var a in Coefficients.AsSpan(1))
            {
                res = op.Add(res, op.Multiply(a, x_n));
                x_n = op.Multiply(x_n, x);
            }
            return res;
        }

        /// <summary>
        /// <para>ラグランジュ補間</para>
        /// <para>(x0, y0), ..., (xn, yn)を満たす n 次多項式を返します。</para>
        /// <para>制約: xは全て異なる</para>
        /// </summary>
        [凾(256)]
        public static PolynomialModInt<T> LagrangeInterpolation((StaticModInt<T> x, StaticModInt<T> y)[] plots) => LagrangeInterpolation((ReadOnlySpan<(StaticModInt<T>, StaticModInt<T>)>)plots);

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
        public static PolynomialModInt<T> LagrangeInterpolation(ReadOnlySpan<(StaticModInt<T> x, StaticModInt<T> y)> plots)
        {
            // y = ∑ (y_i * (Π_k!=i (x - x_k)) / (Π_k!=i (x_i - x_k)) )
            var res = new PolynomialModInt<T>(new StaticModInt<T>[plots.Length]);
            var pall = PAll(plots);
            for (int i = 0; i < plots.Length; i++)
                res += ConstantCoefficient(plots, i) * Pk(plots, i, pall);
            return res;
        }
        // y_i / (Π_k!=i (x_i - x_k)) )
        [凾(256)]
        static StaticModInt<T> ConstantCoefficient(ReadOnlySpan<(StaticModInt<T> x, StaticModInt<T> y)> plots, int i)
        {
            var (xi, yi) = plots[i];
            var d = op.MultiplyIdentity;
            for (int k = 0; k < plots.Length; k++)
                if (i != k)
                    d = op.Multiply(d, op.Subtract(xi, plots[k].x));
            return op.Divide(yi, d);
        }

        // Π_k (x - x_k) となる n+1 次多項式の係数
        [凾(256)]
        static StaticModInt<T>[] PAll(ReadOnlySpan<(StaticModInt<T> x, StaticModInt<T> y)> plots)
        {
            var res = new StaticModInt<T>[plots.Length + 1];
            res[^1] = op.MultiplyIdentity;
            for (int k = plots.Length - 1; k >= 0; k--)
            {
                var xk = op.Minus(plots[k].x);
                for (int j = k; j + 1 < res.Length; j++)
                    res[j] = op.Add(res[j], op.Multiply(xk, res[j + 1]));
            }
            return res;
        }

        // Π_k!=i (x - x_k)
        [凾(256)]
        static PolynomialModInt<T> Pk(ReadOnlySpan<(StaticModInt<T> x, StaticModInt<T> y)> plots, int i, StaticModInt<T>[] pall)
        {
            var xi = plots[i].x;
            var res = new StaticModInt<T>[plots.Length];
            res[^1] = op.MultiplyIdentity;
            for (int j = plots.Length - 2; j >= 0; j--)
                res[j] = op.Add(op.Multiply(xi, res[j + 1]), pall[j + 1]);
            return new PolynomialModInt<T>(res);
        }
    }
}
