using Kzrnm.Competitive.IO;
using System.Text;

namespace Kzrnm.Competitive.Testing.IO;

public class TupleReaderExtensionTests
{
    static readonly UTF8Encoding enc = new UTF8Encoding(false);
    PropertyConsoleReader GetReader(string text)
    {
        return new PropertyConsoleReader(new MemoryStream(enc.GetBytes(text)), enc);
    }

    [Test, MultipleAssertions]
    public async Task IntInt()
    {
        var cr = GetReader(
            """
            1 2
            13 14
            -5 -6
            -7 -8
            """);
        await cr.Repeat(4).IntInt().Should().BeEquivalentOrderTo([
            (1, 2),
            (13, 14),
            (-5, -6),
            (-7, -8),
        ]);
    }

    [Test, MultipleAssertions]
    public async Task Int0Int()
    {
        var cr = GetReader(
            """
            1 2
            13 14
            -5 -6
            -7 -8
            """);
        await cr.Repeat(4).Int0Int().Should().BeEquivalentOrderTo([
            (0, 2),
            (12, 14),
            (-6, -6),
            (-8, -8),
        ]);
    }

    [Test, MultipleAssertions]
    public async Task Int0Int0()
    {
        var cr = GetReader(
            """
            1 2
            13 14
            -5 -6
            -7 -8
            """);
        await cr.Repeat(4).Int0Int0().Should().BeEquivalentOrderTo([
            (0, 1),
            (12, 13),
            (-6, -7),
            (-8, -9),
        ]);
    }

    [Test, MultipleAssertions]
    public async Task IntInt0()
    {
        var cr = GetReader(
            """
            1 2
            13 14
            -5 -6
            -7 -8
            """);
        await cr.Repeat(4).IntInt0().Should().BeEquivalentOrderTo([
            (1, 1),
            (13, 13),
            (-5, -7),
            (-7, -9),
        ]);
    }
}