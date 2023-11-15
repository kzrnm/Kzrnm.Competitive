using Kzrnm.Competitive.Internal;
using Kzrnm.Competitive.IO;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __Matrix_WriteGrid
    {
        [凾(256)]
        public static void WriteGrid<TSelf, T>(this Utf8ConsoleWriter cw, IMatrix<TSelf, T> m)
            where TSelf : IMatrix<TSelf, T>
        {
            var h = m.Height;
            var w = m.Width;
            var v = m.AsSpan();
            for (int i = 0; i < h; i++)
                cw.WriteLineJoin(v.Slice(i * w, w));
        }
    }
}