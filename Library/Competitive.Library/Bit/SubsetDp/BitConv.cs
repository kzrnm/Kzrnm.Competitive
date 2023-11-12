using AtCoder;
using System;
using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    [IsOperator]
    public interface IBitConvOp<T>
    {
        void Transform(Span<T> a);
        void Inverse(Span<T> a);
    }

    /// <summary>
    /// ビット集合での畳み込み
    /// </summary>
    public static class BitConv
    {
        /// <summary>
        /// c[k] = ∑ <paramref name="a"/>[i] <paramref name="b"/>[j] となる c を返します。
        /// </summary>
        [凾(256)]
        public static T[] Conv<T, TOp>(ReadOnlySpan<T> a, ReadOnlySpan<T> b, TOp op = default)
            where T : IMultiplyOperators<T, T, T>
            where TOp : IBitConvOp<T>
        {
            if (a.Length != b.Length) ThrowLengthDiff();
            if (!BitOperations.IsPow2(a.Length)) ThrowNotPow2(nameof(a));
            if (!BitOperations.IsPow2(b.Length)) ThrowNotPow2(nameof(b));

            if (a.Length == 0) return Array.Empty<T>();

            var f = new T[a.Length];
            {
                var gp = ArrayPool<T>.Shared.Rent(b.Length);
                ref var g = ref gp[0];

                a.CopyTo(f);
                b.CopyTo(gp);

                op.Transform(f);
                op.Transform(gp.AsSpan(0, b.Length));

                for (int i = 0; i < f.Length; i++)
                    f[i] *= Unsafe.Add(ref g, i);
                ArrayPool<T>.Shared.Return(gp);
            }

            op.Inverse(f);

            return f;

            static void ThrowLengthDiff() => throw new ArgumentException("All length of inputs must be same.", "b");
            static void ThrowNotPow2(string n) => throw new ArgumentException("A length of input must be power of 2.", n);
        }
    }
}
