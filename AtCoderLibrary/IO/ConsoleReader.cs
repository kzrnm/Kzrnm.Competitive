using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace AtCoder.IO
{
#pragma warning disable CA1819,CA1815,CA1034
    [DebuggerStepThrough]
    public class ConsoleReader
    {
        const int BufSize = 1 << 12;
        private readonly byte[] buffer = new byte[BufSize];
        private readonly Stream input;
        private readonly Encoding encoding;
        private int pos = 0;
        private int len = 0;
        public ConsoleReader(Stream input, Encoding encoding) { this.input = input; this.encoding = encoding; }
        public ConsoleReader() : this(Console.OpenStandardInput(), Console.InputEncoding) { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void MoveNext() { if (++pos >= len) { len = input.Read(buffer, 0, buffer.Length); if (len == 0) { buffer[0] = 10; } pos = 0; } }

        public int Int
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                int res = 0;
                bool neg = false;
                while (buffer[pos] < 48) { neg = buffer[pos] == 45; MoveNext(); }
                do { res = checked(res * 10 + (buffer[pos] ^ 48)); MoveNext(); } while (48 <= buffer[pos]);
                return neg ? -res : res;
            }
        }
        public int Int0 => Int - 1;
        public long Long
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                long res = 0;
                bool neg = false;
                while (buffer[pos] < 48) { neg = buffer[pos] == 45; MoveNext(); }
                do { res = res * 10 + (buffer[pos] ^ 48); MoveNext(); } while (48 <= buffer[pos]);
                return neg ? -res : res;
            }
        }
        public long Long0 => Long - 1;
        public string String
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var sb = new List<byte>();
                while (buffer[pos] <= 32) MoveNext();
                do { sb.Add(buffer[pos]); MoveNext(); } while (32 < buffer[pos]);
                return encoding.GetString(sb.ToArray());
            }
        }
        public string Ascii
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var sb = new StringBuilder();
                while (buffer[pos] <= 32) MoveNext();
                do { sb.Append((char)buffer[pos]); MoveNext(); } while (32 < buffer[pos]);
                return sb.ToString();
            }
        }
        public string Line
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var sb = new List<byte>();
                while (buffer[pos] <= 32) MoveNext();
                do { sb.Add(buffer[pos]); MoveNext(); } while (buffer[pos] != 10 && buffer[pos] != 13);
                return encoding.GetString(sb.ToArray());
            }
        }
        public char Char
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                while (buffer[pos] <= 32) MoveNext();
                char res = (char)buffer[pos];
                MoveNext();
                return res;
            }
        }
        public double Double => double.Parse(Ascii);

        [DebuggerStepThrough]
        public ref struct RepeatReader
        {
            readonly ConsoleReader cr;
            readonly int count;
            public RepeatReader(ConsoleReader cr, int count) { this.cr = cr; this.count = count; }
            public T[] Select<T>(Func<ConsoleReader, T> factory) { var arr = new T[count]; for (var i = 0; i < count; i++) arr[i] = factory(cr); return arr; }
            public T[] Select<T>(Func<ConsoleReader, int, T> factory) { var arr = new T[count]; for (var i = 0; i < count; i++) arr[i] = factory(cr, i); return arr; }
            public string[] Line { get { var arr = new string[count]; for (var i = 0; i < count; i++) arr[i] = cr.Line; return arr; } }
            public string[] String { get { var arr = new string[count]; for (var i = 0; i < count; i++) arr[i] = cr.String; return arr; } }
            public string[] Ascii { get { var arr = new string[count]; for (var i = 0; i < count; i++) arr[i] = cr.Ascii; return arr; } }
            public int[] Int { get { var arr = new int[count]; for (var i = 0; i < count; i++) arr[i] = cr.Int; return arr; } }
            public int[] Int0 { get { var arr = new int[count]; for (var i = 0; i < count; i++) arr[i] = cr.Int0; return arr; } }
            public long[] Long { get { var arr = new long[count]; for (var i = 0; i < count; i++) arr[i] = cr.Long; return arr; } }
            public long[] Long0 { get { var arr = new long[count]; for (var i = 0; i < count; i++) arr[i] = cr.Long0; return arr; } }
            public double[] Double { get { var arr = new double[count]; for (var i = 0; i < count; i++) arr[i] = cr.Double; return arr; } }

            public static implicit operator int[](RepeatReader rr) => rr.Int;
            public static implicit operator long[](RepeatReader rr) => rr.Long;
            public static implicit operator double[](RepeatReader rr) => rr.Double;
            public static implicit operator string[](RepeatReader rr) => rr.Ascii;
        }
        public RepeatReader Repeat(int count) => new RepeatReader(this, count);

        [DebuggerStepThrough]
        public ref struct SplitReader
        {
            readonly ConsoleReader cr;
            public SplitReader(ConsoleReader cr) { this.cr = cr; }
            public string[] String { get { while (cr.buffer[cr.pos] <= 32) cr.MoveNext(); var list = new List<string>(); do { if (cr.buffer[cr.pos] < 32) cr.MoveNext(); else list.Add(cr.String); } while (cr.buffer[cr.pos] != 10 && cr.buffer[cr.pos] != 13); return list.ToArray(); } }
            public string[] Ascii { get { while (cr.buffer[cr.pos] <= 32) cr.MoveNext(); var list = new List<string>(); do { if (cr.buffer[cr.pos] < 32) cr.MoveNext(); else list.Add(cr.Ascii); } while (cr.buffer[cr.pos] != 10 && cr.buffer[cr.pos] != 13); return list.ToArray(); } }
            public int[] Int { get { while (cr.buffer[cr.pos] <= 32) cr.MoveNext(); var list = new List<int>(); do { if (cr.buffer[cr.pos] < 32) cr.MoveNext(); else list.Add(cr.Int); } while (cr.buffer[cr.pos] != 10 && cr.buffer[cr.pos] != 13); return list.ToArray(); } }
            public int[] Int0 { get { while (cr.buffer[cr.pos] <= 32) cr.MoveNext(); var list = new List<int>(); do { if (cr.buffer[cr.pos] < 32) cr.MoveNext(); else list.Add(cr.Int0); } while (cr.buffer[cr.pos] != 10 && cr.buffer[cr.pos] != 13); return list.ToArray(); } }
            public long[] Long { get { while (cr.buffer[cr.pos] <= 32) cr.MoveNext(); var list = new List<long>(); do { if (cr.buffer[cr.pos] < 32) cr.MoveNext(); else list.Add(cr.Long); } while (cr.buffer[cr.pos] != 10 && cr.buffer[cr.pos] != 13); return list.ToArray(); } }
            public long[] Long0 { get { while (cr.buffer[cr.pos] <= 32) cr.MoveNext(); var list = new List<long>(); do { if (cr.buffer[cr.pos] < 32) cr.MoveNext(); else list.Add(cr.Long0); } while (cr.buffer[cr.pos] != 10 && cr.buffer[cr.pos] != 13); return list.ToArray(); } }
            public double[] Double { get { while (cr.buffer[cr.pos] <= 32) cr.MoveNext(); var list = new List<double>(); do { if (cr.buffer[cr.pos] < 32) cr.MoveNext(); else list.Add(cr.Double); } while (cr.buffer[cr.pos] != 10 && cr.buffer[cr.pos] != 13); return list.ToArray(); } }

            public static implicit operator int[](SplitReader sr) => sr.Int;
            public static implicit operator long[](SplitReader sr) => sr.Long;
            public static implicit operator double[](SplitReader sr) => sr.Double;
            public static implicit operator string[](SplitReader sr) => sr.Ascii;
        }
        public SplitReader Split => new SplitReader(this);

        public static implicit operator int(ConsoleReader cr) => cr.Int;
        public static implicit operator long(ConsoleReader cr) => cr.Long;
        public static implicit operator double(ConsoleReader cr) => cr.Double;
        public static implicit operator string(ConsoleReader cr) => cr.Ascii;
    }

}
