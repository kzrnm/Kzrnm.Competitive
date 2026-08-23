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
        [凾(256)]
        public static ReadOnlySpan<byte> AsBytes(this Span<Ascii> a)
            => ((ReadOnlySpan<Ascii>)a).AsBytes();
        /// <inheritdoc cref="AsBytes(ReadOnlySpan{Ascii})" />
        public static ReadOnlySpan<byte> AsBytes(this Ascii[] a)
            => ((ReadOnlySpan<Ascii>)a).AsBytes();
#endif
        /// <summary>
        /// <see cref="Ascii"/> を byte として扱います。
        /// </summary>
        [凾(256)]
        public static ReadOnlySpan<byte> AsBytes(this ReadOnlySpan<Ascii> a)
            => MemoryMarshal.Cast<Ascii, byte>(a);
    }
}
