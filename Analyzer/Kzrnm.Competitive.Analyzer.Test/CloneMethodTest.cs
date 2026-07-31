using VerifyCS = Kzrnm.Competitive.Analyzer.Test.CSharpCodeFixVerifier<
    Kzrnm.Competitive.Analyzer.CloneMethod.Analyzer,
    Kzrnm.Competitive.Analyzer.CloneMethod.CodeFixProvider>;

namespace Kzrnm.Competitive.Analyzer.Test;

public class CloneMethodTest
{
    [Test]
    public async Task Empty(CancellationToken cancellationToken)
    {
        // lang=C#
        var source = """
using System;
public class Hoge : ICloneable
{
    object ICloneable.Clone() => new Hoge();
    public Hoge Clone() => new Hoge();
}
public static class Example
{
    public static object NotKZCOMPETITIVE0005(Hoge obj, int[] arr)
    {
        obj.ToString();
        ((ICloneable)obj).Clone();
        ((ICloneable)arr).Clone();
        _ = ((int[])arr.Clone());
        _ = arr.Clone() as int[];
        return obj.Clone();
    }
}
""";

        await VerifyCS.VerifyAnalyzerAsync(source, [], cancellationToken);
    }
    [Test]
    public async Task Hit(CancellationToken cancellationToken)
    {
        // lang=C#
        var source = """
using System;
public class Hoge : ICloneable
{
    public object Clone() => new Hoge();
}
public static class Example
{
    public static object KZCOMPETITIVE0005()
    {
        return new Hoge().Clone();
    }
    public static object KZCOMPETITIVE0005(int[] obj)
    {
        return obj.Clone();
    }
}
""";

        // lang=C#
        var fixedSource = """
using System;
public class Hoge : ICloneable
{
    public object Clone() => new Hoge();
}
public static class Example
{
    public static object KZCOMPETITIVE0005()
    {
        return ((Hoge)new Hoge().Clone());
    }
    public static object KZCOMPETITIVE0005(int[] obj)
    {
        return ((int[])obj.Clone());
    }
}
""";
        await VerifyCS.VerifyCodeFixAsync(source,
            [
                VerifyCS.Diagnostic().WithSpan(10, 16, 10, 34),
                VerifyCS.Diagnostic().WithSpan(14, 16, 14, 27),
            ],
            fixedSource,
            cancellationToken);
    }
}
