using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AtCoderProject;
using Xunit;

namespace AtCoderTest
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
                if (input != "" && output != "")
                    yield return new[] { input, output };
            }
        }

        [Theory(Timeout = 200)]
        [MemberData(nameof(Source))]
        [DebuggerHidden]
        public void FromSource(string input, string output)
        {
            var inputReader = new ConsoleReader(new StringReader(input));
            Assert.Equal(output.Replace("\r\n", "\n").Trim(), new Program(inputReader).Calc().ToString());
        }
    }
}
