using AtCoder;
using System;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

// https://nyaannyaan.github.io/library/modint/vectorize-modint.hpp
namespace Kzrnm.Competitive
{
    using static Avx2;
    /// <summary>
    /// <see cref="Vector256{T}"/> で <see cref="MontgomeryModInt{T}"/> の演算を行う型
    /// </summary>
    public readonly struct MontgomeryModIntVectorize<T> : IEquatable<MontgomeryModIntVectorize<T>> where T : struct, IStaticMod
    {
        internal static Vector256<uint> R;
        internal static Vector256<uint> M1;
        internal static Vector256<uint> M2;
        internal static Vector256<uint> N2;
        static MontgomeryModIntVectorize()
        {
            var m = new T().Mod;
            R = Vector256.Create(MontgomeryModInt<T>.r);
            M1 = Vector256.Create(m);
            M2 = Vector256.Create(m * 2);
            N2 = Vector256.Create(MontgomeryModInt<T>.n2);
        }

        public readonly Vector256<uint> Value;
        MontgomeryModIntVectorize(Vector256<uint> x) { Value = x; }
        public MontgomeryModIntVectorize(uint a0, uint a1, uint a2, uint a3, uint a4, uint a5, uint a6, uint a7)
            : this(Vector256.Create(a0, a1, a2, a3, a4, a5, a6, a7)) { }
        [凾(256)]
        public static implicit operator MontgomeryModIntVectorize<T>(Vector256<uint> x) => new MontgomeryModIntVectorize<T>(x);

        [凾(256)]
        public bool Equals(MontgomeryModIntVectorize<T> other) => Value.Equals(other.Value);
        public override bool Equals(object other) => other is MontgomeryModIntVectorize<T> o && Value.Equals(o.Value);
        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString()
        {
            var val = Mtoi();
            return $"{val.GetElement(0)} {val.GetElement(1)} {val.GetElement(2)} {val.GetElement(3)} {val.GetElement(4)} {val.GetElement(5)} {val.GetElement(6)} {val.GetElement(7)}";
        }

        [凾(256)]
        public static bool operator ==(MontgomeryModIntVectorize<T> lhs, MontgomeryModIntVectorize<T> rhs)
            => lhs.Value.Equals(rhs.Value);
        [凾(256)]
        public static bool operator !=(MontgomeryModIntVectorize<T> lhs, MontgomeryModIntVectorize<T> rhs)
            => !lhs.Value.Equals(rhs.Value);


        [凾(256 | 512)]
        public static Vector256<uint> Reduce(Vector256<uint> prod02, Vector256<uint> prod13)
        {
            return SimdMontgomery.Reduce(prod02, prod13, R, M1);
        }

        [凾(256)]
        public MontgomeryModIntVectorize<T> Itom() => this * new MontgomeryModIntVectorize<T>(N2);

        [凾(256)]
        public Vector256<uint> Mtoi()
        {
            var A = Value;
            var A13 = Shuffle(A, 0xF5);
            var lmlr02 = Multiply(A, R).AsUInt32();
            var lmlr13 = Multiply(A13, R).AsUInt32();
            var prod02_ = Multiply(lmlr02, M1).AsUInt32();
            var prod13_ = Multiply(lmlr13, M1).AsUInt32();
            var unpalo_ = UnpackLow(prod02_, prod13_);
            var unpahi_ = UnpackHigh(prod02_, prod13_);
            var prod = UnpackHigh(unpalo_.AsUInt64(), unpahi_.AsUInt64()).AsUInt32();
            var cmp = CompareGreaterThan(prod.AsInt32(), Vector256<int>.Zero).AsUInt32();
            var dif = And(cmp, M1);
            return Subtract(dif, prod);
        }

        [凾(256 | 512)]
        public static MontgomeryModIntVectorize<T> operator +(MontgomeryModIntVectorize<T> lhs, MontgomeryModIntVectorize<T> rhs)
        {
            return SimdMontgomery.MontgomeryAdd(lhs.Value, rhs.Value, M2);
        }

        [凾(256 | 512)]
        public static MontgomeryModIntVectorize<T> operator -(MontgomeryModIntVectorize<T> lhs, MontgomeryModIntVectorize<T> rhs)
        {
            return SimdMontgomery.MontgomerySubtract(lhs.Value, rhs.Value, M2);
        }

        [凾(256 | 512)]
        public static MontgomeryModIntVectorize<T> operator *(MontgomeryModIntVectorize<T> lhs, MontgomeryModIntVectorize<T> rhs)
        {
            return SimdMontgomery.MontgomeryMultiply(lhs.Value, rhs.Value, R, M1);
        }
    }
}
