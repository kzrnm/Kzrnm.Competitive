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
    public readonly struct VectorizedStaticModInt<T> : IEquatable<VectorizedStaticModInt<T>>
        where T : struct, IStaticMod
    {
        private static readonly T op = default;
        internal static readonly Vector256<uint> R;
        internal static readonly Vector256<uint> M1;
        internal static readonly Vector256<uint> M2;
        internal static readonly Vector256<uint> N2;
        static VectorizedStaticModInt()
        {
            var rv = op.Mod;
            for (int i = 0; i < 4; ++i) rv *= 2 - op.Mod * rv;
            System.Diagnostics.Debug.Assert(rv * op.Mod == 1);
            R = Vector256.Create(rv);
            M1 = Vector256.Create(op.Mod);
            M2 = Vector256.Create(op.Mod * 2);
            N2 = Vector256.Create((uint)(((ulong)-op.Mod) % op.Mod));
        }

        public readonly Vector256<uint> Value;
        public VectorizedStaticModInt(Vector256<uint> x) { Value = x; }
        public VectorizedStaticModInt(uint a) : this(Vector256.Create(a)) { }
        public VectorizedStaticModInt(uint a0, uint a1, uint a2, uint a3, uint a4, uint a5, uint a6, uint a7)
            : this(Vector256.Create(a0, a1, a2, a3, a4, a5, a6, a7)) { }
        [凾(256)]
        public static implicit operator VectorizedStaticModInt<T>(Vector256<uint> x) => new VectorizedStaticModInt<T>(x);
        public int this[int i]
        {
            [凾(256)]
            get => (int)Value.GetElement(i);
        }

        [凾(256)]
        public bool Equals(VectorizedStaticModInt<T> other) => Value.Equals(other.Value);
        public override bool Equals(object other) => other is VectorizedStaticModInt<T> o && Value.Equals(o.Value);
        public override int GetHashCode() => Value.GetHashCode();

        [凾(256)]
        private static uint Reduce1(ulong b) => (uint)(b % op.Mod);
        public override string ToString()
            => $"{Reduce1(Value.GetElement(0))} {Reduce1(Value.GetElement(1))} {Reduce1(Value.GetElement(2))} {Reduce1(Value.GetElement(3))} {Reduce1(Value.GetElement(4))} {Reduce1(Value.GetElement(5))} {Reduce1(Value.GetElement(6))} {Reduce1(Value.GetElement(7))}";

        [凾(256)]
        public static bool operator ==(VectorizedStaticModInt<T> lhs, VectorizedStaticModInt<T> rhs)
            => lhs.Value.Equals(rhs.Value);
        [凾(256)]
        public static bool operator !=(VectorizedStaticModInt<T> lhs, VectorizedStaticModInt<T> rhs)
            => !lhs.Value.Equals(rhs.Value);


        [凾(256 | 512)]
        public static Vector256<uint> Reduce(Vector256<uint> prod02, Vector256<uint> prod13)
        {
            var unpalo = UnpackLow(prod02, prod13);
            var unpahi = UnpackHigh(prod02, prod13);
            var prodlo = UnpackLow(unpalo.AsUInt64(), unpahi.AsUInt64()).AsUInt32();
            var prodhi = UnpackHigh(unpalo.AsUInt64(), unpahi.AsUInt64()).AsUInt32();
            var hiplm1 = Add(prodhi.AsUInt32(), M1);
            var prodlohi = Shuffle(prodlo, 0xF5);
            var lmlr02 = Multiply(prodlo, R).AsUInt32();
            var lmlr13 = Multiply(prodlohi, R).AsUInt32();
            var prod02_ = Multiply(lmlr02, M1).AsUInt32();
            var prod13_ = Multiply(lmlr13, M1).AsUInt32();
            var unpalo_ = UnpackLow(prod02_, prod13_);
            var unpahi_ = UnpackHigh(prod02_, prod13_);
            var prod = UnpackHigh(unpalo_.AsUInt64(), unpahi_.AsUInt64()).AsUInt32();
            return Subtract(hiplm1, prod);
        }

        [凾(256)]
        public VectorizedStaticModInt<T> Itom() => this * new VectorizedStaticModInt<T>(N2);

        [凾(256 | 512)]
        public VectorizedStaticModInt<T> Mtoi()
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
            return new VectorizedStaticModInt<T>(Subtract(dif, prod));
        }

        [凾(256 | 512)]
        public static VectorizedStaticModInt<T> operator +(VectorizedStaticModInt<T> lhs, VectorizedStaticModInt<T> rhs)
        {
            var A = lhs.Value;
            var B = rhs.Value;
            var apb = Add(A, B);
            var ret = Subtract(apb, M2);
            var cmp = CompareGreaterThan(Vector256<int>.Zero, ret.AsInt32()).AsUInt32();
            var add = And(cmp, M2);
            return new VectorizedStaticModInt<T>(Add(add, ret));
        }

        [凾(256 | 512)]
        public static VectorizedStaticModInt<T> operator -(VectorizedStaticModInt<T> lhs, VectorizedStaticModInt<T> rhs)
        {
            var A = lhs.Value;
            var B = rhs.Value;
            var ret = Subtract(A, B);
            var cmp = CompareGreaterThan(Vector256<int>.Zero, ret.AsInt32()).AsUInt32();
            var add = And(cmp, M2);
            return new VectorizedStaticModInt<T>(Add(add, ret));
        }

        [凾(256 | 512)]
        public static VectorizedStaticModInt<T> operator *(VectorizedStaticModInt<T> lhs, VectorizedStaticModInt<T> rhs)
        {
            var A = lhs.Value;
            var B = rhs.Value;
            var a13 = Shuffle(A, 0xF5);
            var b13 = Shuffle(B, 0xF5);
            var prod02 = Multiply(A, B);
            var prod13 = Multiply(a13, b13);
            return new VectorizedStaticModInt<T>(Reduce(prod02.AsUInt32(), prod13.AsUInt32()));
        }
    }
}
