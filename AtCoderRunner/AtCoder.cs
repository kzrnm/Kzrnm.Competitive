﻿using AtCoderProject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace AtCoderRunner
{
    public class AtCoder
    {
        private class ResouceSource : TheoryData<string, string>
        {
            public ResouceSource()
            {
                Add(InOut.A_IN, InOut.A_OUT);
                Add(InOut.B_IN, InOut.B_OUT);
                Add(InOut.C_IN, InOut.C_OUT);
                Add(InOut.D_IN, InOut.D_OUT);
                Add(InOut.E_IN, InOut.E_OUT);
                Add(InOut.F_IN, InOut.F_OUT);
            }
            public new void Add(string p1, string p2)
            {
                if (!string.IsNullOrEmpty(p1) && !string.IsNullOrEmpty(p2))
                    base.Add(p1, p2);
            }
        }

        static Regex doubleRegex = new Regex(@"^\d+\.\d+$", RegexOptions.IgnoreCase);
        [Theory(Timeout = 200)]
        [ClassData(typeof(ResouceSource))]
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