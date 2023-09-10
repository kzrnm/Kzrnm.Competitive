using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using static Avx2;
    using static Sse2;
    public static class SimdMontgomery
    {
        [凾(256)] public static Vector128<uint> MultiplyLow(Vector128<uint> a, Vector128<uint> b) => Sse41.MultiplyLow(a, b);
        [凾(256)]
        public static Vector128<uint> MultiplyHigh(Vector128<uint> a, Vector128<uint> b)
        {
            var a13 = Shuffle(a, 0xF5);
            var b13 = Shuffle(b, 0xF5);
            var prod02 = Multiply(a, b).AsUInt32();
            var prod13 = Multiply(a13, b13).AsUInt32();
            var prod = UnpackHigh(
                UnpackLow(prod02, prod13).AsUInt64(),
                UnpackHigh(prod02, prod13).AsUInt64());
            return prod.AsUInt32();
        }
        [凾(256)]
        public static Vector128<uint> MontgomeryMultiply(Vector128<uint> a, Vector128<uint> b, Vector128<uint> r, Vector128<uint> m1)
        {
            return Subtract(
                Add(MultiplyHigh(a, b), m1),
                MultiplyHigh(MultiplyLow(MultiplyLow(a, b), r), m1));
        }
        [凾(256)]
        public static Vector128<uint> MontgomeryAdd(Vector128<uint> a, Vector128<uint> b, Vector128<uint> m2, Vector128<uint> m0)
        {
            var ret = Subtract(Add(a, b), m2);
            return Add(And(CompareGreaterThan(m0.AsInt32(), ret.AsInt32()).AsUInt32(), m2), ret);
        }
        [凾(256)]
        public static Vector128<uint> MontgomerySubtract(Vector128<uint> a, Vector128<uint> b, Vector128<uint> m2, Vector128<uint> m0)
        {
            var ret = Subtract(a, b);
            return Add(And(CompareGreaterThan(m0.AsInt32(), ret.AsInt32()).AsUInt32(), m2), ret);
        }


        [凾(256)] public static Vector256<uint> MultiplyLow(Vector256<uint> a, Vector256<uint> b) => Avx2.MultiplyLow(a, b);
        [凾(256)]
        public static Vector256<uint> MultiplyHigh(Vector256<uint> a, Vector256<uint> b)
        {
            var a13 = Shuffle(a, 0xF5);
            var b13 = Shuffle(b, 0xF5);
            var prod02 = Multiply(a, b).AsUInt32();
            var prod13 = Multiply(a13, b13).AsUInt32();
            var prod = UnpackHigh(
                UnpackLow(prod02, prod13).AsUInt64(),
                UnpackHigh(prod02, prod13).AsUInt64());
            return prod.AsUInt32();
        }
        [凾(256)]
        public static Vector256<uint> MontgomeryMultiply(Vector256<uint> a, Vector256<uint> b, Vector256<uint> r, Vector256<uint> m1)
        {
            return Subtract(
                Add(MultiplyHigh(a, b), m1),
                MultiplyHigh(MultiplyLow(MultiplyLow(a, b), r), m1));
        }
        [凾(256)]
        public static Vector256<uint> MontgomeryAdd(Vector256<uint> a, Vector256<uint> b, Vector256<uint> m2, Vector256<uint> m0)
        {
            var ret = Subtract(Add(a, b), m2);
            return Add(And(CompareGreaterThan(m0.AsInt32(), ret.AsInt32()).AsUInt32(), m2), ret);
        }
        [凾(256)]
        public static Vector256<uint> MontgomerySubtract(Vector256<uint> a, Vector256<uint> b, Vector256<uint> m2, Vector256<uint> m0)
        {
            var ret = Subtract(a, b);
            return Add(And(CompareGreaterThan(m0.AsInt32(), ret.AsInt32()).AsUInt32(), m2), ret);
        }
    }
}
