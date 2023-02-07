using System;
using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.IO
{
    // competitive-verifier: TITLE TupleWriterExtension
    public static class __Utf8ConsoleWriter__TupleWriterExtension
    {
        [凾(256)]
        public static Utf8ConsoleWriter WriteTuples<T1, T2>(this Utf8ConsoleWriter cw, ReadOnlySpan<(T1, T2)> tuple)
        {
            foreach (var t in tuple)
                cw.WriteLineJoin(t);
            return cw;
        }
        [凾(256)]
        public static Utf8ConsoleWriter WriteTuples<T1, T2>(this Utf8ConsoleWriter cw, IEnumerable<(T1, T2)> tuple)
        {
            foreach (var t in tuple)
                cw.WriteLineJoin(t);
            return cw;
        }
        [凾(256)]
        public static Utf8ConsoleWriter WriteTuples<T1, T2, T3>(this Utf8ConsoleWriter cw, ReadOnlySpan<(T1, T2, T3)> tuple)
        {
            foreach (var t in tuple)
                cw.WriteLineJoin(t);
            return cw;
        }
        [凾(256)]
        public static Utf8ConsoleWriter WriteTuples<T1, T2, T3>(this Utf8ConsoleWriter cw, IEnumerable<(T1, T2, T3)> tuple)
        {
            foreach (var t in tuple)
                cw.WriteLineJoin(t);
            return cw;
        }
        [凾(256)]
        public static Utf8ConsoleWriter WriteTuples<T1, T2, T3, T4>(this Utf8ConsoleWriter cw, ReadOnlySpan<(T1, T2, T3, T4)> tuple)
        {
            foreach (var t in tuple)
                cw.WriteLineJoin(t);
            return cw;
        }
        [凾(256)]
        public static Utf8ConsoleWriter WriteTuples<T1, T2, T3, T4>(this Utf8ConsoleWriter cw, IEnumerable<(T1, T2, T3, T4)> tuple)
        {
            foreach (var t in tuple)
                cw.WriteLineJoin(t);
            return cw;
        }
    }
}
