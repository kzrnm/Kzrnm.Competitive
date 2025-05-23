using Kzrnm.Competitive.IO;
using System.IO;
using System.Linq;
using System.Text;

namespace Kzrnm.Competitive.Testing.IO;

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
        cr.AsciiToNum('a').ShouldBe(Enumerable.Range(0, 26).Select(v => (byte)v));
    }

    [Fact]
    public void LargeAlphabet()
    {
        var cr = GetReader("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        cr.AsciiToNum('A').ShouldBe(Enumerable.Range(0, 26).Select(v => (byte)v));

        cr = GetReader("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        cr.AsciiToNum().ShouldBe(Enumerable.Range(0, 26).Select(v => (byte)v));
    }

    [Fact]
    public void Number()
    {
        var cr = GetReader("0123456789");
        cr.AsciiToNum('0').ShouldBe(Enumerable.Range(0, 10).Select(v => (byte)v));
    }
}
