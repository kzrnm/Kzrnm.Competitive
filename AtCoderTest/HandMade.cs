using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AtCoderProject;
using Xunit;

public class HandMade
{
    class MyStringBuilder : IEnumerable
    {
        private readonly StringBuilder sb = new StringBuilder();
        public override string ToString() => sb.ToString();
        public void Add(string s) => sb.AppendLine(s);
        IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
    }
    [Fact(Timeout = 2000)]
    public void TestByLogicalInput()
    {
        var sb = new MyStringBuilder
        {

        };
        var input = sb.ToString();
        if (string.IsNullOrEmpty(input)) return;
        using var sr = new StringReader(input);
        var result = new Program(new ConsoleReader(sr)).Calc();
        Console.WriteLine(result);
    }
}
