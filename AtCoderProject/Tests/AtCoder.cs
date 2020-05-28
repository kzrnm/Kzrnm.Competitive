using AtCoderProject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace AtCoderProject.Tests
{
    public class AtCoder
    {
        public static IEnumerable<object[]> Source()
        {
            var data = new[] {
                (InOut.A_IN, InOut.A_OUT),
                (InOut.B_IN, InOut.B_OUT),
                (InOut.C_IN, InOut.C_OUT),
                (InOut.D_IN, InOut.D_OUT),
                (InOut.E_IN, InOut.E_OUT),
                (InOut.F_IN, InOut.F_OUT),
            };
            foreach (var (input, output) in data)
            {
                if (!string.IsNullOrEmpty(input) && !string.IsNullOrEmpty(output))
                    yield return new[] { input, output };
            }
        }

        static Regex doubleRegex = new Regex(@"^\d+\.\d+$", RegexOptions.IgnoreCase);
        [Theory(Timeout = 200)]
        [MemberData(nameof(Source))]
        public void FromSource(string input, string output)
        {
            var encoding = new UTF8Encoding(false);
            using var inSteam = new MemoryStream(encoding.GetBytes(input));
            using var outStream = new MemoryStream(30 * 100000);
            var cr = new ConsoleReader(inSteam, encoding);
            var cw = new ConsoleWriter(outStream, encoding);
            new Program(cr, cw).Run();

            var result = encoding.GetString(outStream.ToArray());
            if (doubleRegex.IsMatch(output))
                Assert.Equal(double.Parse(output), double.Parse(result), 10);
            else
                Assert.Equal(Normalize(output), Normalize(result));
        }
        private string Normalize(string s) => s.Replace("\r\n", "\n").Trim();
    }
}