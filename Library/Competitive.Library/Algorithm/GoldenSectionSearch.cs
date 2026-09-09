using AtCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class GoldenSectionSearch
    {
        /// <summary>
        /// [<paramref name="lo"/>, <paramref name="hi"/>] で極小値をとる x を求めます。
        /// </summary>
        [凾(256)]
        public static double ExtremumDouble<Ty, TOp>(double lo, double hi, double eps = 1e-8)
            where Ty : IComparable<Ty>
            where TOp : struct, IGoldenSectionSearchFunction<double, Ty>, allows ref struct
            => ExtremumDouble<Ty, TOp>(new TOp(), lo, hi, eps);
        /// <summary>
        /// [<paramref name="lo"/>, <paramref name="hi"/>] で極小値をとる x を求めます。
        /// </summary>
        [凾(256)]
        public static double ExtremumDouble<TOp>(double lo, double hi, double eps = 1e-8)
            where TOp : struct, IGoldenSectionSearchFunction<double, double>, allows ref struct
            => ExtremumDouble<double, TOp>(new TOp(), lo, hi, eps);

        /// <summary>
        /// [<paramref name="lo"/>, <paramref name="hi"/>] で極小値をとる x を求めます。
        /// </summary>
        [凾(256)]
        public static double ExtremumDouble<TOp>(this TOp op, double lo, double hi, double eps = 1e-8)
            where TOp : IGoldenSectionSearchFunction<double, double>, allows ref struct
            => ExtremumDouble<double, TOp>(op, lo, hi, eps);
        /// <summary>
        /// [<paramref name="lo"/>, <paramref name="hi"/>] で極小値をとる x を求めます。
        /// </summary>
        public static double ExtremumDouble<Ty, TOp>(this TOp op, double lo, double hi, double eps = 1e-8)
            where Ty : IComparable<Ty>
            where TOp : IGoldenSectionSearchFunction<double, Ty>, allows ref struct
        {
            const double PHI = 1.61803398874989484; // 黄金比: (1 + √5)/2

            var mL = (lo * PHI + hi) / (1 + PHI);
            var mH = (lo + PHI * hi) / (1 + PHI);

#pragma warning disable IDE0059 // 値の不必要な代入
            var vL = op.Value(lo);
            var vmL = op.Value(mL);
            var vmH = op.Value(mH);
            var vH = op.Value(hi);

            while (hi - lo > eps)
            {
                if (vmL.CompareTo(vmH) < 0)
                {
                    (mL, mH, hi) = ((lo * PHI + mH) / (1 + PHI), mL, mH);
                    (vmL, vmH, vH) = (op.Value(mL), vmL, vmH);
                }
                else
                {
                    (lo, mL, mH) = (mL, mH, (mL + PHI * hi) / (1 + PHI));
                    (vL, vmL, vmH) = (vmL, vmH, op.Value(mH));
                }
            }
#pragma warning restore IDE0059 // 値の不必要な代入
            return mL;
        }

        /// <summary>
        /// [<paramref name="lo"/>, <paramref name="hi"/>] で極小値をとる x を求めます。
        /// </summary>
        [凾(256)]
        public static T Extremum<T, Ty, TOp>(T lo, T hi)
            where T : IBinaryInteger<T>
            where Ty : IComparable<Ty>
            where TOp : struct, IGoldenSectionSearchFunction<T, Ty>, allows ref struct
            => Extremum<T, Ty, TOp>(new TOp(), lo, hi);

        static ReadOnlySpan<uint> Fib32 => [1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584,
            4181, 6765, 10946, 17711, 28657, 46368, 75025, 121393, 196418, 317811, 514229, 832040, 1346269, 2178309, 3524578,
            5702887, 9227465, 14930352, 24157817, 39088169, 63245986, 102334155, 165580141, 267914296, 433494437, 701408733, 1134903170,
            1836311903, 2971215073];
        static ReadOnlySpan<ulong> Fib64 => [1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584,
            4181, 6765, 10946, 17711, 28657, 46368, 75025, 121393, 196418, 317811, 514229, 832040, 1346269, 2178309, 3524578,
            5702887, 9227465, 14930352, 24157817, 39088169, 63245986, 102334155, 165580141, 267914296, 433494437, 701408733, 1134903170,
            1836311903, 2971215073, 4807526976, 7778742049, 12586269025, 20365011074, 32951280099, 53316291173, 86267571272,
            139583862445, 225851433717, 365435296162, 591286729879, 956722026041, 1548008755920, 2504730781961, 4052739537881,
            6557470319842, 10610209857723, 17167680177565, 27777890035288, 44945570212853, 72723460248141, 117669030460994,
            190392490709135, 308061521170129, 498454011879264, 806515533049393, 1304969544928657, 2111485077978050, 3416454622906707,
            5527939700884757, 8944394323791464, 14472334024676221, 23416728348467685, 37889062373143906, 61305790721611591, 99194853094755497,
            160500643816367088, 259695496911122585, 420196140727489673, 679891637638612258, 1100087778366101931, 1779979416004714189,
            2880067194370816120, 4660046610375530309, 7540113804746346429, 12200160415121876738];

        static ReadOnlySpan<T> GetFib<T>(T max) where T : IBinaryInteger<T>
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(uint))
            {
                var m32 = uint.CreateTruncating(max);
                var f = Fib32;
                return Unsafe.BitCast<ReadOnlySpan<uint>, ReadOnlySpan<T>>(f[..(f.LastIndexOfAnyInRange(0u, m32) + 1)]);
            }
            if (typeof(T) == typeof(long) || typeof(T) == typeof(ulong))
            {
                var m64 = ulong.CreateTruncating(max);
                var f = Fib64;
                return Unsafe.BitCast<ReadOnlySpan<ulong>, ReadOnlySpan<T>>(f[..(f.LastIndexOfAnyInRange(0u, m64) + 1)]);
            }

            var ls = new List<T> { T.One, T.One };
            while (ls[^1] < max)
                ls.Add(ls[^1] + ls[^2]);
            return ls.ToArray();
        }

        /// <summary>
        /// [<paramref name="lo"/>, <paramref name="hi"/>] で極小値をとる x を求めます。
        /// </summary>
        public static T Extremum<T, Ty, TOp>(this TOp op, T lo, T hi)
            where T : IBinaryInteger<T>
            where Ty : IComparable<Ty>
            where TOp : IGoldenSectionSearchFunction<T, Ty>, allows ref struct
        {
            if (lo + T.One + T.One >= hi)
            {
                if (lo == hi) return lo;
                if (lo + T.One == hi) return op.Value(lo).CompareTo(op.Value(hi)) <= 0 ? lo : hi;
                Debug.Assert(lo + T.One + T.One == hi);
                return lo + T.One;
            }

            ReadOnlySpan<T> fib = GetFib(hi - lo);
            var mL = lo + fib[^3];
            var mH = hi - fib[^3];

#pragma warning disable IDE0059 // 値の不必要な代入
            var vL = op.Value(lo);
            var vmL = op.Value(mL);
            var vmH = op.Value(mH);
            var vH = op.Value(hi);

            if (hi != lo + fib[^1])
            {
                Debug.Assert(hi > lo + fib[^1]);
                if (vmL.CompareTo(vmH) < 0)
                {
                    mH = lo + fib[^2];
                    hi = lo + fib[^1];
                    vmH = op.Value(mH);
                    vH = op.Value(hi);
                }
                else
                {
                    lo = hi - fib[^1];
                    mL = hi - fib[^2];
                    vL = op.Value(lo);
                    vmL = op.Value(mL);
                }
            }

            for (int i = fib.Length - 4; i >= 0; i--)
            {
                if (vmL.CompareTo(vmH) < 0)
                {
                    (mL, mH, hi) = (lo + fib[i], mL, mH);
                    (vmL, vmH, vH) = (op.Value(mL), vmL, vmH);
                }
                else
                {
                    (lo, mL, mH) = (mL, mH, hi - fib[i]);
                    (vL, vmL, vmH) = (vmL, vmH, op.Value(mH));
                }
            }
#pragma warning restore IDE0059 // 値の不必要な代入
            return mL;
        }

        /// <summary>
        /// 極小値/極大値をとるインデックスを求めます。<paramref name="max"/> が <see langword="true"/> なら極大点、<see langword="false"/> なら極小点を求めます。
        /// </summary>
        [凾(256)]
        public static int Extremum<T>(ReadOnlySpan<T> span, bool max) where T : IComparable<T>, INumberBase<T>
            => max
            ? Extremum<int, T, RevOp<T>>(new(span), 0, span.Length - 1)
            : Extremum<int, T, NormalOp<T>>(new(span), 0, span.Length - 1);
        readonly ref struct NormalOp<T>(ReadOnlySpan<T> Span) : IGoldenSectionSearchFunction<int, T> where T : IComparable<T>
        {
            readonly ReadOnlySpan<T> Span = Span;
            [凾(256)] public T Value(int v) => Span[v];
        }
        readonly ref struct RevOp<T>(ReadOnlySpan<T> Span) : IGoldenSectionSearchFunction<int, T> where T : IComparable<T>, INumberBase<T>
        {
            readonly ReadOnlySpan<T> Span = Span;
            [凾(256)] public T Value(int v) => -Span[v];
        }
    }
    [IsOperator]
    public interface IGoldenSectionSearchFunction<in Tx, out Ty>
    {
        Ty Value(Tx v);
    }
}
