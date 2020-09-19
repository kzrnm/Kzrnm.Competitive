using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AtCoder.IO
{
#pragma warning disable CA1819, CA1815, CA1034
    [DebuggerStepThrough]
    public class ConsoleWriter
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] public readonly StreamWriter sw;
        public ConsoleWriter() : this(Console.OpenStandardOutput(), Console.OutputEncoding) { }
        public ConsoleWriter(Stream output, Encoding encoding) { sw = new StreamWriter(output, encoding); }
        public void Flush() => sw.Flush();
        public ConsoleWriter WriteLine(ReadOnlySpan<char> obj) { sw.WriteLine(obj); return this; }
        public ConsoleWriter WriteLine<T>(T obj) { sw.WriteLine(obj.ToString()); return this; }
        public ConsoleWriter WriteLineJoin<T>(Span<T> col) => WriteMany(' ', (ReadOnlySpan<T>)col);
        public ConsoleWriter WriteLineJoin<T>(ReadOnlySpan<T> col) => WriteMany(' ', col);
        public ConsoleWriter WriteLineJoin<T>(IEnumerable<T> col) => WriteMany(' ', col);
        public ConsoleWriter WriteLineJoin<T>(params T[] col) => WriteMany(' ', col);
        public ConsoleWriter WriteLineJoin(params object[] col) => WriteMany(' ', col);
        public ConsoleWriter WriteLineJoin<T1, T2>(T1 v1, T2 v2)
        {
            sw.Write(v1.ToString()); sw.Write(' ');
            sw.WriteLine(v2.ToString()); return this;
        }
        public ConsoleWriter WriteLineJoin<T1, T2, T3>(T1 v1, T2 v2, T3 v3)
        {
            sw.Write(v1.ToString()); sw.Write(' ');
            sw.Write(v2.ToString()); sw.Write(' ');
            sw.WriteLine(v3.ToString()); return this;
        }
        public ConsoleWriter WriteLineJoin<T1, T2, T3, T4>(T1 v1, T2 v2, T3 v3, T4 v4)
        {
            sw.Write(v1.ToString()); sw.Write(' ');
            sw.Write(v2.ToString()); sw.Write(' ');
            sw.Write(v3.ToString()); sw.Write(' ');
            sw.WriteLine(v4.ToString()); return this;
        }
        public ConsoleWriter WriteLines<T>(Span<T> col) => WriteMany('\n', (ReadOnlySpan<T>)col);
        public ConsoleWriter WriteLines<T>(ReadOnlySpan<T> col) => WriteMany('\n', col);
        public ConsoleWriter WriteLines<T>(IEnumerable<T> col) => WriteMany('\n', col);
        public ConsoleWriter WriteLineGrid<T>(IEnumerable<IEnumerable<T>> cols)
        {
            foreach (var col in cols)
                WriteLineJoin(col);
            return this;
        }
        private ConsoleWriter WriteMany<T>(char sep, ReadOnlySpan<T> col)
        {
            var en = col.GetEnumerator();
            if (!en.MoveNext())
                return this;
            sw.Write(en.Current.ToString());
            while (en.MoveNext())
            {
                sw.Write(sep);
                sw.Write(en.Current.ToString());
            }
            sw.WriteLine();
            return this;
        }
        private ConsoleWriter WriteMany<T>(char sep, IEnumerable<T> col)
        {
            var en = col.GetEnumerator();
            if (!en.MoveNext())
                return this;
            sw.Write(en.Current.ToString());
            while (en.MoveNext())
            {
                sw.Write(sep);
                sw.Write(en.Current.ToString());
            }
            sw.WriteLine();
            return this;
        }
    }

}
