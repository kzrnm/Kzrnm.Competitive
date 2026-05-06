#if !LIBRARY
using Kzrnm.Competitive;
using SourceExpander;

namespace Embedding;

public class SourceExpanderTest
{
    [Test, Kzrnm.Competitive.Testing.MultipleAssertions]
    public async Task LanguageVersion()
    {
        var embedded = await EmbeddedData.LoadFromAssembly(typeof(Global));
#if NET9_0
        const string expected = "13.0";
#elif NET10_0
        const string expected = "14.0";
#endif
        await embedded.AssemblyMetadatas.Should().ContainKey("SourceExpander.EmbeddedLanguageVersion");
        await embedded.AssemblyMetadatas.Should().ContainKeyWithValue("SourceExpander.EmbeddedLanguageVersion", expected);
    }

    [Test, Kzrnm.Competitive.Testing.MultipleAssertions]
    public async Task EmbeddedSource()
    {
        var list = new List<string> { "apple", "banana", "cherry" };
        await Assert.That(list).Contains("banana");
        await list.Should().Contain("banana");

        var embedded = await EmbeddedData.LoadFromAssembly(typeof(Global));
        await embedded.AssemblyMetadatas.Should().ContainKey("SourceExpander.EmbeddedSourceCode.GZipBase32768");
        await embedded.SourceFiles.Select(s => s.FileName).Should().HaveAtLeast(3);
        await embedded.SourceFiles.Select(s => s.FileName).Should().All(name => name.StartsWith("Competitive.Library>"));

        await embedded.SourceFiles.SelectMany(s => s.TypeNames).Should().Contain("Kzrnm.Competitive.Global");
        await embedded.SourceFiles.SelectMany(s => s.TypeNames).Should().Contain("Kzrnm.Competitive.__FenwickTreeExtension");
    }
    [Test, Kzrnm.Competitive.Testing.MultipleAssertions]
    public async Task EmbeddedNamespaces()
    {
        var embedded = await EmbeddedData.LoadFromAssembly(typeof(Global));
        await embedded.AssemblyMetadatas.Should().ContainKey("SourceExpander.EmbeddedNamespaces");
        await embedded.EmbeddedNamespaces.Should().Contain("Kzrnm.Competitive");
        await embedded.EmbeddedNamespaces.Should().Contain("Kzrnm.Competitive.Internal");
    }
}
#endif