using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.IO
{
    public static class AsciiReaderExtension
    {
        /// <summary>
        /// <see cref="ConsoleReader.AsciiSpan(int)"/> の各要素から <paramref name="diff"/> を引いた配列を返します。
        /// </summary>
        /// <param name="cr"></param>
        /// <param name="diff"></param>
        /// <returns></returns>
        [凾(256)]
        public static short[] AsciiToNum(this ConsoleReader cr, int diff = 'A')
        {
            var arr = cr.AsciiSpan();
            var ret = new short[arr.Length];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = (short)(arr[i] - diff);
            return ret;
        }
    }
}