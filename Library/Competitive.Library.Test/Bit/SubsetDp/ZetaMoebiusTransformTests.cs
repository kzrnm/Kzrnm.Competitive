using System.Linq;
using Kzrnm.Competitive;

namespace Kzrnm.Competitive.Testing.Bit.SubsetDp;

public class ZetaMoebiusTransformTests
{
    [Theory]
    [InlineData(1 << 1)]
    [InlineData(1 << 2)]
    [InlineData(1 << 3)]
    [InlineData(1 << 4)]
    public void SupersetZetaMoebiusTransform(int length)
    {
        var a = new int[length];
        var b = new int[length];
        for (int i = 0; i < a.Length; i++)
            a[i] = 1 << i;

        for (int i = 0; i < b.Length; i++)
            for (int j = i; j < a.Length; j++)
                if ((i & j) == i)
                    b[i] += a[j];

        var a2 = a.ToArray();
        var b2 = b.ToArray();

        ZetaMoebiusTransform.SupersetZetaTransform(a2);
        ZetaMoebiusTransform.SupersetMoebiusTransform(b2);

        a2.ShouldBe(b);
        b2.ShouldBe(a);
    }

    [Theory]
    [InlineData(1 << 1)]
    [InlineData(1 << 2)]
    [InlineData(1 << 3)]
    [InlineData(1 << 4)]
    public void SubsetZetaMoebiusTransform(int length)
    {
        var a = new int[length];
        var b = new int[length];
        for (int i = 0; i < a.Length; i++)
            a[i] = 1 << i;

        for (int i = 0; i < b.Length; i++)
            for (int j = 0; j <= i; j++)
                if ((i & j) == j)
                    b[i] += a[j];

        var a2 = a.ToArray();
        var b2 = b.ToArray();

        ZetaMoebiusTransform.SubsetZetaTransform(a2);
        ZetaMoebiusTransform.SubsetMoebiusTransform(b2);

        a2.ShouldBe(b);
        b2.ShouldBe(a);
    }
}
