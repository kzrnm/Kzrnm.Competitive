using Kzrnm.Competitive.IO;
using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.IO
{
    public class TupleWriterExtensionTests
    {
        [Fact]
        public void WriteTuples2()
        {
            var utf8Wrapper = new Utf8ConsoleWriterWrapper();
            using (var cw = utf8Wrapper.GetWriter())
            {
                var arr = new (int Num, string Text)[]
                {
                    (1, "Red"),
                    (3, "Blue"),
                    (5, "Green"),
                    (7, "White"),
                };
                cw.WriteTuples<int, string>(arr.AsSpan());
                cw.WriteTuples(arr.AsEnumerable().Reverse());
            }
            utf8Wrapper.Read().Should().Be("""
            1 Red
            3 Blue
            5 Green
            7 White
            7 White
            5 Green
            3 Blue
            1 Red

            """.Replace("\r\n", "\n"));
        }

        [Fact]
        public void WriteTuples3()
        {
            var utf8Wrapper = new Utf8ConsoleWriterWrapper();
            using (var cw = utf8Wrapper.GetWriter())
            {
                var arr = new (int Num, string Text, char C)[]
                {
                    (1, "Red", 'r'),
                    (3, "Blue", 'b'),
                    (5, "Green", 'g'),
                    (7, "White", 'w'),
                };
                cw.WriteTuples<int, string, char>(arr.AsSpan());
                cw.WriteTuples(arr.AsEnumerable().Reverse());
            }
            utf8Wrapper.Read().Should().Be("""
            1 Red r
            3 Blue b
            5 Green g
            7 White w
            7 White w
            5 Green g
            3 Blue b
            1 Red r

            """.Replace("\r\n", "\n"));
        }

        [Fact]
        public void WriteTuples4()
        {
            var utf8Wrapper = new Utf8ConsoleWriterWrapper();
            using (var cw = utf8Wrapper.GetWriter())
            {
                var arr = new (int Num, string Text, char C, decimal D)[]
                {
                    (1, "Red", 'r', 0.5m),
                    (3, "Blue", 'b', 0.75m),
                    (5, "Green", 'g', 0.25m),
                    (7, "White", 'w', 0.3m),
                };
                cw.WriteTuples<int, string, char, decimal>(arr.AsSpan());
                cw.WriteTuples(arr.AsEnumerable().Reverse());
            }
            utf8Wrapper.Read().Should().Be("""
            1 Red r 0.50000000000000000000
            3 Blue b 0.75000000000000000000
            5 Green g 0.25000000000000000000
            7 White w 0.30000000000000000000
            7 White w 0.30000000000000000000
            5 Green g 0.25000000000000000000
            3 Blue b 0.75000000000000000000
            1 Red r 0.50000000000000000000

            """.Replace("\r\n", "\n"));
        }
    }
}
