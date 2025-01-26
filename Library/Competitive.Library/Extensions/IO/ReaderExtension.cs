using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.IO
{
    public static class _ReaderExtension
    {
        /// <summary>
        /// <see cref="ConsoleReader.Ascii()"/> の各要素から <paramref name="diff"/> を引いた配列を返します。
        /// </summary>
        /// <param name="cr"></param>
        /// <param name="diff"></param>
        /// <returns></returns>
        [凾(256)]
        public static byte[] AsciiToNum(this ConsoleReader cr, int diff = 'A')
        {
            var a = cr.Ascii().d;
            for (int i = 0; i < a.Length; i++)
                a[i] -= (byte)diff;
            return a;
        }
    }
}