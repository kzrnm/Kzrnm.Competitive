using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AtCoderProject;
using Xunit;

public class HandMade
{
    [Fact(Timeout = 2000)]
    public void TestByLogicalInput()
    {
        var sb = new StringBuilder();
        {

        }
        var input = sb.ToString();
        if (string.IsNullOrEmpty(input)) return;
        using var sr = new StringReader(input);
        var result = new Program(new ConsoleReader(sr)).Calc();
        Console.WriteLine(result);
    }
}
