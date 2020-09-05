using AtCoderProject;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace AtCoderLib.Global
{
    public class ConsoleWriterTests
    {
        private const int BufSize = 1 << 8;
        private readonly byte[] buffer = new byte[BufSize];
        private readonly string newLine;
        private readonly MemoryStream stream;
        private readonly ConsoleWriter cw;
        public ConsoleWriterTests()
        {
            stream = new MemoryStream(buffer);
            cw = new ConsoleWriter(stream, new UTF8Encoding(false));
            newLine = cw.sw.NewLine;
        }

        private static byte[] ToBytes(ReadOnlySpan<char> str)
        {
            var res = new byte[str.Length];
            for (int i = 0; i < res.Length; i++) res[i] = (byte)str[i];
            return res;
        }

        [Fact]
        public void WriteLine()
        {
            cw.WriteLine(-123456);
            buffer.Should().Equal(Enumerable.Repeat(0, BufSize));
            cw.Flush();
            buffer.Should().StartWith(ToBytes("-123456" + newLine));
        }

        [Fact]
        public void WriteLineSpan()
        {
            cw.WriteLine("foobar".AsSpan());
            buffer.Should().Equal(Enumerable.Repeat(0, BufSize));
            cw.Flush();
            buffer.Should().StartWith(ToBytes("foobar" + newLine));
        }

        [Fact]
        public void WriteLineJoin2()
        {
            cw.WriteLineJoin("foo", 1);
            buffer.Should().Equal(Enumerable.Repeat(0, BufSize));
            cw.Flush();
            buffer.Should().StartWith(ToBytes($"foo 1{newLine}"));
        }

        [Fact]
        public void WriteLineJoin3()
        {
            cw.WriteLineJoin("foo", 1, -2L);
            buffer.Should().Equal(Enumerable.Repeat(0, BufSize));
            cw.Flush();
            buffer.Should().StartWith(ToBytes($"foo 1 -2{newLine}"));
        }

        [Fact]
        public void WriteLineJoin4()
        {
            cw.WriteLineJoin("foo", 1, -2L, 'x');
            buffer.Should().Equal(Enumerable.Repeat(0, BufSize));
            cw.Flush();
            buffer.Should().StartWith(ToBytes($"foo 1 -2 x{newLine}"));
        }

        [Fact]
        public void WriteLineJoinMany()
        {
            cw.WriteLineJoin("foo", 1, -2L, 'x', "bar");
            buffer.Should().Equal(Enumerable.Repeat(0, BufSize));
            cw.Flush();
            buffer.Should().StartWith(ToBytes($"foo 1 -2 x bar{newLine}"));
        }

        [Fact]
        public void WriteLineJoinManySameType()
        {
            cw.WriteLineJoin(1, 2, 3, 4, 5);
            buffer.Should().Equal(Enumerable.Repeat(0, BufSize));
            cw.Flush();
            buffer.Should().StartWith(ToBytes($"1 2 3 4 5{newLine}"));
        }

        [Fact]
        public void WriteLineJoinIEnumerable()
        {
            cw.WriteLineJoin(Enumerable.Range(1, 5));
            buffer.Should().Equal(Enumerable.Repeat(0, BufSize));
            cw.Flush();
            buffer.Should().StartWith(ToBytes($"1 2 3 4 5{newLine}"));
        }

        [Fact]
        public void WriteLineJoinSpan()
        {
            cw.WriteLineJoin((Span<int>)Enumerable.Range(1, 5).ToArray());
            buffer.Should().Equal(Enumerable.Repeat(0, BufSize));
            cw.Flush();
            buffer.Should().StartWith(ToBytes($"1 2 3 4 5{newLine}"));
        }

        [Fact]
        public void WriteLineJoinReadOnlySpan()
        {
            cw.WriteLineJoin((ReadOnlySpan<int>)Enumerable.Range(1, 5).ToArray());
            buffer.Should().Equal(Enumerable.Repeat(0, BufSize));
            cw.Flush();
            buffer.Should().StartWith(ToBytes($"1 2 3 4 5{newLine}"));
        }

        [Fact]
        public void WriteLinesIEnumerable()
        {
            cw.WriteLines(Enumerable.Range(1, 5));
            buffer.Should().Equal(Enumerable.Repeat(0, BufSize));
            cw.Flush();
            buffer.Should().StartWith(ToBytes($"1\n2\n3\n4\n5{newLine}"));
        }

        [Fact]
        public void WriteLinesSpan()
        {
            cw.WriteLines((Span<int>)Enumerable.Range(1, 5).ToArray());
            buffer.Should().Equal(Enumerable.Repeat(0, BufSize));
            cw.Flush();
            buffer.Should().StartWith(ToBytes($"1\n2\n3\n4\n5{newLine}"));
        }

        [Fact]
        public void WriteLinesReadOnlySpan()
        {
            cw.WriteLines((ReadOnlySpan<int>)Enumerable.Range(1, 5).ToArray());
            buffer.Should().Equal(Enumerable.Repeat(0, BufSize));
            cw.Flush();
            buffer.Should().StartWith(ToBytes($"1\n2\n3\n4\n5{newLine}"));
        }
    }
}
