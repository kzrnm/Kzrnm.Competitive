using System;
using System.Runtime.InteropServices;
using Ascii = Kzrnm.Competitive.IO.Ascii;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __AsciiExtension
    {
#if !NET10_0_OR_GREATER
        /// <inheritdoc cref="AsBytes(ReadOnlySpan{Ascii})" />
        public static Span<byte> AsBytes(this Ascii[] a)
            => ((Span<Ascii>)a).AsBytes();
#endif
        /// <inheritdoc cref="AsBytes(ReadOnlySpan{Ascii})" />
        [凾(256)]
        public static Span<byte> AsBytes(this Span<Ascii> a)
            => MemoryMarshal.Cast<Ascii, byte>(a);
        /// <summary>
        /// <see cref="Ascii"/> を byte として扱います。
        /// </summary>
        [凾(256)]
        public static ReadOnlySpan<byte> AsBytes(this ReadOnlySpan<Ascii> a)
            => MemoryMarshal.Cast<Ascii, byte>(a);
    }
}
