using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.Internal
{
    public static class Montgomery
    {
        /// <summary>
        /// Montgomery の r ?
        /// </summary>
        /// <param name="m">Mod</param>
        /// <returns></returns>
        [凾(256)]
        public static uint GetR(uint m)
        {
            var r = m;
            r *= 2 - m * r;
            r *= 2 - m * r;
            r *= 2 - m * r;
            r *= 2 - m * r;
            return r;
        }
        /// <summary>
        /// Montgomery の n2 ?
        /// </summary>
        /// <param name="m">Mod</param>
        /// <returns></returns>
        [凾(256)]
        public static uint GetN2(uint m) => (uint)(((ulong)-m) % m);
    }
}
