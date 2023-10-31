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
#endif
            embedded.AssemblyMetadatas
                .Should().ContainKey("SourceExpander.EmbeddedLanguageVersion")
                .WhoseValue.Should().Be(expected);
        }

        [Fact]
        public async Task EmbeddedSource()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Global));
            embedded.AssemblyMetadatas.Should().ContainKey("SourceExpander.EmbeddedSourceCode.GZipBase32768");
            embedded.SourceFiles.Select(s => s.FileName)
                .Should()
                .HaveCountGreaterThan(2)
                .And
                .OnlyContain(name => name.StartsWith("Competitive.Library>"));
            embedded.SourceFiles.SelectMany(s => s.TypeNames)
                .Should()
                .Contain(
                    "Kzrnm.Competitive.Global",
                    "Kzrnm.Competitive.FenwickTreeExtension");
        }

        [Fact]
        public async Task EmbeddedNamespaces()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Global));
            embedded.AssemblyMetadatas.Should().ContainKey("SourceExpander.EmbeddedNamespaces");
            embedded.EmbeddedNamespaces
                .Should()
                .Contain(
                    "Kzrnm.Competitive.Internal",
                    "Kzrnm.Competitive");

        }
    }
#endif
}
