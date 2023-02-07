using Kzrnm.Competitive.IO;
using System.IO;
using System.Linq;
using System.Text;

namespace Kzrnm.Competitive.Testing.IO
{
    public class AsciiReaderExtensionTests
    {
        static readonly UTF8Encoding enc = new UTF8Encoding(false);
        PropertyConsoleReader GetReader(string text)
        {
            return new PropertyConsoleReader(new MemoryStream(enc.GetBytes(text)), enc);
        }

        [Fact]
        public void SmallAlphabet()
        {
            var cr = GetReader("abcdefghijklmnopqrstuvwxyz");
            cr.AsciiToInt('a').Should().Equal(Enumerable.Range(0, 26));
        }

        [Fact]
        public void LargeAlphabet()
        {
            var cr = GetReader("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            cr.AsciiToInt('A').Should().Equal(Enumerable.Range(0, 26));
        }

        [Fact]
        public void Number()
        {
            var cr = GetReader("0123456789");
            cr.AsciiToInt('0').Should().Equal(Enumerable.Range(0, 10));
        }
    }
}
