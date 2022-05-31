using Kzrnm.Competitive.IO;
using System;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    using O = ConsoleOutput;
    public static class ConsoleOutputExt
    {
        [凾(256)]
        public static O ToConsoleOutput<T>(this T f) where T : IUtf8ConsoleWriterFormatter
        {
            var cw = O.cw;
            f.Write(cw);
            return cw.WriteLine();
        }
    }
    public struct ConsoleOutput
    {
        public static Utf8ConsoleWriter cw;
        public static implicit operator O(int v) => cw.WriteLine(v);
        public static implicit operator O(long v) => cw.WriteLine(v);
        public static implicit operator O(uint v) => cw.WriteLine(v);
        public static implicit operator O(ulong v) => cw.WriteLine(v);
        public static implicit operator O(double v) => cw.WriteLine(v);
        public static implicit operator O(decimal v) => cw.WriteLine(v);
        public static implicit operator O(char v) => cw.WriteLine(v);
        public static implicit operator O(ReadOnlySpan<char> v) => cw.WriteLine(v);
        public static implicit operator O(char[] v) => cw.WriteLine((ReadOnlySpan<char>)v);
        public static implicit operator O(string v) => cw.WriteLine((ReadOnlySpan<char>)v);
        public static implicit operator O(bool v) => cw.WriteLine((ReadOnlySpan<char>)(v ? "Yes" : "No"));
        public static implicit operator O(Utf8ConsoleWriter _) => default;
    }
}
