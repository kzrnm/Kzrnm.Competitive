using AtCoder;
using FluentAssertions;
using SourceExpander;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Embedding
{
    public class SourceExpanderTest
    {
        [Fact]
        public async Task LanguageVersion()
        {
            var embedded = await EmbeddedData.LoadFromAssembly(typeof(Global));
            embedded.AssemblyMetadatas
                .Should().ContainKey("SourceExpander.EmbeddedLanguageVersion")
                .WhichValue.Should().Be("8.0");
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
                    "AtCoder.Global",
                    "AtCoder.FenwickTreeExtension");
        }
    }
}
