using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    using static Avx2;
    using static Sse2;
    public static class SimdMontgomery
    {
        [凾(256)]
        public static Vector128<uint> Reduce(Vector128<uint> prod02, Vector128<uint> prod13, Vector128<uint> r, Vector128<uint> m1)
        {
            var unpalo = UnpackLow(prod02, prod13);
            var unpahi = UnpackHigh(prod02, prod13);
            var prodlo = UnpackLow(unpalo.AsUInt64(), unpahi.AsUInt64()).AsUInt32();
            var prodhi = UnpackHigh(unpalo.AsUInt64(), unpahi.AsUInt64()).AsUInt32();
            var hiplm1 = Add(prodhi.AsUInt32(), m1);
            var prodlohi = Shuffle(prodlo, 0xF5);
            var lmlr02 = Multiply(prodlo, r).AsUInt32();
            var lmlr13 = Multiply(prodlohi, r).AsUInt32();
            var prod02_ = Multiply(lmlr02, m1).AsUInt32();
            var prod13_ = Multiply(lmlr13, m1).AsUInt32();
            var unpalo_ = UnpackLow(prod02_, prod13_);
            var unpahi_ = UnpackHigh(prod02_, prod13_);
            var prod = UnpackHigh(unpalo_.AsUInt64(), unpahi_.AsUInt64()).AsUInt32();
            return Subtract(hiplm1, prod);
        }
        [凾(256)]
        public static Vector128<uint> MontgomeryMultiply(Vector128<uint> a, Vector128<uint> b, Vector128<uint> r, Vector128<uint> m1)
        {
            var a13 = Shuffle(a, 0xF5);
            var b13 = Shuffle(b, 0xF5);
            var prod02 = Multiply(a, b);
            var prod13 = Multiply(a13, b13);
            return Reduce(prod02.AsUInt32(), prod13.AsUInt32(), r, m1);
        }
        [凾(256)]
        public static Vector128<uint> MontgomeryAdd(Vector128<uint> a, Vector128<uint> b, Vector128<uint> m2)
        {
            var ret = Subtract(Add(a, b), m2);
            return Add(And(CompareGreaterThan(Vector128<int>.Zero, ret.AsInt32()).AsUInt32(), m2), ret);
        }
        [凾(256)]
        public static Vector128<uint> MontgomerySubtract(Vector128<uint> a, Vector128<uint> b, Vector128<uint> m2)
        {
            var ret = Subtract(a, b);
            return Add(And(CompareGreaterThan(Vector128<int>.Zero, ret.AsInt32()).AsUInt32(), m2), ret);
        }

        [凾(256)]
        public static Vector256<uint> Reduce(Vector256<uint> prod02, Vector256<uint> prod13, Vector256<uint> r, Vector256<uint> m1)
        {
            var unpalo = UnpackLow(prod02, prod13);
            var unpahi = UnpackHigh(prod02, prod13);
            var prodlo = UnpackLow(unpalo.AsUInt64(), unpahi.AsUInt64()).AsUInt32();
            var prodhi = UnpackHigh(unpalo.AsUInt64(), unpahi.AsUInt64()).AsUInt32();
            var hiplm1 = Add(prodhi.AsUInt32(), m1);
            var prodlohi = Shuffle(prodlo, 0xF5);
            var lmlr02 = Multiply(prodlo, r).AsUInt32();
            var lmlr13 = Multiply(prodlohi, r).AsUInt32();
            var prod02_ = Multiply(lmlr02, m1).AsUInt32();
            var prod13_ = Multiply(lmlr13, m1).AsUInt32();
            var unpalo_ = UnpackLow(prod02_, prod13_);
            var unpahi_ = UnpackHigh(prod02_, prod13_);
            var prod = UnpackHigh(unpalo_.AsUInt64(), unpahi_.AsUInt64()).AsUInt32();
            return Subtract(hiplm1, prod);
        }
        [凾(256)]
        public static Vector256<uint> MontgomeryMultiply(Vector256<uint> a, Vector256<uint> b, Vector256<uint> r, Vector256<uint> m1)
        {
            var a13 = Shuffle(a, 0xF5);
            var b13 = Shuffle(b, 0xF5);
            var prod02 = Multiply(a, b);
            var prod13 = Multiply(a13, b13);
            return Reduce(prod02.AsUInt32(), prod13.AsUInt32(), r, m1);
        }
        [凾(256)]
        public static Vector256<uint> MontgomeryAdd(Vector256<uint> a, Vector256<uint> b, Vector256<uint> m2)
        {
            var ret = Subtract(Add(a, b), m2);
            return Add(And(CompareGreaterThan(Vector256<int>.Zero, ret.AsInt32()).AsUInt32(), m2), ret);
        }
        [凾(256)]
        public static Vector256<uint> MontgomerySubtract(Vector256<uint> a, Vector256<uint> b, Vector256<uint> m2)
        {
            var ret = Subtract(a, b);
            return Add(And(CompareGreaterThan(Vector256<int>.Zero, ret.AsInt32()).AsUInt32(), m2), ret);
        }
    }
}
