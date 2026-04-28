namespace Kzrnm.Competitive.Testing.MathNS.Matrix;

public class WriteMatrixTest
{
    [Test]
    public async Task Matrix2x2()
    {
        var utf8Wrapper = new Utf8ConsoleWriterWrapper();
        using (var cw = utf8Wrapper.GetWriter())
        {
            cw.WriteGrid(new Matrix2x2<int>((1, 2), (3, 4)));
        }
        await utf8Wrapper.Read().Should().BeEqualTo("""
        1 2
        3 4

        """.Replace("\r\n", "\n"));
    }
    [Test]
    public async Task ArrayMatrix()
    {
        var utf8Wrapper = new Utf8ConsoleWriterWrapper();
        using (var cw = utf8Wrapper.GetWriter())
        {
            cw.WriteGrid(new ArrayMatrix<int>(new int[2, 3] {
                { 1, 2, 3, },
                { -1, -2, -3, },
            }));
        }
        await utf8Wrapper.Read().Should().BeEqualTo("""
        1 2 3
        -1 -2 -3

        """.Replace("\r\n", "\n"));
    }
}