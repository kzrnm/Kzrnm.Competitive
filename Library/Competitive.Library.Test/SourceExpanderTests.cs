using Kzrnm.Competitive;
using SourceExpander;
using System.Linq;
using System.Threading.Tasks;

namespace Embedding
{
#if !LIBRARY
    public class SourceExpanderTest
    {
        [Fact]
        public async Task LanguageVersion()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Global));
#if NET7_0
            const string expected = "11.0";
#elif NET8_0
            const string expected = "12.0";
#elif NET9_0
            const string expected = "13.0";
#endif
            embedded.AssemblyMetadatas.ShouldContainKeyAndValue("SourceExpander.EmbeddedLanguageVersion", expected);
        }

        [Fact]
        public async Task EmbeddedSource()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Global));
            embedded.AssemblyMetadatas.ShouldContainKey("SourceExpander.EmbeddedSourceCode.GZipBase32768");
            embedded.SourceFiles.Select(s => s.FileName).Count().ShouldBeGreaterThan(2);
            embedded.SourceFiles.Select(s => s.FileName).ShouldAllBe(name => name.StartsWith("Competitive.Library>"));

            embedded.SourceFiles.SelectMany(s => s.TypeNames).ShouldContain("Kzrnm.Competitive.Global");
            embedded.SourceFiles.SelectMany(s => s.TypeNames).ShouldContain("Kzrnm.Competitive.__FenwickTreeExtension");
        }

        [Fact]
        public async Task EmbeddedNamespaces()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Global));
            embedded.AssemblyMetadatas.ShouldContainKey("SourceExpander.EmbeddedNamespaces");
            embedded.EmbeddedNamespaces.ShouldContain("Kzrnm.Competitive");
            embedded.EmbeddedNamespaces.ShouldContain("Kzrnm.Competitive.Internal");

        }
    }
#endif
}
