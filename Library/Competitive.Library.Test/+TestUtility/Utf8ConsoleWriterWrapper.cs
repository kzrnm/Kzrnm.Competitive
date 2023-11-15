using Kzrnm.Competitive.IO;
using System.IO;
using System.Text;

namespace Kzrnm.Competitive.Testing
{
    public class Utf8ConsoleWriterWrapper
    {
        readonly MemoryStream stream = new();
        public string Read() => new UTF8Encoding(false).GetString(stream.ToArray());
        public Utf8ConsoleWriter GetWriter() => new(stream);
    }
}
