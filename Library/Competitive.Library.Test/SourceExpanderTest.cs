using FluentAssertions;
using Kzrnm.Competitive;
using SourceExpander;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Embedding
{
#if !LIBRARY
    public class SourceExpanderTest
    {
        [Fact]
        public async Task LanguageVersion()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Global));
            embedded.AssemblyMetadatas
                .Should().ContainKey("SourceExpander.EmbeddedLanguageVersion")
                .WhoseValue.Should().Be("8.0");
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
                .Equal(
                    "Kzrnm.Competitive",
                    "Kzrnm.Competitive.DataStructure",
                    "Kzrnm.Competitive.DebugUtil",
                    "Kzrnm.Competitive.InternalWavelet",
                    "Kzrnm.Competitive.LinqInternals",
                    "Kzrnm.Competitive.SetInternals",
                    "Kzrnm.Competitive.Testing");

        }
    }
#endif
}
