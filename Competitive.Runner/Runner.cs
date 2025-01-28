using Kzrnm.Competitive.IO;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Competitive.Runner
{
    public class LineString : IXunitSerializable
    {
        public string Value { get; set; }
        void IXunitSerializable.Deserialize(IXunitSerializationInfo info)
        {
            Value = info.GetValue<string>(nameof(Value));
        }

        void IXunitSerializable.Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Value), Value);
        }
        public override string ToString() => Value.TrimEnd().Replace("\n", "↵");
        public static implicit operator LineString(string v) => new LineString { Value = v };
        public static implicit operator string(LineString l) => l.Value;
    }
    public partial class Runner
    {
        private class ResouceSource : TheoryData<int, LineString, LineString>
        {
            public ResouceSource()
            {
                int num = 0;
                Add(++num, InOut.A_IN, InOut.A_OUT);
                Add(++num, InOut.B_IN, InOut.B_OUT);
                Add(++num, InOut.C_IN, InOut.C_OUT);
                Add(++num, InOut.D_IN, InOut.D_OUT);
                Add(++num, InOut.E_IN, InOut.E_OUT);
                Add(++num, InOut.F_IN, InOut.F_OUT);
            }
            public void Add(int num, string p1, string p2)
            {
                if (!string.IsNullOrEmpty(p1) && !string.IsNullOrEmpty(p2))
                    base.Add(num, p1, p2);
            }
        }

        [Theory(Timeout = 4000)]
        [ClassData(typeof(ResouceSource))]
        public Task FromSource(int _, LineString input, LineString output) => Task.Run(() =>
        {
            var encoding = new UTF8Encoding(false);
            using var inSteam = new MemoryStream(encoding.GetBytes(input));
            using var outStream = new MemoryStream(30 * 100000);
            var cr = new PropertyConsoleReader(inSteam, encoding);
            var cw = new Utf8ConsoleWriter(outStream);
            new Program(cr, cw).Run();

            var result = encoding.GetString(outStream.ToArray());
            if (DoubleRegex().IsMatch(output))
                Assert.Equal(double.Parse(output), double.Parse(result), 10);
            else
                Assert.Equal(Normalize(output), Normalize(result));

            static string Normalize(string s) => s.Replace("\r\n", "\n").Trim();
        }, TestContext.Current.CancellationToken);
        [GeneratedRegex("^-?\\d+\\.\\d+$", RegexOptions.IgnoreCase, "ja-JP")]
        private static partial Regex DoubleRegex();
    }
}