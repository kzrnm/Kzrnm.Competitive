using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive.IO
{
    // competitive-verifier: TITLE TupleWriterExtension
    public static class __Utf8ConsoleWriter__TupleWriterExtension
    {
        public static Utf8ConsoleWriter WriteTuples<T1, T2>(this Utf8ConsoleWriter cw, ReadOnlySpan<(T1, T2)> tuple)
        {
            foreach (var t in tuple)
                cw.WriteLineJoin(t);
            return cw;
        }
        public static Utf8ConsoleWriter WriteTuples<T1, T2>(this Utf8ConsoleWriter cw, IEnumerable<(T1, T2)> tuple)
        {
            foreach (var t in tuple)
                cw.WriteLineJoin(t);
            return cw;
        }
        public static Utf8ConsoleWriter WriteTuples<T1, T2, T3>(this Utf8ConsoleWriter cw, ReadOnlySpan<(T1, T2, T3)> tuple)
        {
            foreach (var t in tuple)
                cw.WriteLineJoin(t);
            return cw;
        }
        public static Utf8ConsoleWriter WriteTuples<T1, T2, T3>(this Utf8ConsoleWriter cw, IEnumerable<(T1, T2, T3)> tuple)
        {
            foreach (var t in tuple)
                cw.WriteLineJoin(t);
            return cw;
        }
        public static Utf8ConsoleWriter WriteTuples<T1, T2, T3, T4>(this Utf8ConsoleWriter cw, ReadOnlySpan<(T1, T2, T3, T4)> tuple)
        {
            foreach (var t in tuple)
                cw.WriteLineJoin(t);
            return cw;
        }
        public static Utf8ConsoleWriter WriteTuples<T1, T2, T3, T4>(this Utf8ConsoleWriter cw, IEnumerable<(T1, T2, T3, T4)> tuple)
        {
            foreach (var t in tuple)
                cw.WriteLineJoin(t);
            return cw;
        }
    }
}
