using Kzrnm.Competitive.IO;

namespace Kzrnm.Competitive.Testing.IO;

public class AsciiExtensionTests
{
    [Test]
    public async Task AsBytes()
    {
        var asciis = new Ascii[3];
        asciis[0] = 'a';
        asciis[1] = 'b';
        asciis[2] = 'C';
        await Assert.That(asciis.AsBytes().SequenceEqual("abC"u8)).IsTrue();
        await Assert.That(((Span<Ascii>)asciis).AsBytes().SequenceEqual("abC"u8)).IsTrue();
        await Assert.That(((ReadOnlySpan<Ascii>)asciis).AsBytes().SequenceEqual("abC"u8)).IsTrue();
    }
}