using System;

namespace AtCoder
{
    /// <summary>
    /// 多項式
    /// </summary>
    public class Polynomial<T, TOp>
        where T : struct
        where TOp : struct, IArithmeticOperator<T>
    {
        private static readonly TOp op = default;
        public readonly T[] Value;
        /// <summary>
        /// 多項式を生成します。
        /// </summary>
        /// <param name="polynomial"><paramref name="polynomial"/>[i] がi次の係数となる多項式</param>
        public Polynomial(T[] polynomial) { this.Value = polynomial; }


        public static Polynomial<T, TOp> operator +(Polynomial<T, TOp> lhs, Polynomial<T, TOp> rhs)
        {
            var lp = lhs.Value;
            var rp = rhs.Value;
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
            var lp = lhs.Value;
            var rp = rhs.Value;
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
            var lp = lhs.Value;
            var rp = rhs.Value;
            var arr = new T[lp.Length + rp.Length - 1];

            for (int l = 0; l < lp.Length; l++)
                for (int r = 0; r < rp.Length; r++)
                    arr[l + r] = op.Add(arr[l + r], op.Multiply(lp[l], rp[r]));

            return new Polynomial<T, TOp>(arr);
        }

        public static Polynomial<T, TOp> operator /(Polynomial<T, TOp> lhs, Polynomial<T, TOp> rhs)
            => lhs.DivRem(rhs, out _);

        public Polynomial<T, TOp> DivRem(Polynomial<T, TOp> rhs, out Polynomial<T, TOp> remainder)
        {
            Span<T> lp = (T[])this.Value.Clone();
            ReadOnlySpan<T> rp = rhs.Value;

            if (lp.Length < rp.Length)
            {
                remainder = new Polynomial<T, TOp>(Array.Empty<T>());
                return this;
            }

            var res = new T[lp.Length - rp.Length + 1];

            for (int i = res.Length - 1; i >= 0; i--)
            {
                var current = lp.Slice(i, rp.Length);
                res[i] = op.Divide(current[^1], rp[^1]);
                for (int j = 0; j < current.Length; j++)
                {
                    current[j] = op.Subtract(current[j], op.Multiply(res[i], rp[j]));
                }
            }

            remainder = new Polynomial<T, TOp>(lp.Slice(0, rp.Length - 1).ToArray());
            return new Polynomial<T, TOp>(res);
        }

        /// <summary>
        /// 微分した多項式を返します。
        /// </summary>
        /// <returns></returns>
        public Polynomial<T, TOp> Derivative()
        {
            var arr = new T[Value.Length - 1];
            T a = default;
            for (int i = 0; i < arr.Length; i++)
                arr[i] = op.Multiply(a = op.Increment(a), Value[i + 1]);

            return new Polynomial<T, TOp>(arr);
        }
        /// <summary>
        /// 積分した多項式を返します。
        /// </summary>
        /// <returns></returns>
        public Polynomial<T, TOp> Integrate()
        {
            var arr = new T[Value.Length + 1];
            T a = default;
            for (int i = 1; i < arr.Length; i++)
                arr[i] = op.Divide(Value[i - 1], a = op.Increment(a));

            return new Polynomial<T, TOp>(arr);
        }

        /// <summary>
        /// 多項式に <paramref name="x"/> を代入した値を返します。
        /// </summary>
        public T Calc(T x)
        {
            T x_n = x;
            T res = Value[0];
            foreach (var a in Value[1..])
            {
                res = op.Add(res, op.Multiply(a, x_n));
                x_n = op.Multiply(x_n, x);
            }
            return res;
        }
    }
}
