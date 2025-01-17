using Kzrnm.Competitive.IO;
using System.IO;
using System.Text;

namespace Kzrnm.Competitive.Testing.IO
{
    public class TupleReaderExtensionTests
    {
        static readonly UTF8Encoding enc = new UTF8Encoding(false);
        PropertyConsoleReader GetReader(string text)
        {
            return new PropertyConsoleReader(new MemoryStream(enc.GetBytes(text)), enc);
        }

        [Fact]
        public void IntInt()
        {
            var cr = GetReader(
                """
                1 2
                13 14
                -5 -6
                -7 -8
                """);
            cr.Repeat(4).IntInt().ShouldBe([
                (1, 2),
                (13, 14),
                (-5, -6),
                (-7, -8),
            ]);
        }

        [Fact]
        public void Int0Int()
        {
            var cr = GetReader(
                """
                1 2
                13 14
                -5 -6
                -7 -8
                """);
            cr.Repeat(4).Int0Int().ShouldBe([
                (0, 2),
                (12, 14),
                (-6, -6),
                (-8, -8),
            ]);
        }

        [Fact]
        public void Int0Int0()
        {
            var cr = GetReader(
                """
                1 2
                13 14
                -5 -6
                -7 -8
                """);
            cr.Repeat(4).Int0Int0().ShouldBe([
                (0, 1),
                (12, 13),
                (-6, -7),
                (-8, -9),
            ]);
        }

        [Fact]
        public void IntInt0()
        {
            var cr = GetReader(
                """
                1 2
                13 14
                -5 -6
                -7 -8
                """);
            cr.Repeat(4).IntInt0().ShouldBe([
                (1, 1),
                (13, 13),
                (-5, -7),
                (-7, -9),
            ]);
        }
    }
}
