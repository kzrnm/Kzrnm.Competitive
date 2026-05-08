using Kzrnm.Competitive.IO;
using System.Text;

namespace Kzrnm.Competitive.Testing.IO;

public class AsciiReaderExtensionTests
{
    static readonly UTF8Encoding enc = new UTF8Encoding(false);
    PropertyConsoleReader GetReader(string text)
    {
        return new PropertyConsoleReader(new MemoryStream(enc.GetBytes(text)), enc);
    }

    [Test, MultipleAssertions]
    public async Task SmallAlphabet()
    {
        var cr = GetReader("abcdefghijklmnopqrstuvwxyz");
        await cr.AsciiToNum('a').Should().BeStrictlyEquivalentTo(Enumerable.Range(0, 26).Select(v => (byte)v));
    }

    [Test, MultipleAssertions]
    public async Task LargeAlphabet()
    {
        var cr = GetReader("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        await cr.AsciiToNum('A').Should().BeStrictlyEquivalentTo(Enumerable.Range(0, 26).Select(v => (byte)v));

        cr = GetReader("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        await cr.AsciiToNum().Should().BeStrictlyEquivalentTo(Enumerable.Range(0, 26).Select(v => (byte)v));
    }

    [Test, MultipleAssertions]
    public async Task Number()
    {
        var cr = GetReader("0123456789");
        await cr.AsciiToNum('0').Should().BeStrictlyEquivalentTo(Enumerable.Range(0, 10).Select(v => (byte)v));
    }
}