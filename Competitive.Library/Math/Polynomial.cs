using AtCoder;
using System;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// 多項式
    /// </summary>
    public class Polynomial<T, TOp>
        where T : struct
        where TOp : struct, IArithmeticOperator<T>, IUnaryNumOperator<T>
    {
        private static readonly TOp op = default;
        public readonly T[] Coefficients;
        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        /// <param name="polynomial"><paramref name="polynomial"/>[i] がi次の係数となる多項式</param>
        public Polynomial(T[] polynomial) { this.Coefficients = polynomial; }


        public static Polynomial<T, TOp> operator +(Polynomial<T, TOp> lhs, Polynomial<T, TOp> rhs)
        {
            var lp = lhs.Coefficients;
            var rp = rhs.Coefficients;
            var arr = new T[Math.Max(lp.Length, rp.Length)];
            lp.CopyTo(arr.AsSpan());
            for (int i = 0; i < rp.Length; i++)
            {
                arr[i] = op.Add(arr[i], rp[i]);
            }
            return new Polynomial<T, TOp>(arr);
        }
        public static Polynomial<T, TOp> operator -(Polynomial<T, TOp> lhs, Polynomial<T, TOp> rhs)
        {
            var lp = lhs.Coefficients;
            var rp = rhs.Coefficients;
            var arr = new T[Math.Max(lp.Length, rp.Length)];
            lp.CopyTo(arr.AsSpan());
            for (int i = 0; i < rp.Length; i++)
            {
                arr[i] = op.Subtract(arr[i], rp[i]);
            }
            return new Polynomial<T, TOp>(arr);
        }
        public static Polynomial<T, TOp> operator *(Polynomial<T, TOp> lhs, Polynomial<T, TOp> rhs)
        {
            var lp = lhs.Coefficients;
            var rp = rhs.Coefficients;
            var arr = new T[lp.Length + rp.Length - 1];

            for (int l = 0; l < lp.Length; l++)
                for (int r = 0; r < rp.Length; r++)
                    arr[l + r] = op.Add(arr[l + r], op.Multiply(lp[l], rp[r]));

            return new Polynomial<T, TOp>(arr);
        }
        public static Polynomial<T, TOp> operator *(T lhs, Polynomial<T, TOp> rhs)
        {
            var rp = rhs.Coefficients;
            var arr = new T[rp.Length];
            for (int r = 0; r < rp.Length; r++)
                arr[r] = op.Multiply(lhs, rp[r]);

            return new Polynomial<T, TOp>(arr);
        }

        public static Polynomial<T, TOp> operator /(Polynomial<T, TOp> lhs, Polynomial<T, TOp> rhs)
            => lhs.DivRem(rhs, out _);

        public Polynomial<T, TOp> DivRem(Polynomial<T, TOp> rhs, out Polynomial<T, TOp> remainder)
        {
            var lp = (T[])this.Coefficients.Clone();
            ReadOnlySpan<T> rp = rhs.Coefficients;

            if (lp.Length < rp.Length)
            {
                remainder = new Polynomial<T, TOp>(Array.Empty<T>());
                return this;
            }

            var res = new T[lp.Length - rp.Length + 1];

            for (int i = res.Length - 1; i >= 0; i--)
            {
                var current = lp.AsSpan(i, rp.Length);
                res[i] = op.Divide(current[^1], rp[^1]);
                for (int j = 0; j < current.Length; j++)
                {
                    current[j] = op.Subtract(current[j], op.Multiply(res[i], rp[j]));
                }
            }

            remainder = new Polynomial<T, TOp>(lp.AsSpan(0, rp.Length - 1).ToArray());
            return new Polynomial<T, TOp>(res);
        }

        /// <summary>
        /// 微分した多項式を返します。
        /// </summary>
        /// <returns></returns>
        public Polynomial<T, TOp> Derivative()
        {
            var arr = new T[Coefficients.Length - 1];
            T a = default;
            for (int i = 0; i < arr.Length; i++)
                arr[i] = op.Multiply(a = op.Increment(a), Coefficients[i + 1]);

            return new Polynomial<T, TOp>(arr);
        }
        /// <summary>
        /// 積分した多項式を返します。
        /// </summary>
        /// <returns></returns>
        public Polynomial<T, TOp> Integrate()
        {
            var arr = new T[Coefficients.Length + 1];
            T a = default;
            for (int i = 1; i < arr.Length; i++)
                arr[i] = op.Divide(Coefficients[i - 1], a = op.Increment(a));

            return new Polynomial<T, TOp>(arr);
        }

        /// <summary>
        /// 多項式に <paramref name="x"/> を代入した値を返します。
        /// </summary>
        public T Calc(T x)
        {
            T x_n = x;
            T res = Coefficients[0];
            foreach (var a in Coefficients[1..])
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
        public static Polynomial<T, TOp> LagrangeInterpolation((T x, T y)[] plots) => LagrangeInterpolation((ReadOnlySpan<(T, T)>)plots);

        /// <summary>
        /// <para>ラグランジュ補間</para>
        /// <para>(x0, y0), ..., (xn, yn)を満たす n 次多項式を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>制約:</para>
        /// <para>- xは全て異なる</para>
        /// <para>計算量: O(n^2)</para>
        /// </remarks>
        public static Polynomial<T, TOp> LagrangeInterpolation(ReadOnlySpan<(T x, T y)> plots)
        {
            // y_i / (Π_k!=i (x_i - x_k)) )
            static T ConstantCoefficient(ReadOnlySpan<(T x, T y)> plots, int i)
            {
                var (xi, yi) = plots[i];
                var d = op.MultiplyIdentity;
                for (int k = 0; k < plots.Length; k++)
                    if (i != k)
                        d = op.Multiply(d, op.Subtract(xi, plots[k].x));
                return op.Divide(yi, d);
            }

            // Π_k (x - x_k) となる n+1 次多項式の係数
            static T[] PAll(ReadOnlySpan<(T x, T y)> plots)
            {
                var res = new T[plots.Length + 1];
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
            static Polynomial<T, TOp> Pk(ReadOnlySpan<(T x, T y)> plots, int i, T[] pall)
            {
                var xi = plots[i].x;
                var res = new T[plots.Length];
                res[^1] = op.MultiplyIdentity;
                for (int j = plots.Length - 2; j >= 0; j--)
                    res[j] = op.Add(op.Multiply(xi, res[j + 1]), pall[j + 1]);
                return new Polynomial<T, TOp>(res);
            }

            // y = ∑ (y_i * (Π_k!=i (x - x_k)) / (Π_k!=i (x_i - x_k)) )
            var res = new Polynomial<T, TOp>(new T[plots.Length]);
            var pall = PAll(plots);
            for (int i = 0; i < plots.Length; i++)
                res += ConstantCoefficient(plots, i) * Pk(plots, i, pall);
            return res;
        }
    }
}
