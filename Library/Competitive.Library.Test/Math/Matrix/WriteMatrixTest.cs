using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kzrnm.Competitive.Testing.MathNS.Matrix
{
    public class WriteMatrixTest
    {
        [Fact]
        public void Matrix2x2()
        {
            var utf8Wrapper = new Utf8ConsoleWriterWrapper();
            using (var cw = utf8Wrapper.GetWriter())
            {
                cw.WriteGrid(new Matrix2x2<int>((1, 2), (3, 4)));
            }
            utf8Wrapper.Read().ShouldBe("""
            1 2
            3 4

            """.Replace("\r\n", "\n"));
        }
        [Fact]
        public void ArrayMatrix()
        {
            var utf8Wrapper = new Utf8ConsoleWriterWrapper();
            using (var cw = utf8Wrapper.GetWriter())
            {
                cw.WriteGrid(new ArrayMatrix<int>(new int[2, 3] {
                    { 1, 2, 3, },
                    { -1, -2, -3, },
                }));
            }
            utf8Wrapper.Read().ShouldBe("""
            1 2 3
            -1 -2 -3

            """.Replace("\r\n", "\n"));
        }
    }
}
