using AtCoder.Internal;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://nyaannyaan.github.io/library/modint/barrett-reduction.hpp
    // competitive-verifier: TITLE barrett-reduction の拡張
    /// <summary>
    /// barrett-reduction の拡張
    /// </summary>
    public static class __BarrettExtension
    {
        [凾(256)]
        public static ulong Div(this Barrett bt, ulong z)
        {
            var IM = bt.IM;
            var Mod = bt.Mod;
            var x = InternalMath.Mul128Bit(z, IM);
            var v = unchecked((uint)(z - x * Mod));
            if (Mod <= v) --x;
            return x;
        }
        [凾(256)]
        public static (ulong Quotient, uint Remainder) DivRem(this Barrett bt, ulong z)
        {
            var IM = bt.IM;
            var Mod = bt.Mod;
            var x = InternalMath.Mul128Bit(z, IM);
            var v = unchecked((uint)(z - x * Mod));
            if (Mod <= v)
            {
                v += Mod;
                --x;
            }
            return (x, v);
        }
    }
}
