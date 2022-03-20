using Kzrnm.Competitive.IO;
using System;

namespace Kzrnm.Competitive
{
    public static class ConsoleOutputExt
    {
        public static ConsoleOutput ToConsoleOutput<T>(this T f) where T : IUtf8ConsoleWriterFormatter
        {
            var cw = ConsoleOutput.cw;
            f.Write(cw);
            return cw.WriteLine();
        }
    }
    public struct ConsoleOutput
    {
        public static Utf8ConsoleWriter cw;
        public static implicit operator ConsoleOutput(int v) => cw.WriteLine(v);
        public static implicit operator ConsoleOutput(long v) => cw.WriteLine(v);
        public static implicit operator ConsoleOutput(uint v) => cw.WriteLine(v);
        public static implicit operator ConsoleOutput(ulong v) => cw.WriteLine(v);
        public static implicit operator ConsoleOutput(double v) => cw.WriteLine(v);
        public static implicit operator ConsoleOutput(decimal v) => cw.WriteLine(v);
        public static implicit operator ConsoleOutput(char v) => cw.WriteLine(v);
        public static implicit operator ConsoleOutput(char[] v) => cw.WriteLine((ReadOnlySpan<char>)v);
        public static implicit operator ConsoleOutput(string v) => cw.WriteLine((ReadOnlySpan<char>)v);
        public static implicit operator ConsoleOutput(bool v) => cw.WriteLine((ReadOnlySpan<char>)Program.YesNo(v));
        public static implicit operator ConsoleOutput(Utf8ConsoleWriter _) => default;
    }
}